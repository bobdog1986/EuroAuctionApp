using EuroAuctionApp.CoreViews.Helpers;
using EuroAuctionApp.CoreViews.Models;
using EuroAuctionApp.CSV.Interfaces;
using EuroAuctionApp.DAL.Entites;
using EuroAuctionApp.DAL.Services;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.Infra.Events;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EuroAuctionApp.Infra.Helpers;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows;
using Notifications.Wpf;
using EuroAuctionApp.DAL.Interfaces;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class FileOperationViewModel : ViewModelBase
    {
        public List<AuctionDataModel> AuctionDataCollection;
        private static readonly string quoteBoard = "QuoteBoard";

        //private static string DbPath
        //{
        //    get
        //    {
        //        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), KeyNames.AppName, "DataBase");
        //    }
        //} 

        private DateTime? auctionDatePickerDate;
        private IAuctionRepository auctionRepository;
        private ICsvService csvHelper;
        private readonly NotificationManager _notificationManager = new NotificationManager();


        private ObservableCollection<AuctionDataModel> filterAuctionDataCollection;

        private ObservableCollection<string> marketCollection;

        private DateTime? quoteDatePickerDate;

        private ObservableCollection<QuoteFileModel> quoteFileCollection;

        private string selectedBackupPath;

        private string selectedMarket;

        public FileOperationViewModel()
        {
            LogInfo("FileOperationViewModel");
            csvHelper = Resolve<ICsvService>();
            auctionRepository = Resolve<IAuctionRepository>();

            InitCommands();
            InitDisplay();
            SubscribeEvents();

            ColorSettingHelper.UpdateProfitSettings();
        }

        public DelegateCommand AddQuoteFileCommand { get; private set; }

        public DelegateCommand AddQuoteFolderCommand { get; private set; }

        public DateTime? AuctionDatePickerDate
        {
            get { return auctionDatePickerDate; }
            set { SetProperty(ref auctionDatePickerDate, value); }
        }

        public DelegateCommand BackupQuotesCommand { get; private set; }

        public DelegateCommand CalculateQuoteCommand { get; private set; }

        public DelegateCommand CleanAuctionCommand { get; private set; }

        public DelegateCommand CleanQuotesCommand { get; private set; }

        public DelegateCommand CopyAvgProfitCommand { get; private set; }

        public DelegateCommand CopyCloseProfitCommand { get; private set; }

        public DelegateCommand CopyLastProfitCommand { get; private set; }

        public ObservableCollection<AuctionDataModel> FilterAuctionDataCollection
        {
            get { return filterAuctionDataCollection; }
            set { SetProperty(ref filterAuctionDataCollection, value); }
        }

        public ObservableCollection<string> MarketCollection
        {
            get { return marketCollection; }
            set { SetProperty(ref marketCollection, value); }
        }

        public DateTime? QuoteDatePickerDate
        {
            get { return quoteDatePickerDate; }
            set { SetProperty(ref quoteDatePickerDate, value); }
        }

        public ObservableCollection<QuoteFileModel> QuoteFileCollection
        {
            get { return quoteFileCollection; }
            set { SetProperty(ref quoteFileCollection, value); }
        }

        public string SelectedBackupPath
        {
            get { return selectedBackupPath; }
            set { SetProperty(ref selectedBackupPath, value); }
        }

        public string SelectedMarket
        {
            get { return selectedMarket; }
            set { SetProperty(ref selectedMarket, value); }
        }

        public DelegateCommand SelectMarketCommand { get; private set; }

        public DelegateCommand WriteToDbCommand { get; private set; }

        private void AddQuoteFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.RestoreDirectory = true;
                dialog.Filter = "CSV Files|*.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string file = dialog.FileName;

                    QuoteFileCollection = QuoteFileCollection ?? new ObservableCollection<QuoteFileModel>();
                    QuoteFileCollection.Add(new QuoteFileModel(file));
                }
            }
            catch (Exception ex)
            {
                LogError("OpenFile() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private async void AddQuoteFolder()
        {
            string lastQuoteFolder = await AppSettingHelper.TryGetSettingAsync(KeyNames.LastQuoteFolderKey);

            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                dialog.SelectedPath = lastQuoteFolder;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var path = dialog.SelectedPath;
                    PublishStatusMessage(path);

                    var files = Directory.GetFiles(path, "QuoteBoardExport_*.csv");

                    if (files != null && files.Count() > 0)
                    {
                        QuoteFileCollection = new ObservableCollection<QuoteFileModel>();

                        foreach (var file in files)
                        {
                            QuoteFileCollection.Add(new QuoteFileModel(file));
                        }
                    }

                    await AppSettingHelper.TryInsertSettingAsync(KeyNames.LastQuoteFolderKey, path);

                    IdentifyQuoteFiles();
                }
            }
            catch (Exception ex)
            {
                LogError("DoSaveButtonClick() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void CalculateAuctionData()
        {
            foreach (var data in AuctionDataCollection)
            {
                data.Calculate();
            }
        }

        private async Task ShowMessageWithTitleAsync(string header, string message)
        {
            await MainWindow?.ShowMessageAsync(header, message);
        }

        private void ShowMessageWithTitle(string header, string message)
        {
            MainWindow?.ShowModalMessageExternal(header, message);
        }

        private MetroWindow MainWindow => System.Windows.Application.Current.MainWindow as MetroWindow;

        private async void CalculateQuoteFiles()
        {
            try
            {
                AuctionDataCollection.Clear();

                ImportAllQuoteFileData();

                // Show...
                string header = "Title";
                string message = "Calculating...";

                ProgressDialogController controller = await MainWindow.ShowProgressAsync(header, message);
                controller.SetIndeterminate();

                await Task.Run(
                    () =>
                    {
                        CalculateAuctionData();

                        UpdateMarketListAndSelectFirst();

                        UpdateFilterStockDataCollection();
                    });

                // Close...
                await controller.CloseAsync();
            }
            catch (Exception ex)
            {
                LogError("CalculateQuoteFiles() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void CleanAuctionData()
        {
            AuctionDataCollection.Clear();

            SelectedMarket = string.Empty;

            MarketCollection.Clear();

            FilterAuctionDataCollection.Clear();
        }

        private void CleanQuotesData()
        {
            QuoteFileCollection.Clear();
        }

        private void CopyAvgProfitData()
        {
            try
            {
                if (FilterAuctionDataCollection != null && FilterAuctionDataCollection.Count > 0)
                {
                    var data = FilterAuctionDataCollection.Select(o => o.AvgProfitPercent.ToString());
                    System.Windows.Forms.Clipboard.SetText(string.Join(Environment.NewLine, data));

                    NotifyInfo("Info","Copy Avg!");
                }
            }
            catch (Exception ex)
            {
                LogError("CopyAvgProfitData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void CopyCloseProfitData()
        {
            try
            {
                if (FilterAuctionDataCollection != null && FilterAuctionDataCollection.Count > 0)
                {
                    var data = FilterAuctionDataCollection.Select(o => o.CloseProfitPercent.ToString());
                    System.Windows.Forms.Clipboard.SetText(string.Join(Environment.NewLine, data));

                    NotifyInfo("Info", "Copy Close!");

                }
            }
            catch (Exception ex)
            {
                LogError("CopyCloseProfitData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void CopyLastProfitData()
        {
            try
            {
                if (FilterAuctionDataCollection != null && FilterAuctionDataCollection.Count > 0)
                {
                    var data = FilterAuctionDataCollection.Select(o => o.LastProfitPercent.ToString());
                    System.Windows.Forms.Clipboard.SetText(string.Join(Environment.NewLine, data));

                    NotifyInfo("Info", "Copy Last!");

                }
            }
            catch (Exception ex)
            {
                LogError("CopyLast5ProfitData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private async void DoSelectMarket()
        {
            await Task.Run(()=> UpdateFilterStockDataCollection());
        }

        private async Task<string> GetBackupPath()
        {
            string backupPath = await AppSettingHelper.TryGetSettingAsync(KeyNames.BackupPathKey);

            if (string.IsNullOrEmpty(backupPath))
            {
                backupPath = @"D:\EuroAuctionApp Data";
            }

            return backupPath;
        }

        private async Task<string> GetQuoteBackupRootPath()
        {
            string backupPath = await GetBackupPath();
            return Path.Combine(backupPath, quoteBoard);
        }

        private DateTime GetQuoteFileDateTime(string filename)
        {
            return QuoteFileModel.ParseDateTime(filename);
        }

        private async Task<string> GetQuoteFileSavePath(DateTime dateTime)
        {
            string year= dateTime.ToString("yyyy");
            string yearAndMonth = dateTime.ToString("yyyy-MM");
            string quoteFileRootPath = await GetQuoteBackupRootPath();
            return Path.Combine(quoteFileRootPath, year,yearAndMonth, dateTime.ToString("yyyy-MM-dd"));
        }

        private void IdentifyQuoteFiles()
        {
            try
            {
                SetQuoteDatePickerToFirst();

                var groups = QuoteFileCollection.ToList().GroupBy(o => o.FirstMarket);

                foreach (var group in groups)
                {
                    var files = group.Select(o => o).ToList();
                    if (files.Count == 3)
                    {
                        files[0].Is25File = true;
                        files[1].IsCloseFile = true;
                        files[2].IsAuctionFile = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("IdentifyQuoteFiles() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void ImportAllQuoteFileData()
        {
            try
            {
                if (QuoteFileCollection != null && QuoteFileCollection.Count > 0)
                {
                    AuctionDatePickerDate = QuoteFileCollection[0].DateTime;

                    foreach (var quote in QuoteFileCollection)
                    {
                        if (quote.Is25File)
                        {
                            ReadLast5CsvFileData(quote.FileName);
                        }
                        else if (quote.IsCloseFile)
                        {
                            ReadCloseCsvFileData(quote.FileName);
                        }
                        else if (quote.IsAuctionFile)
                        {
                            ReadAuctionCsvFileData(quote.FileName);
                        }
                        else
                        {
                            //
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ImportAllQuoteFileData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private async void InitBackupRootPath()
        {
            SelectedBackupPath = await GetBackupPath();
        }

        private void InitCommands()
        {
            AddQuoteFileCommand = new DelegateCommand(AddQuoteFile);
            AddQuoteFolderCommand = new DelegateCommand(AddQuoteFolder);

            CalculateQuoteCommand = new DelegateCommand(CalculateQuoteFiles);
            SelectMarketCommand = new DelegateCommand(DoSelectMarket);
            BackupQuotesCommand = new DelegateCommand(MoveQuotesToDefaultFolder);
            CleanQuotesCommand = new DelegateCommand(CleanQuotesData);

            CleanAuctionCommand = new DelegateCommand(CleanAuctionData);
            WriteToDbCommand = new DelegateCommand(WriteToDb);

            CopyLastProfitCommand = new DelegateCommand(CopyLastProfitData);
            CopyCloseProfitCommand = new DelegateCommand(CopyCloseProfitData);
            CopyAvgProfitCommand = new DelegateCommand(CopyAvgProfitData);
        }

        private void InitDisplay()
        {
            InitBackupRootPath();

            AuctionDataCollection = new List<AuctionDataModel>();
            QuoteFileCollection = new ObservableCollection<QuoteFileModel>();
            MarketCollection = new ObservableCollection<string>();
        }

        private void MoveFile(string source, string dest)
        {
            try
            {
                File.Copy(source, dest, true);
                File.SetAttributes(dest, FileAttributes.Normal);

                File.SetAttributes(source, FileAttributes.Normal);
                File.Delete(source);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private async void MoveQuotesToDefaultFolder()
        {
            try
            {
                if (QuoteDatePickerDate == null)
                {
                    PublishStatusMessage("select Quote picker");
                    return;
                }

                string targetPath = await GetQuoteFileSavePath(QuoteDatePickerDate.Value);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                //PublishStatusMessage(targetPath);

                // Show...
                string header = "Back up quotes";
                string message = "To : "+targetPath;

                ProgressDialogController controller = await MainWindow.ShowProgressAsync(header, message);
                controller.SetIndeterminate();

                await Task.Run(() => 
                {
                    foreach (var quote in QuoteFileCollection)
                    {
                        MoveFile(quote.FileName, Path.Combine(targetPath, quote.ShortName));
                    }
                } );

                await controller.CloseAsync();

            }
            catch (Exception ex)
            {
                LogError("MoveQuotesToDefaultFolder() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void OnProfitSettingChanged()
        {
            ColorSettingHelper.UpdateProfitSettings();
        }

        private void PublishStatusMessage(string message)
        {
            EventAggregator.GetEvent<StatusMessageEvent>().Publish(message);
        }

        private void ReadAuctionCsvFileData(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    var records = csvHelper.ReadQuoteAllLines(filename);
                    var dateTime = GetQuoteFileDateTime(filename);

                    if (records != null && records.Count > 0)
                    {
                        AuctionDataCollection = AuctionDataCollection ?? new List<AuctionDataModel>();

                        foreach (var record in records)
                        {
                            var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                            if (old == null)
                            {
                                old = new AuctionDataModel()
                                {
                                    Symbol = record.Symbol,
                                    AuctionPrice = record.LastPrice,
                                    VolumeAtAuction = record.Volume
                                };
                                AuctionDataCollection.Add(old);
                            }
                            else
                            {
                                old.AuctionPrice = record.LastPrice;
                                old.VolumeAtAuction = record.Volume;
                            }
                            old.AuctionDateTime = dateTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ReadAuctionCsvFileData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);

            }
        }

        private void ReadCloseCsvFileData(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    var records = csvHelper.ReadQuoteAllLines(filename);
                    var dateTime = GetQuoteFileDateTime(filename);

                    if (records != null && records.Count > 0)
                    {
                        AuctionDataCollection = AuctionDataCollection ?? new List<AuctionDataModel>();

                        foreach (var record in records)
                        {
                            var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                            if (old == null)
                            {
                                old = new AuctionDataModel()
                                {
                                    Symbol = record.Symbol,
                                    ClosePrice = record.LastPrice,
                                    VolumeAtClose = record.Volume
                                };
                                AuctionDataCollection.Add(old);
                            }
                            else
                            {
                                old.ClosePrice = record.LastPrice;
                                old.VolumeAtClose = record.Volume;
                            }
                            old.AuctionDateTime = dateTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ReadCloseCsvFileData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);

            }
        }

        private void ReadLast5CsvFileData(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    var records = csvHelper.ReadQuoteAllLines(filename);

                    var dateTime = GetQuoteFileDateTime(filename);

                    if (records != null && records.Count > 0)
                    {
                        AuctionDataCollection = AuctionDataCollection ?? new List<AuctionDataModel>();

                        foreach (var record in records)
                        {
                            var old = AuctionDataCollection.FirstOrDefault(o => o.Symbol == record.Symbol);
                            if (old == null)
                            {
                                old = new AuctionDataModel()
                                {
                                    Symbol = record.Symbol,
                                    LastPrice = record.LastPrice,
                                    VolumeAtLast = record.Volume
                                };
                                AuctionDataCollection.Add(old);
                            }
                            else
                            {
                                old.LastPrice = record.LastPrice;
                                old.VolumeAtLast = record.Volume;
                            }

                            old.AuctionDateTime = dateTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("ReadLast5CsvFileData() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);

            }
        }

        private void SetQuoteDatePickerToFirst()
        {
            try
            {
                if (QuoteFileCollection != null && QuoteFileCollection.Count > 0)
                {
                    QuoteDatePickerDate = QuoteFileCollection[0].DateTime;
                }
            }
            catch (Exception ex)
            {
                LogError("SetQuoteDatePickerToFirst() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void SubscribeEvents()
        {
            EventAggregator.GetEvent<ProfitSettingsChangedEvent >().Subscribe(OnProfitSettingChanged);
        }

        private void UpdateFilterStockDataCollection()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedMarket))
                {
                    FilterAuctionDataCollection = new ObservableCollection<AuctionDataModel>(AuctionDataCollection);
                }
                else
                {
                    FilterAuctionDataCollection = new ObservableCollection<AuctionDataModel>(AuctionDataCollection.Where(o => o.MarketCode == SelectedMarket));
                }
            }
            catch (Exception ex)
            {
                LogError("UpdateFilterStockDataCollection() : ex = " + ex.Message);
                PublishStatusMessage(ex.Message);
            }
        }

        private void UpdateMarketListAndSelectFirst()
        {
            MarketCollection = new ObservableCollection<string>(AuctionDataCollection.Select(o => o.MarketCode).Distinct().OrderBy(o => o));

            if (MarketCollection.Count > 0)
            {
                SelectedMarket = MarketCollection[0];
            }
        }

        private async void WriteToDb()
        {
            //EventAggregator.GetEvent<NotificationInfoEvent>().Publish("Write db");
            // Show...
            string header = "Write to database";
            string message = "waiting...";

            ProgressDialogController controller = await MainWindow.ShowProgressAsync(header, message);
            controller.SetIndeterminate();

            await Task.Run(() => WriteAllRecords());

            await controller.CloseAsync();
        }

        private void WriteAllRecords()
        {
            try
            {
                foreach (var market in MarketCollection)
                {
                    var records = AuctionDataCollection.Where(o => o.MarketCode == market);
                    foreach (var record in records)
                    {
                        var entry = new AuctionRecord()
                        {
                            SymbolName = record.Symbol,
                            AuctionDate = AuctionDatePickerDate.Value,
                            AuctionDateString = AuctionDatePickerDate.Value.ToString(FormatterHelper.ShortDateFormat),
                            AuctionDateNumber = AuctionDatePickerDate.Value.Year * 10000 + AuctionDatePickerDate.Value.Month * 100 + AuctionDatePickerDate.Value.Day,

                            AuctionPrice = record.AuctionPrice,
                            ClosePrice = record.ClosePrice,
                            LastPrice = record.LastPrice,

                            VolumeAtAuction = record.VolumeAtAuction,
                            VolumeAtClose = record.VolumeAtClose,
                            VolumeAtLast = record.VolumeAtLast,
                        };

                        auctionRepository.InsertOrUpdateRecord(entry);
                    }

                    //string indiceName = "Indice." + market;

                    //double indiceAvgAuctionPrice = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.AuctionPrice; }) / records.Count(),3);
                    //double indiceAvgClosePrice = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.ClosePrice; }) / records.Count(),3);
                    //double indiceAvgLastPrice = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.Last5Price; }) / records.Count(),3);

                    //int indiceSumVolumeAtAuction = records.Aggregate(0, (current, next) => { return current + next.VolumeAtClose; });
                    //int indiceSumVolumeAtClose = records.Aggregate(0, (current, next) => { return current + next.VolumeAtClose; });
                    //int indiceSumVolumeAtLast = records.Aggregate(0, (current, next) => { return current + next.VolumeAt25; });

                    //int indiceSumAuctionVolume = records.Aggregate(0, (current, next) => { return current + next.AuctionVolume; });
                    //int indiceSumLastTradeVolume = records.Aggregate(0, (current, next) => { return current + next.LastTradeVolume; });

                    //double indiceAvgAuctionVolumePercent =Math.Round( records.Aggregate(0.0, (current, next) => { return current + next.AuctionVolumePercent; }) / records.Count(),1);
                    //double indiceAvgLastTradeVolumePercent =Math.Round( records.Aggregate(0.0, (current, next) => { return current + next.Last5VolumePercent; }) / records.Count(),1);

                    //double indiceLastProfitBP = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.Last5ProfitInBP; }), 1);
                    //double indiceCloseProfitBP = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.CloseProfitInBP; }), 1);
                    //double indiceAvgProfitBP = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.AvgProfitInBP; }), 1);

                    //double indiceLastProfitPercent = Math.Round( records.Aggregate(0.0, (current, next) => { return current + next.Last5ProfitInPercent; }),3);
                    //double indiceCloseProfitPercent = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.CloseProfitInPercent; }),3);
                    //double indiceAvgProfitPercent = Math.Round(records.Aggregate(0.0, (current, next) => { return current + next.AvgProfitInPercent; }),3);

                    //var indice = new AuctionRecord()
                    //{
                    //    SymbolName = indiceName,
                    //    AuctionDateTime = AuctionDatePickerDate.Value,
                    //    AuctionDateString = AuctionDatePickerDate.Value.ToString(yyyyMMdd),

                    //    AuctionPrice = indiceAvgAuctionPrice,
                    //    ClosePrice = indiceAvgClosePrice,
                    //    LastPrice = indiceAvgLastPrice,

                    //    VolumeAtAuction = indiceSumVolumeAtAuction,
                    //    VolumeAtClose = indiceSumVolumeAtClose,
                    //    VolumeAtLast = indiceSumVolumeAtLast,
                    //};

                    //dbClient.InsertOrUpdateAuctionRecord(indice);
                }

                PublishStatusMessage("WriteToDb");
            }
            catch (Exception ex)
            {
                LogError("WriteToDb() : ex = " + ex.Message);
                PublishStatusMessage("error when write to db");
                throw;
            }

        }

        private void NotifyInfo(string title, string message, double duration=1 )
        {
            var content = new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Information
            };

            _notificationManager.Show(content,expirationTime: TimeSpan.FromSeconds(duration));
        }
    }
}
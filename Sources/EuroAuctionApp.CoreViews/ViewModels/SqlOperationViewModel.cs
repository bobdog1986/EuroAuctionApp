using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.CoreViews.Views;
using System.Data;
using System.Windows;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using System.Collections.ObjectModel;
using EuroAuctionApp.DAL;
using EuroAuctionApp.DAL.Interfaces;
using EuroAuctionApp.CoreViews.Models;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class SqlOperationViewModel:ViewModelBase
    {
        private readonly static string datePattern = "yyyy-MM-dd";
        private IAuctionRepository auctionRepository;
        public SqlOperationViewModel()
        {
            auctionRepository = Resolve<IAuctionRepository>();

            RegisterCommands();
            InitDisplay();
            Init();
            InitMarketCollection();
        }

        private void InitMarketCollection()
        {
            try
            {
                var markets = auctionRepository.GetAllMarkets().Select(o => o.Code);
                MarketCollection = new ObservableCollection<string>(markets) { };
                SelectedMarket = MarketCollection[0];
            }
            catch(Exception ex)
            {

            }
        }

        private void Init()
        {
            //var ds = new DataSet();
            QueryAuctionResult = new DataTable();
            //ds.Tables.Add(QueryAuctionResult);

            DataColumn column1 = new DataColumn("col1");
            DataColumn column2 = new DataColumn("col2");
            QueryAuctionResult.Columns.Add(column1);
            QueryAuctionResult.Columns.Add(column2);

            var row1 = QueryAuctionResult.NewRow();
            row1["col1"] = "aaa";
            row1["col2"] = "bbb";
            QueryAuctionResult.Rows.Add(row1);

        }

        private void RegisterCommands()
        {
            QueryByDayCountCommand = new DelegateCommand(DoQueryByDayCount);
            QueryByDateRangeCommand = new DelegateCommand(DoQueryByDateRange);
        }

        private void DoQueryByDayCount()
        {

        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public IEnumerable<DateTime> EachDayReverse(DateTime end, DateTime stop)
        {
            for (var day = end.Date; day.Date <= stop.Date; day = day.AddDays(-1))
                yield return day;
        }

        private string GetAuctionColumnHeader(DateTime day)
        {
            return day.ToString("MM-dd") + " (" + day.ToString("ddd").Substring(1) + ")";
        }

        private async void DoQueryByDateRange()
        {

            await Task.Run(() => QueryByDateRange()); 
        }

        private void QueryByDateRange()
        {
            try
            {
                if (StartQueryDate == null) { return; }
                if (EndQueryDate == null)
                {
                    EndQueryDate = DateTime.Now;
                }

                var records = auctionRepository.GetMarketRecordsByDateRange(SelectedMarket, StartQueryDate.Value, EndQueryDate.Value);
                if (!records.Any()) { return; }

                var symbols = records.Select(r => r.SymbolName).Distinct().ToList();
                if (!symbols.Any()) { return; }
                symbols.Sort();

                var dt = new DataTable();
                DataColumn columnSymbol = new DataColumn("Symbol");
                dt.Columns.Add(columnSymbol);

                List<DateTime> auctionDays = new List<DateTime>();

                foreach (var day in EachDay(StartQueryDate.Value, EndQueryDate.Value))
                {
                    if(day.DayOfWeek== DayOfWeek.Saturday|| day.DayOfWeek == DayOfWeek.Sunday) { continue; }

                    var dayRecords = records.Where(r => r.AuctionDateString == day.ToString(datePattern));
                    if (dayRecords.Any())
                    {
                        var dateStr = GetAuctionColumnHeader(day);
                        DataColumn col = new DataColumn(dateStr);
                        dt.Columns.Add(col);

                        auctionDays.Add(day);
                    }
                }

                foreach(var symbol in symbols)
                {
                    var row = dt.NewRow();
                    row["Symbol"] = symbol;

                    foreach (var day in auctionDays)
                    {
                        var record = records.Where(r => r.AuctionDateString == day.ToString(datePattern) && r.SymbolName==symbol).FirstOrDefault();
                        if (record != null)
                        {
                            var recordDto = new AuctionDataModel()
                            {
                                Symbol = record.SymbolName,
                                AuctionDateTime=record.AuctionDate,
                                AuctionDateString=record.AuctionDateString,
                                AuctionDateNumber=record.AuctionDateNumber,
                                LastPrice = record.LastPrice ?? double.NaN,
                                ClosePrice = record.ClosePrice ?? double.NaN,
                                AuctionPrice= record.AuctionPrice ?? double.NaN,
                                VolumeAtLast = record.VolumeAtLast ?? 0,
                                VolumeAtClose = record.VolumeAtClose ?? 0,
                                VolumeAtAuction = record.VolumeAtAuction ?? 0,
                            };

                            recordDto.Calculate();
                            row[GetAuctionColumnHeader(day)] = recordDto.AvgProfitPercent;
                            
                        }
                        else
                        {
                            row[GetAuctionColumnHeader(day)] = double.NaN;
                        }
                    }

                    dt.Rows.Add(row);
                }

                QueryAuctionResult = dt;
            }
            catch(Exception ex)
            {
                LogError(ex.Message);
            }

        }

        private void InitDisplay()
        {
            QueryDayCountCollection = new List<int> { 5, 8, 10, 12, 15, 20, 25, 30 };
            SelectedQueryDayCount = QueryDayCountCollection[3];
        }

        private DataTable queryAuctionResult = new DataTable();
        public DataTable QueryAuctionResult
        {
            get { return queryAuctionResult; }
            set { SetProperty(ref queryAuctionResult, value);}
        }

        public DelegateCommand QueryByDayCountCommand { get; private set; }
        public DelegateCommand QueryByDateRangeCommand { get; private set; }

        private List<int> queryDayCountCollection = new List<int>();

        public List<int> QueryDayCountCollection
        {
            get { return queryDayCountCollection; }
            set { SetProperty(ref queryDayCountCollection, value); }
        }

        private int selectedQueryDayCount;

        public int SelectedQueryDayCount
        {
            get { return selectedQueryDayCount; }
            set { SetProperty(ref selectedQueryDayCount, value); }
        }

        private bool isQueryByDayCount;
        private bool IsQueryByDayCount
        {
            get { return isQueryByDayCount; }
            set { SetProperty(ref isQueryByDayCount, value); }
        }


        private ObservableCollection<string> marketCollection;

        public ObservableCollection<string> MarketCollection
        {
            get { return marketCollection; }
            set { SetProperty(ref marketCollection, value); }
        }

        private string selectedMarket;

        public string SelectedMarket
        {
            get { return selectedMarket; }
            set { SetProperty(ref selectedMarket, value); }
        }

        private DateTime? startQueryDate;
        public DateTime? StartQueryDate
        {
            get { return startQueryDate; }
            set { SetProperty(ref startQueryDate, value); }
        }

        private DateTime? endQueryDate;
        public DateTime? EndQueryDate
        {
            get { return endQueryDate; }
            set { SetProperty(ref endQueryDate, value); }
        }
    }
}

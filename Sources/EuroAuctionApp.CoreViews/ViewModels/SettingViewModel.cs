using EuroAuctionApp.CoreViews.Helpers;
using EuroAuctionApp.CoreViews.Models;
using EuroAuctionApp.CSV.Interfaces;
using EuroAuctionApp.DAL.Entites;
using EuroAuctionApp.DAL.Services;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Constants;
using EuroAuctionApp.Infra.Events;
using EuroAuctionApp.Infra.Services;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Reflection;
using System.Text;
using System.IO;
using EuroAuctionApp.Infra.Helpers;
using System;

namespace EuroAuctionApp.CoreViews.ViewModels
{
    public class SettingViewModel:ViewModelBase
    {
        public SettingViewModel()
        {
            LogInfo("SettingViewModel");

            InitCommands();
            InitDisplay();
        }

        public DelegateCommand SaveProfitSettingsCommand { get; private set; }
        public DelegateCommand CancelProfitSettingsCommand { get; private set; }
        public DelegateCommand RestoreProfitSettingsCommand { get; private set; }
        public DelegateCommand SaveAuctionSettingsCommand { get; private set; }
        public DelegateCommand CancelAuctionSettingsCommand { get; private set; }
        public DelegateCommand RestoreAuctionSettingsCommand { get; private set; }

        private void InitCommands()
        {
            SaveProfitSettingsCommand = new DelegateCommand(DoSaveProfitSettings);
            CancelProfitSettingsCommand = new DelegateCommand(DoCancelProfitSettings);
            RestoreProfitSettingsCommand = new DelegateCommand(DoRestoreProfitSettings);

            SaveAuctionSettingsCommand = new DelegateCommand(DoSaveAuctionSettings);
            CancelAuctionSettingsCommand = new DelegateCommand(DoCancelAuctionSettings);
            RestoreAuctionSettingsCommand = new DelegateCommand(DoRestoreAuctionSettings);
        }

        private async void DoRestoreProfitSettings()
        {
            try
            {
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit3ColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfit3ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit2ColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfit2ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfitColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfitColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.NormalProfitColorKey, AppDefaultSettings.ValuePairs[KeyNames.NormalProfitColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfitColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfitColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit2ColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfit2ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit3ColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfit3ColorKey]);

                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit3ValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfit3ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit2ValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfit2ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfitValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpProfitValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfitValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfitValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit2ValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfit2ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit3ValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownProfit3ValueKey]);


                SelectedUpProfit3Color = (string)AppDefaultSettings.ValuePairs[KeyNames.UpProfit3ColorKey];
                SelectedUpProfit2Color = (string)AppDefaultSettings.ValuePairs[KeyNames.UpProfit2ColorKey];
                SelectedUpProfitColor = (string)AppDefaultSettings.ValuePairs[KeyNames.UpProfitColorKey];
                SelectedNormalProfitColor = (string)AppDefaultSettings.ValuePairs[KeyNames.NormalProfitColorKey];
                SelectedDownProfitColor = (string)AppDefaultSettings.ValuePairs[KeyNames.DownProfitColorKey];
                SelectedDownProfit2Color = (string)AppDefaultSettings.ValuePairs[KeyNames.DownProfit2ColorKey];
                SelectedDownProfit3Color = (string)AppDefaultSettings.ValuePairs[KeyNames.DownProfit3ColorKey];

                UpProfit3 = (double)AppDefaultSettings.ValuePairs[KeyNames.UpProfit3ValueKey];
                UpProfit2 = (double)AppDefaultSettings.ValuePairs[KeyNames.UpProfit2ValueKey];
                UpProfit = (double)AppDefaultSettings.ValuePairs[KeyNames.UpProfitValueKey];
                DownProfit = (double)AppDefaultSettings.ValuePairs[KeyNames.DownProfitValueKey];
                DownProfit2 = (double)AppDefaultSettings.ValuePairs[KeyNames.DownProfit2ValueKey];
                DownProfit3 = (double)AppDefaultSettings.ValuePairs[KeyNames.DownProfit3ValueKey];

                OnProfitSettingsRestored();
            }
            catch (Exception ex)
            {

            }
        }

        private async void DoSaveProfitSettings()
        {
            try
            {
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit3ColorKey, SelectedUpProfit3Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit2ColorKey, SelectedUpProfit2Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfitColorKey, SelectedUpProfitColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.NormalProfitColorKey, SelectedNormalProfitColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfitColorKey, SelectedDownProfitColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit2ColorKey, SelectedDownProfit2Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit3ColorKey, SelectedDownProfit3Color);

                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit3ValueKey, UpProfit3);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfit2ValueKey, UpProfit2);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpProfitValueKey, UpProfit);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfitValueKey, DownProfit);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit2ValueKey, DownProfit2);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownProfit3ValueKey, DownProfit3);

                OnProfitSettingsSaved();
            }
            catch(Exception ex)
            {

            }
        }

        private async void DoCancelProfitSettings()
        {
            try
            {
                SelectedUpProfit3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit3ColorKey);
                SelectedUpProfit2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit2ColorKey);
                SelectedUpProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfitColorKey);
                SelectedNormalProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalProfitColorKey);
                SelectedDownProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfitColorKey);
                SelectedDownProfit2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit2ColorKey);
                SelectedDownProfit3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit3ColorKey);

                UpProfit3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit3ValueKey);
                UpProfit2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit2ValueKey);
                UpProfit = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfitValueKey);
                DownProfit = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfitValueKey);
                DownProfit2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit2ValueKey);
                DownProfit3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit3ValueKey);
            }
            catch (Exception ex)
            {

            }
        }
        private async void DoRestoreAuctionSettings()
        {
            try
            {
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction3ColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuction3ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction2ColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuction2ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuctionColorKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuctionColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.NormalAuctionColorKey, AppDefaultSettings.ValuePairs[KeyNames.NormalAuctionColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuctionColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuctionColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction2ColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuction2ColorKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction3ColorKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuction3ColorKey]);

                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction3ValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuction3ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction2ValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuction2ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuctionValueKey, AppDefaultSettings.ValuePairs[KeyNames.UpAuctionValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuctionValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuctionValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction2ValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuction2ValueKey]);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction3ValueKey, AppDefaultSettings.ValuePairs[KeyNames.DownAuction3ValueKey]);


                SelectedUpAuction3Color = (string)AppDefaultSettings.ValuePairs[KeyNames.UpAuction3ColorKey];
                SelectedUpAuction2Color = (string)AppDefaultSettings.ValuePairs[KeyNames.UpAuction2ColorKey];
                SelectedUpAuctionColor = (string)AppDefaultSettings.ValuePairs[KeyNames.UpAuctionColorKey];
                SelectedNormalAuctionColor = (string)AppDefaultSettings.ValuePairs[KeyNames.NormalAuctionColorKey];
                SelectedDownAuctionColor = (string)AppDefaultSettings.ValuePairs[KeyNames.DownAuctionColorKey];
                SelectedDownAuction2Color = (string)AppDefaultSettings.ValuePairs[KeyNames.DownAuction2ColorKey];
                SelectedDownAuction3Color = (string)AppDefaultSettings.ValuePairs[KeyNames.DownAuction3ColorKey];

                UpAuction3 = (double)AppDefaultSettings.ValuePairs[KeyNames.UpAuction3ValueKey];
                UpAuction2 = (double)AppDefaultSettings.ValuePairs[KeyNames.UpAuction2ValueKey];
                UpAuction = (double)AppDefaultSettings.ValuePairs[KeyNames.UpAuctionValueKey];
                DownAuction = (double)AppDefaultSettings.ValuePairs[KeyNames.DownAuctionValueKey];
                DownAuction2 = (double)AppDefaultSettings.ValuePairs[KeyNames.DownAuction2ValueKey];
                DownAuction3 = (double)AppDefaultSettings.ValuePairs[KeyNames.DownAuction3ValueKey];

                OnAuctionSettingsRestored();
            }
            catch (Exception ex)
            {

            }
        }

        private async void DoSaveAuctionSettings()
        {
            try
            {
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction3ColorKey, SelectedUpAuction3Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction2ColorKey, SelectedUpAuction2Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuctionColorKey, SelectedUpAuctionColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.NormalAuctionColorKey, SelectedNormalAuctionColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuctionColorKey, SelectedDownAuctionColor);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction2ColorKey, SelectedDownAuction2Color);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction3ColorKey, SelectedDownAuction3Color);

                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction3ValueKey, UpAuction3);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuction2ValueKey, UpAuction2);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.UpAuctionValueKey, UpAuction);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuctionValueKey, DownAuction);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction2ValueKey, DownAuction2);
                await AppSettingHelper.TryInsertSettingAsync(KeyNames.DownAuction3ValueKey, DownAuction3);

                OnAuctionSettingsSaved();
            }
            catch (Exception ex)
            {

            }
        }
        private async void DoCancelAuctionSettings()
        {
            try
            {
                SelectedUpAuction3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction3ColorKey);
                SelectedUpAuction2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction2ColorKey);
                SelectedUpAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuctionColorKey);
                SelectedNormalAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalAuctionColorKey);
                SelectedDownAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuctionColorKey);
                SelectedDownAuction2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction2ColorKey);
                SelectedDownAuction3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction3ColorKey);

                UpAuction3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction3ValueKey);
                UpAuction2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction2ValueKey);
                UpAuction = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuctionValueKey);
                DownAuction = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuctionValueKey);
                DownAuction2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuction2ValueKey);
                DownAuction3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuction3ValueKey);
            }
            catch (Exception ex)
            {

            }
        }
        private void OnProfitSettingsSaved()
        {
            EventAggregator.GetEvent<ProfitSettingsChangedEvent >().Publish();
        }

        private void OnProfitSettingsRestored()
        {
            EventAggregator.GetEvent<ProfitSettingsChangedEvent >().Publish();
        }

        private void OnAuctionSettingsSaved()
        {
            EventAggregator.GetEvent<AuctionSettingsChangedEvent>().Publish();
        }

        private void OnAuctionSettingsRestored()
        {
            EventAggregator.GetEvent<AuctionSettingsChangedEvent>().Publish();
        }

        private async void InitDisplay()
        {
            InitAllColors();

            SelectedUpProfit3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit3ColorKey);
            SelectedUpProfit2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfit2ColorKey);
            SelectedUpProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpProfitColorKey);
            SelectedNormalProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalProfitColorKey);
            SelectedDownProfitColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfitColorKey);
            SelectedDownProfit2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit2ColorKey);
            SelectedDownProfit3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownProfit3ColorKey);

            UpProfit3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit3ValueKey);
            UpProfit2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfit2ValueKey);
            UpProfit = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpProfitValueKey);
            DownProfit = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfitValueKey);
            DownProfit2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit2ValueKey);
            DownProfit3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit3ValueKey);

            //
            SelectedUpAuction3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction3ColorKey);
            SelectedUpAuction2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuction2ColorKey);
            SelectedUpAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.UpAuctionColorKey);
            SelectedNormalAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.NormalAuctionColorKey);
            SelectedDownAuctionColor = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuctionColorKey);
            SelectedDownAuction2Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction2ColorKey);
            SelectedDownAuction3Color = await AppSettingHelper.TryGetSettingAsync(KeyNames.DownAuction3ColorKey);

            UpAuction3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction3ValueKey);
            UpAuction2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuction2ValueKey);
            UpAuction = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.UpAuctionValueKey);
            DownAuction = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuctionValueKey);
            DownAuction2 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownAuction2ValueKey);
            DownAuction3 = await AppSettingHelper.TryGetSettingAsync<double>(KeyNames.DownProfit3ValueKey);
        }

        private void InitAllColors()
        {
            AllColors = new List<string>()
            {
                "White",
                "Black",

                "Red",
                "Green",
                "Blue",

                "Yellow",
                "Teal",
                "Orange",
                "YellowGreen",
                "Violet",
                "Gray",
                "GreenYellow",
                "Pink",
                "Brown",
                "Purple",
                "Lime",
                "Cyan",

                "Coral",
                "HotPink",
                "BlueViolet",
                "BurlyWood",
                "CadetBlue",
                //"Chartresue",
                //"Chocolate",
                "CornflowerBlue",
                "Crimson",
                "DarkBlue",
                "DarkCyan",
                "DarkGoldenrod",
                "DarkGray",
                "DarkGreen",
                "DarkMagenta",
                "DarkOliveGreen",
                "DarkOrange",
                "DarkOrchid",
                "DarkRed",
                "DarkSalmon",
                "DarkSlateBlue",
                "DarkSlateGray",
                "DarkTurquoise",
                "DarkViolet",
                "DeepPink",
                "DeepSkyBlue",
                "DodgerBlue",
                "Firebrick",
                "ForestGreen",
                "Fuchsia",
                "Gold",
                "Goldenrod",
                "IndianRed",
                "Indigo",
                "LawnGreen",
                "LightCoral",
                "LightGray",
                "LightGreen",
                "LightPink",
                "LightSalmon",
                "LightSeaGreen",
                "LightSkyBlue",
                "LightSlateGray",
                //"LightSteelBlue",
                "LimeGreen",
                "Magenta",
                "Maroon",
                "MediumAquamarine",
                "MediumBlue",
                "MediumOrchid",
                "MediumPurple",
                "MediumSeaGreen",
                "MediumSlateBlue",
                "MediumSpringGreen",
                "MediumTurquoise",
                "MediumVioletRed",
                "MidnightBlue",
                "Navy",
                "Olive",
                "OliveDrab",
                "OrangeRed",
                "Orchid",
                "PaleVioletRed",
                "Peru",
                "RosyBrown",
                "RoyalBlue",
                "SaddleBrown",
                "Salmon",
                "SandyBrown",
                "SeaGreen",
                "Sienna",
                "SkyBlue",
                "SlateBlue",
                "SlateGray",
                "SpringGreen",
                "SteelBlue",
                "Tan",
                "Thistle",
                "Tomato",
                "Turquoise",
            };
        }

        private IList<string> GetAllSystemColors()
        {
            List<string> colors = new List<string>();
            var properties = typeof(Colors).GetProperties();
            foreach(var p in properties)
            {
                var name = p.Name.Replace("System.Windows.Media.Colors ", "");
                colors.Add(name);
            }
            return colors;
        }

        public List<string> AllColors { get; set; }

        public double upProfit3;
        public double UpProfit3
        {
            get { return upProfit3; }
            set { SetProperty(ref upProfit3, value); }
        }

        public string selectedUpProfit3Color;
        public string SelectedUpProfit3Color
        {
            get { return selectedUpProfit3Color; }
            set { SetProperty(ref selectedUpProfit3Color, value); }
        }

        public double upProfit2;
        public double UpProfit2
        {
            get { return upProfit2; }
            set { SetProperty(ref upProfit2, value); }
        }

        public string selectedUpProfit2Color;
        public string SelectedUpProfit2Color
        {
            get { return selectedUpProfit2Color; }
            set { SetProperty(ref selectedUpProfit2Color, value); }
        }

        public double upProfit;
        public double UpProfit
        {
            get { return upProfit; }
            set { SetProperty(ref upProfit, value); }
        }

        public string selectedUpProfitColor;
        public string SelectedUpProfitColor
        {
            get { return selectedUpProfitColor; }
            set { SetProperty(ref selectedUpProfitColor, value); }
        }

        public string selectedNormalProfitColor;
        public string SelectedNormalProfitColor
        {
            get { return selectedNormalProfitColor; }
            set { SetProperty(ref selectedNormalProfitColor, value); }
        }

        public double downProfit;
        public double DownProfit
        {
            get { return downProfit; }
            set { SetProperty(ref downProfit, value); }
        }

        public string selectedDownProfitColor;
        public string SelectedDownProfitColor
        {
            get { return selectedDownProfitColor; }
            set { SetProperty(ref selectedDownProfitColor, value); }
        }

        public double downProfit2;
        public double DownProfit2
        {
            get { return downProfit2; }
            set { SetProperty(ref downProfit2, value); }
        }

        public string selectedDownProfit2Color;
        public string SelectedDownProfit2Color
        {
            get { return selectedDownProfit2Color; }
            set { SetProperty(ref selectedDownProfit2Color, value); }
        }

        public double downProfit3;
        public double DownProfit3
        {
            get { return downProfit3; }
            set { SetProperty(ref downProfit3, value); }
        }

        public string selectedDownProfit3Color;
        public string SelectedDownProfit3Color
        {
            get { return selectedDownProfit3Color; }
            set { SetProperty(ref selectedDownProfit3Color, value); }
        }

        public double upAuction3;
        public double UpAuction3
        {
            get { return upAuction3; }
            set { SetProperty(ref upAuction3, value); }
        }

        public string selectedUpAuction3Color;
        public string SelectedUpAuction3Color
        {
            get { return selectedUpAuction3Color; }
            set { SetProperty(ref selectedUpAuction3Color, value); }
        }

        public double upAuction2;
        public double UpAuction2
        {
            get { return upAuction2; }
            set { SetProperty(ref upAuction2, value); }
        }

        public string selectedUpAuction2Color;
        public string SelectedUpAuction2Color
        {
            get { return selectedUpAuction2Color; }
            set { SetProperty(ref selectedUpAuction2Color, value); }
        }

        public double upAuction;
        public double UpAuction
        {
            get { return upAuction; }
            set { SetProperty(ref upAuction, value); }
        }

        public string selectedUpAuctionColor;
        public string SelectedUpAuctionColor
        {
            get { return selectedUpAuctionColor; }
            set { SetProperty(ref selectedUpAuctionColor, value); }
        }

        public string selectedNormalAuctionColor;
        public string SelectedNormalAuctionColor
        {
            get { return selectedNormalAuctionColor; }
            set { SetProperty(ref selectedNormalAuctionColor, value); }
        }

        public double downAuction;
        public double DownAuction
        {
            get { return downAuction; }
            set { SetProperty(ref downAuction, value); }
        }

        public string selectedDownAuctionColor;
        public string SelectedDownAuctionColor
        {
            get { return selectedDownAuctionColor; }
            set { SetProperty(ref selectedDownAuctionColor, value); }
        }

        public double downAuction2;
        public double DownAuction2
        {
            get { return downAuction2; }
            set { SetProperty(ref downAuction2, value); }
        }

        public string selectedDownAuction2Color;
        public string SelectedDownAuction2Color
        {
            get { return selectedDownAuction2Color; }
            set { SetProperty(ref selectedDownAuction2Color, value); }
        }

        public double downAuction3;
        public double DownAuction3
        {
            get { return downAuction3; }
            set { SetProperty(ref downAuction3, value); }
        }

        public string selectedDownAuction3Color;
        public string SelectedDownAuction3Color
        {
            get { return selectedDownAuction3Color; }
            set { SetProperty(ref selectedDownAuction3Color, value); }
        }
    }
}

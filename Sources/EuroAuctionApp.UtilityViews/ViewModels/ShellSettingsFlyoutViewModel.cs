using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.Infra.Base;
using EuroAuctionApp.Infra.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Prism.Commands;
using System.Windows;

namespace EuroAuctionApp.UtilityViews.ViewModels
{
    public class ShellSettingsFlyoutViewModel:ViewModelBase
    {
        private ILocalizerService localizer;

        public ShellSettingsFlyoutViewModel()
        {
            localizer = Resolve<ILocalizerService>();

            this.AccentColors = ThemeManager.Accents.Select(o => o.Name).ToList();
            this.SupportedLanguages = localizer.SupportedLanguages.Select(o => o.Name).ToList();

            LoadDefaultSettings();

            SelectedAccentColorChangedCommand = new DelegateCommand(DoChangeAccentColor);
            SelectedLanguageChangedCommand = new DelegateCommand(DoChangeLanguage);

            DoChangeAccentColor();
            DoChangeLanguage();
        }

        public IList<string> AccentColors { get; set; }
        public IList<string> SupportedLanguages { get; set; }

        private string selectedAccentColor;
        public string SelectedAccentColor
        {
            get { return selectedAccentColor; }
            set { SetProperty(ref selectedAccentColor, value); }
        }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set { SetProperty(ref selectedLanguage, value); }
        }

        public DelegateCommand SelectedAccentColorChangedCommand { get; private set; }
        public DelegateCommand SelectedLanguageChangedCommand { get; private set; }

        private void LoadDefaultSettings()
        {
            if (string.IsNullOrEmpty(SelectedAccentColor))
            {
                SelectedAccentColor = "Cyan";
            }

            if (string.IsNullOrEmpty(SelectedLanguage))
            {
                SelectedLanguage = "zh-CN";
            }
        }

        private void DoChangeAccentColor()
        {
            try
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(SelectedAccentColor);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            }
            catch (Exception ex)
            {
                LogError("DoChangeAccentColor() : " + ex.Message);
            }
        }

        private void DoChangeLanguage()
        {
            try
            {
                localizer.SetLocale(SelectedLanguage);
            }
            catch (Exception ex)
            {
                LogError("DoChangeLanguage() : " + ex.Message);
            }
        }
    }
}

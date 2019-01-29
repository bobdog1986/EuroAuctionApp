
// Helpers/Settings.cs This file was automatically added when you installed the Settings Plugin. If you are not using a PCL then comment this file back in to use it.
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace EuroAuctionApp.Infra.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly string DefaultLanguage = "zh-CN";
        private static readonly string DefaultAccent = "Cyan";

        #endregion


        public static string BackupPath
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(BackupPath), SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(BackupPath), value);
			}
		}

        public static string QuoteFolderImportPath
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(QuoteFolderImportPath), SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(QuoteFolderImportPath), value);
            }
        }

        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Language), DefaultLanguage);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Language), value);
            }
        }

        public static string Accent
        {
            get
            {
                return AppSettings.GetValueOrDefault(nameof(Accent), DefaultAccent);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Accent), value);
            }
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Globalization;
using System.IO;
using EuroAuctionApp.CoreViews.Helpers;

namespace EuroAuctionApp.CoreViews.Models
{
    public class QuoteFileModel:BindableBase
    {
        public QuoteFileModel(string filename)
        {
            FileName = filename;
            GetFileShortName();
            DateTime=ParseDateTime(filename);
            ParseFirstSymbol();
        }

        private string firstSymbol;
        public string FirstSymbol
        {
            get { return firstSymbol; }
            set { SetProperty(ref firstSymbol, value); }
        }

        private string firstMarket;
        public string FirstMarket
        {
            get { return firstMarket; }
            set { SetProperty(ref firstMarket, value); }
        }

        private string filename;
        public string FileName
        {
            get { return filename; }
            set { SetProperty(ref filename, value); }
        }

        private string shortName;
        public string ShortName
        {
            get { return shortName; }
            set { SetProperty(ref shortName, value); }
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { SetProperty(ref dateTime, value); }
        }

        private bool is25File;
        public bool Is25File
        {
            get { return is25File; }
            set { SetProperty(ref is25File, value); }
        }

        private bool isCloseFile;
        public bool IsCloseFile
        {
            get { return isCloseFile; }
            set { SetProperty(ref isCloseFile, value); }
        }

        private bool isAuctionFile;
        public bool IsAuctionFile
        {
            get { return isAuctionFile; }
            set { SetProperty(ref isAuctionFile, value); }
        }

        private void GetFileShortName()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                ShortName = Path.GetFileName(FileName);
            }
        }

        public static DateTime ParseDateTime(string filename)
        {
            string shortName = Path.GetFileName(filename);
            if (!string.IsNullOrEmpty(shortName))
            {
                string timeString = shortName.Substring(FormatterHelper.QuotePrefix.Length, FormatterHelper.QuoteTimeFormat.Length);
                return DateTime.ParseExact(timeString, FormatterHelper.QuoteTimeFormat, CultureInfo.InvariantCulture);
            }
            else
            {
                return DateTime.Now;
            }
        }

        private void ParseFirstSymbol()
        {
            var lines = File.ReadAllLines(FileName);
            if (lines != null && lines.Length > 1)
            {
                var cells= lines[1].Split(',');
                if (cells != null && cells.Length > 0)
                {
                    FirstSymbol = cells[0];
                    FirstMarket = FirstSymbol.Split('.')[1];
                }
            }
        }
    }
}

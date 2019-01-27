﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroAuctionApp.DAL.Interfaces;
using EuroAuctionApp.DAL.Models;
using System.IO;
using CsvHelper;

namespace EuroAuctionApp.DAL.Services
{
    public class CsvService:ICsvService
    {
        public IList<StockLineData> ReadAllLines(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(filename);
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            TrimInvalidCells(filename);

            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<StockLineData>();
                return records.ToList();
            }
        }

        void TrimInvalidCells(string filename)
        {
            StringBuilder sb = new StringBuilder();
            var lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Count(); i++)
            {
                if (i == 0)
                {
                    sb.AppendLine(lines[0]);
                }
                else
                {
                    var cells = lines[i].Split(',');
                    var correctCells = cells.Select(o => o == "-" ? string.Empty : o);

                    sb.AppendLine(string.Join(",", correctCells));
                }
            }

            File.WriteAllText(filename, sb.ToString(), Encoding.UTF8);
        }
    }
}
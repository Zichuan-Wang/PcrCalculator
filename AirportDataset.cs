﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcrCalculator
{
    public struct Airport
    {
        public string CountryName;
        public string Code;
        public string FriendlyName;
        public int PCRInAdvance;
        public DateTime StartTime;
        public TimeZoneInfo TimeZone;
    }
    public static class AirportDataset
    {
        public static List<Airport> Airports = new List<Airport>();
        public static string[] AirportNames;

        public static readonly DateTime DefaultTime = new DateTime(2020, 1, 1, 0, 0, 1);
        static AirportDataset()
        {
        }

        public static void LoadData()
        {
            foreach (var it in PcrData.PCR.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var content = it.Split(',');
                foreach (var it2 in content[1].Split('/'))
                {
                    Airports.Add(new Airport()
                    {
                        CountryName = content[0],
                        Code = it2,
                        FriendlyName = content[2],
                        TimeZone = TimeZoneInfo.FindSystemTimeZoneById(content[3]),
                        PCRInAdvance = int.TryParse(content[4], out var advance) ? advance : -1,
                        StartTime = content[5].Contains('/') ? GetStartTime(content[5]) : DefaultTime
                    });
                }
            }

            Airports = Airports.OrderBy(a => a.Code).ToList();

            AirportNames = new string[Airports.Count];
            for (var i = 0; i < Airports.Count; i++)
            {
                AirportNames[i] = $"{Airports[i].Code} {Airports[i].CountryName} {Airports[i].FriendlyName}";
            }
        }

        private static DateTime GetStartTime(string str)
        {
            var month = int.Parse(str.Substring(0, str.IndexOf('/')));
            var day = int.Parse(str.Substring(str.IndexOf('/') + 1));
            return new DateTime(2020, month, day, 0, 0, 1);
        }

        public static bool TryGetAirport(string name, out Airport info)
        {
            for (var i = 0; i < Airports.Count; i++)
                if (Airports[i].Code == name)
                {
                    info = Airports[i];
                    return true;
                }

            info = default;
            return false;
        }
    }
   
}

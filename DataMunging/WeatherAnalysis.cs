using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataMunging.Models;

namespace DataMunging
{
    public class WeatherAnalysis
    {
        public static (bool IsValid, string Message) Calculate(DataMungingDetails dataMungingDetails)
        {
            try
            {
                if (!File.Exists(dataMungingDetails.FileName))
                    return (false, "File does not exist");

                var includeHeaders = dataMungingDetails.IsHeaderIncluded;

                //Read all the data in the file in one go
                var lines = File.ReadAllLines(dataMungingDetails.FileName).ToList();

                if (lines.Count > 0)
                {
                    //Storing the last line of the data and removing it from original days data
                    var monthAvg = lines[lines.Count - 1];
                    lines.RemoveAt(lines.Count - 1);

                    var temperatures = PopulateData(lines, includeHeaders);

                    var dayWithMinTempDiff = temperatures.OrderBy(m => m.MinDiff).FirstOrDefault();

                    return (true, $"Day with smallest temperature spread: {dayWithMinTempDiff.Day}");
                }
                else
                    return (false, "File does not contain any record");
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.ToString()}");
            }
        }

        private static List<Temperatures> PopulateData(List<string> lines, bool includeHeaders)
        {
            var records = new List<Temperatures>();

            foreach (var line in lines)
            {
                if (includeHeaders)
                {
                    //No processing is required for the headers
                    includeHeaders = false;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        //If 8 or more spaces are present, then it represents that a column contains NULL there
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{8,}", options);
                        //Placing an indicator to represent NULL value for a column
                        var lineContents = regex.Replace(line, " null ");
                        //Splitting the line on the basis of space but picking only non-empty strings
                        var contents = lineContents.Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();

                        if (contents.Count < 17)
                        {
                            //Invalid record
                        }
                        else
                        {
                            //Add record to the list of valid records
                            records.Add(new Temperatures
                            {
                                Day = contents.ElementAtOrDefault(0) != null ? int.Parse(contents[0]) : 0,
                                MxT = contents.ElementAtOrDefault(1) != null ? int.Parse(contents[1].Replace("*", "")) : 0,
                                MnT = contents.ElementAtOrDefault(2) != null ? int.Parse(contents[2].Replace("*", "")) : 0,
                                AvT = contents.ElementAtOrDefault(3) != null ? int.Parse(contents[3]) : 0,
                                HDDay = contents.ElementAtOrDefault(4) != null && contents.ElementAtOrDefault(4) != "null" ? (int?)int.Parse(contents[4]) : null,
                                AvDP = contents.ElementAtOrDefault(5) != null ? double.Parse(contents[5]) : 0,
                                HrP1 = contents.ElementAtOrDefault(6) != null && contents.ElementAtOrDefault(6) != "null" ? (double?)double.Parse(contents[6]) : null,
                                TPcpn = contents.ElementAtOrDefault(7) != null ? double.Parse(contents[7]) : 0,
                                WxType = contents.ElementAtOrDefault(8) != null && contents.ElementAtOrDefault(8) != "null" ? contents[8] : null,
                                PDir = contents.ElementAtOrDefault(9) != null ? contents[9] : "",
                                AvSp = contents.ElementAtOrDefault(10) != null ? double.Parse(contents[10]) : 0,
                                Dir = contents.ElementAtOrDefault(11) != null ? contents[11] : "",
                                MxS = contents.ElementAtOrDefault(12) != null ? int.Parse(contents[12].Replace("*", "")) : 0,
                                SkyC = contents.ElementAtOrDefault(13) != null ? double.Parse(contents[13]) : 0,
                                MxR = contents.ElementAtOrDefault(14) != null ? int.Parse(contents[14]) : 0,
                                MnR = contents.ElementAtOrDefault(15) != null ? int.Parse(contents[15]) : 0,
                                AvSLP = contents.ElementAtOrDefault(16) != null ? double.Parse(contents[16]) : 0,
                                MinDiff = Math.Abs((contents.ElementAtOrDefault(1) != null ? int.Parse(contents[1].Replace("*", "")) : 0) -
                                          (contents.ElementAtOrDefault(2) != null ? int.Parse(contents[2].Replace("*", "")) : 0))
                            });
                        }
                    }
                }
            }

            return records;
        }
    }
}
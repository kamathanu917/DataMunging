using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataMunging.Models;

namespace DataMunging
{
    public class SoccerLegueTable
    {
        public static (bool IsValid, string Message) Calculate(DataMungingDetails dataMungingDetails)
        {
            try
            {
                if (!File.Exists(dataMungingDetails.FileName))
                    return (false, "File does not exist");

                var includeHeaders = dataMungingDetails.IsHeaderIncluded;

                //Read all the data in the file in one go
                var lines = File.ReadAllLines(dataMungingDetails.FileName);

                if (lines.Length > 0)
                {
                    var scores = PopulateData(lines, includeHeaders);

                    var teamWithMinGoalDiff = scores.OrderBy(m => m.MinDiff).FirstOrDefault();

                    return (true, $"Team with minimum goal difference: {teamWithMinGoalDiff.TeamName}");
                }
                else
                    return (false, "File does not contain any record");
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.ToString()}");
            }
        }

        private static List<Scorecard> PopulateData(string[] lines, bool includeHeaders)
        {
            var records = new List<Scorecard>();

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
                        //Splitting the line on the basis of space but picking only non-empty strings
                        var contents = line.Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                        if (contents.Distinct().Count() == 1)
                        {
                            //No processing is required for the line containing only dashes
                        }
                        else if (contents.Count < 10)
                        {
                            //Invalid record
                        }
                        else
                        {
                            //Add record to the list of valid records
                            records.Add(new Scorecard
                            {
                                TeamName = contents.ElementAtOrDefault(1) != null ? contents[1] : "",
                                P = contents.ElementAtOrDefault(2) != null ? int.Parse(contents[2]) : 0,
                                W = contents.ElementAtOrDefault(3) != null ? int.Parse(contents[3]) : 0,
                                L = contents.ElementAtOrDefault(4) != null ? int.Parse(contents[4]) : 0,
                                D = contents.ElementAtOrDefault(5) != null ? int.Parse(contents[5]) : 0,
                                F = contents.ElementAtOrDefault(6) != null ? int.Parse(contents[6]) : 0,
                                A = contents.ElementAtOrDefault(8) != null ? int.Parse(contents[8]) : 0,
                                Pts = contents.ElementAtOrDefault(9) != null ? int.Parse(contents[9]) : 0,
                                MinDiff = Math.Abs((contents.ElementAtOrDefault(6) != null ? int.Parse(contents[6]) : 0) -
                                          (contents.ElementAtOrDefault(8) != null ? int.Parse(contents[8]) : 0))
                            });
                        }
                    }
                }
            }

            return records;
        }
    }
}
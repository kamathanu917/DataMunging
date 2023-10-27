using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using DataMunging.Models;

namespace DataMunging
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);
                var config = builder.Build();

                var dataMungingName = config.GetSection("ProgramToExecute").Value;

                var details = new DataMungingDetails
                {
                    FileName = config.GetSection(dataMungingName).GetSection("FileName").Value,
                    IsHeaderIncluded = config.GetSection(dataMungingName).GetSection("IncludeHeader").Value.Equals("true")
                };

                var result = dataMungingName.Equals(Constants.WEATHER_ANALYSIS) ? WeatherAnalysis.Calculate(details) : SoccerLegueTable.Calculate(details);

                Console.WriteLine(result.Message);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.ToString()}");
            }
        }
    }
}
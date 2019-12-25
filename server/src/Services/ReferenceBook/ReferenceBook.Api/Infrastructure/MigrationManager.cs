using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Collections.Extensions;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReferenceBook.Domain.AggregatesModel.CountryAggregate;
using ReferenceBook.Domain.AggregatesModel.LanguageAggregate;
using ReferenceBook.Infrastructure;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ReferenceBook.Api.Infrastructure
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using (var context = scope.ServiceProvider.GetRequiredService<ReferenceBookContext>())
            {
                var env = scope.ServiceProvider.GetService<IHostEnvironment>();

                try
                {
                    context.Database.Migrate();

                    if (!context.Countries.Any())
                        context.Countries.AddRange(GetCountriesFromFile(env.ContentRootPath));

                    if (!context.Languages.Any())
                        context.Languages.AddRange(GetLanguagesFromFile(env.ContentRootPath));

                    context.SaveChangesAsync().Wait();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return host;
        }
        private static IEnumerable<Country> GetCountriesFromFile(string contentRootPath)
        {
            var csvFileCountries = Path.Combine(contentRootPath, "Setup", "Countries.csv");
            if (!File.Exists(csvFileCountries)) throw new Exception("File is not exists");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "Name", "Alpha2Code", "Alpha3Code", "NumericCode", "Region", "SubRegion" };
                csvheaders = GetHeaders(requiredHeaders, csvFileCountries);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return File.ReadAllLines(csvFileCountries)
                       .Skip(1)
                       .SelectTry(CreateCountry)
                       .OnCaughtException(ex =>
                        {
                            Log.Fatal(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                            return null!;
                        })
                        .OfType<Country>();
        }

        private static Country CreateCountry(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Country is null or empty");

            var list = value.Split(",").ToArray();
            return new Country(list[0].ToString(),
                               list[1].ToEnum<Alpha2Code>(),
                               list[2].ToEnum<Alpha3Code>(),
                               Convert.ToInt16(list[3]),
                               list[4].ToEnum<Region>(),
                               list[5]);
        }

        private static IEnumerable<Language> GetLanguagesFromFile(string contentRootPath)
        {
            var csvFileLanguages = Path.Combine(contentRootPath, "Setup", "Languages.csv");
            if (!File.Exists(csvFileLanguages)) throw new Exception("File is not exists");

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "Name", "NativeName", "Code" };
                csvheaders = GetHeaders(requiredHeaders, csvFileLanguages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return File.ReadAllLines(csvFileLanguages)
                       .Skip(1)
                       .SelectTry(CreateLanguage)
                       .OnCaughtException(ex =>
                        {
                            Log.Fatal(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                            return null!;
                        })
                        .OfType<Language>();
        }

        private static Language CreateLanguage(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new Exception("Language is null or empty");

            var list = value.Split(",").ToArray();
            return new Language(list[0].ToString(),list[1].ToString(), list[2].ToEnum<LanguageCode>());
        }

        private static string[] GetHeaders(string[] requiredHeaders, string csvFile)
        {
            var csvHeaders = File.ReadLines(csvFile).First().Split(',');

            if (csvHeaders.Count() != requiredHeaders.Count())
                throw new Exception($"requiredHeader count '{requiredHeaders.Count()}' is different then read header '{csvHeaders.Count()}'");

            foreach (var requiredHeader in requiredHeaders)
                if (!csvHeaders.Contains(requiredHeader))
                    throw new Exception($"does not contain required header '{requiredHeader}'");

            return csvHeaders;
        }
    }
}
using Biblioteca.Services.Reports;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;

namespace Biblioteca.Services
{
    public class DataService
    {
        public const string DataPath = @"Data";
        public const string FlagPath = $@"{DataPath}\Flags";
        public const string COAPath = $@"{DataPath}\COAs";

        public static Task<Response> SaveDataAsync(List<Country> countries, CancellationToken ct, IProgress<CountrySaveProgressReport> progress)
        {
            return new DataService(countries, progress).SaveDataAsync(ct);
        }

        public static async Task<Response> LoadDataAsync(CancellationToken ct, IProgress<CountryLoadProgressReport> progress)
        {
            var response = new Response()
            {
                Success = false,
                Message = "Something wrong happened when loading the data"
            };

            // Verify if the DB exists
            string dbPath = @$"{DataPath}\Countries.sqlite";
            if (!File.Exists(dbPath))
            {
                response.Message = "No data to be loaded...";
                return response;
            }

            var connection = new SQLiteConnection(@$"Data Source={dbPath}");
            try
            {
                string sqlcommand;
                SQLiteCommand command;

                connection.Open();
                sqlcommand = """
                    SELECT COUNT(*)
                    FROM Countries
                    """;
                command = new SQLiteCommand(sqlcommand, connection);
                int totalCountries = Convert.ToInt32(command.ExecuteScalar());
                var report = new CountryLoadProgressReport
                {
                };

                sqlcommand = """
                    SELECT CountryJson
                    FROM Countries
                    """;
                command = new SQLiteCommand(sqlcommand, connection);
                var reader = await command.ExecuteReaderAsync(ct);

                List<Country> results = [];

                while (reader.Read())
                {
                    ct.ThrowIfCancellationRequested();
                    string countryJson = reader.GetString(0);
                    Country? country = JsonConvert.DeserializeObject<Country>(countryJson);
                    if (country != null)
                        results.Add(country);

                    progress.Report(new CountryLoadProgressReport
                    {
                        TotalCountries = totalCountries,
                        LoadedCountries = results.Count
                    });
                }

                response.Success = true;
                response.Message = results;
            }
            catch (Exception ex)
            {
                response.Message += $"\r\n{ex}";
            }
            finally
            {
                connection.Close();
            }
            return response;
        }

        /// <summary>
        /// Converts a number of bytes into KB/MB/GB/TB if needed
        /// </summary>
        public static string SimplifyBytes(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} bytes";

            string[] endings = ["KB", "MB", "GB", "TB"];
            double data = bytes / 1024;

            int iter = 0;
            while (data > 1024)
            {
                data /= 1024;
                iter++;
            }

            return Math.Round(data, 2) + endings[iter];
        }

        private List<Country> _countries;
        private List<string> _countriesJson;
        private Dictionary<string, byte[]> _images; // Not used in method 1 of saving the images
        private SQLiteConnection _connection;
        private IProgress<CountrySaveProgressReport> _progress;

        private int _savedCountries = 0;
        private long _totalBytes = 0;
        private long _savedBytes = 0;

        private DataService(List<Country> countries, IProgress<CountrySaveProgressReport> progress)
        {
            _countries = countries;
            _countriesJson = new(countries.Count);
            _images = new(countries.Count * 4); // Not all countries have the 4 images, but consider it anyway
            // Keep the original order 
            countries.ForEach(country => _countriesJson.Add(JsonConvert.SerializeObject(country)));

            _progress = progress;
            _connection = new SQLiteConnection(@$"Data Source={DataPath}\Countries.sqlite");
        }

        /// <summary>
        /// Saves the list 'countries' into the DB 'Data/Countries.sqlite'
        /// </summary>
        /// <param name="countries"></param>
        /// <param name="ct"></param>
        public async Task<Response> SaveDataAsync(CancellationToken ct)
        {
            Response response = new()
            {
                Success = false,
                Message = "Something wrong happened when saving the data"
            };

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

                _progress.Report(new CountrySaveProgressReport
                {
                    TotalCountries = _countriesJson.Count,
                    SavedCountries = _savedCountries,
                    TotalBytes = -1,
                    SavedBytes = -1,
                });
                await CalculateDataSizeAsync(ct);
                _progress.Report(new CountrySaveProgressReport
                {
                    TotalCountries = _countriesJson.Count,
                    SavedCountries = _savedCountries,
                    TotalBytes = _totalBytes,
                    SavedBytes = _savedBytes,
                });

                CreateFolders();

                _connection.Open();
                CreateTable();

                await Parallel.ForEachAsync(_countries.Select((x, i) => (i + 1, x, _countriesJson[i])), ct,
                    WriteCountry);

                response.Success = true;
                response.Message = $"Saved data at {DateTime.Now}";
            }
            catch (TaskCanceledException)
            {
                // Remove the data folder
                if (Directory.Exists(DataPath))
                    Directory.Delete(DataPath, true);
                response.Message = "Save cancelled";
            }
            catch (Exception ex)
            {
                // Remove the data folder
                if (Directory.Exists(DataPath))
                    Directory.Delete(DataPath, true);
                response.Message += $"\r\n{ex}";
            }
            finally
            {
                _connection.Close();
            }
            return response;
        }

        /// <summary>
        /// Pre-downloads the images and calculates the number of bytes stored
        /// </summary>
        /// <param name="ct"></param>
        /// <exception cref="Exception">Whenever there is no space available in the drive</exception>
        private async Task CalculateDataSizeAsync(CancellationToken ct)
        {
            HttpClient client = new();

            /*
             * Method 1: download the images while saving the data into the DB
             */
            //await Parallel.ForEachAsync(_countries.Select((x, i) => (Index: i + 1, Country: x)), ct,
            //    async (values, ct) =>
            //    {
            //        Interlocked.Add(ref _totalBytes, _countriesJson[values.Index - 1].Length);

            //        if (values.Country.Flags.PNG != "n/a")
            //        {
            //            var flagPng = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, values.Country.Flags.PNG), ct);
            //            Interlocked.Add(ref _totalBytes, flagPng.Content.Headers.ContentLength ?? 0);
            //        }
            //        if (values.Country.Flags.SVG != "n/a")
            //        {
            //            var flagSvg = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, values.Country.Flags.SVG), ct);
            //            Interlocked.Add(ref _totalBytes, flagSvg.Content.Headers.ContentLength ?? 0);
            //        }
            //        if (values.Country.CoatOfArms.PNG != "n/a")
            //        {
            //            var coaPng = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, values.Country.CoatOfArms.PNG), ct);
            //            Interlocked.Add(ref _totalBytes, coaPng.Content.Headers.ContentLength ?? 0);
            //        }
            //        if (values.Country.CoatOfArms.SVG != "n/a")
            //        {
            //            var coaSvg = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, values.Country.CoatOfArms.SVG), ct);
            //            Interlocked.Add(ref _totalBytes, coaSvg.Content.Headers.ContentLength ?? 0);
            //        }
            //    });

            /*
             * Method 2: download the images before saving the data into the DB
             */
            await Parallel.ForEachAsync(_countries.Select((x, i) => (Index: i + 1, Country: x, CountryJson: _countriesJson[i])), ct,
                async (values, ct) =>
                {
                    Interlocked.Add(ref _totalBytes, values.CountryJson.Length);

                    byte[] flagPng, flagSvg, coaPng, coaSvg;
                    if (values.Country.Flags.PNG != "n/a")
                    {
                        flagPng = await client.GetByteArrayAsync(values.Country.Flags.PNG, ct);
                        _images.Add($"flag{values.Index:000}.png", flagPng);
                        Interlocked.Add(ref _totalBytes, flagPng.LongLength);
                    }
                    if (values.Country.Flags.SVG != "n/a")
                    {
                        flagSvg = await client.GetByteArrayAsync(values.Country.Flags.SVG, ct);
                        _images.Add($"flag{values.Index:000}.svg", flagSvg);
                        Interlocked.Add(ref _totalBytes, flagSvg.LongLength);
                    }
                    if (values.Country.CoatOfArms.PNG != "n/a")
                    {
                        coaPng = await client.GetByteArrayAsync(values.Country.CoatOfArms.PNG, ct);
                        _images.Add($"coa{values.Index:000}.png", coaPng);
                        Interlocked.Add(ref _totalBytes, coaPng.LongLength);
                    }
                    if (values.Country.CoatOfArms.SVG != "n/a")
                    {
                        coaSvg = await client.GetByteArrayAsync(values.Country.CoatOfArms.SVG, ct);
                        _images.Add($"coa{values.Index:000}.svg", coaSvg);
                        Interlocked.Add(ref _totalBytes, coaSvg.LongLength);
                    }
                });

            client.Dispose();

            // Verify if there is enough space in the drive
            var drive = DriveInfo.GetDrives()
                .First(drive => drive.Name == Directory.GetDirectoryRoot(DataPath));
            long freeSpace = drive.AvailableFreeSpace;
            // If there are already images stored subtract their size, they will be overwritten
            if (Directory.Exists(DataPath))
                freeSpace -= new DirectoryInfo(DataPath)
                .EnumerateDirectories().Sum(dir => dir.EnumerateFiles().Sum(file => file.Length));

            // Play safe and check 1.2x the number of bytes to save
            if (_totalBytes * 1.2 >= freeSpace)
                throw new Exception("Not enough space on disk");

        }

        /// <summary>
        /// Create the necessary folders to store the data
        /// </summary>
        private void CreateFolders()
        {
            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);

            if (Directory.Exists(FlagPath))
                Directory.Delete(FlagPath, true);
            Directory.CreateDirectory(FlagPath);

            if (Directory.Exists(COAPath))
                Directory.Delete(COAPath, true);
            Directory.CreateDirectory(COAPath);
        }

        /// <summary>
        /// Create the table in the DB
        /// </summary>
        private void CreateTable()
        {
            string sqlcommand;
            SQLiteCommand command;

            sqlcommand = """
                CREATE TABLE IF NOT EXISTS Countries (
                    CountryId       INTEGER PRIMARY KEY,
                    CountryJson     TEXT
                );
                DELETE FROM Countries;
                """;

            command = new SQLiteCommand(sqlcommand, _connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Save the country's JSON into the DB and write the images in their respective folders
        /// </summary>
        /// <param name="values"></param>
        /// <param name="ct"></param>
        /// <exception cref="DbException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        private async ValueTask WriteCountry((int Index, Country Country, string CountryJson) values, CancellationToken ct)
        {
            if (JsonConvert.SerializeObject(values.Country) != values.CountryJson)
                throw new Exception($"Country json {values.CountryJson.Substring(5, 20)} != Country {values.Country.Name.Common}");
            string sqlcommand = $"""
                INSERT INTO Countries
                (CountryId, CountryJson)
                VALUES
                ({values.Index}, '{values.CountryJson.Replace("'", "''")}')
                """;

            var command = new SQLiteCommand(sqlcommand, _connection);
            await command.ExecuteNonQueryAsync(ct);
            Interlocked.Add(ref _savedBytes, values.CountryJson.Length);

            await WriteImages(values.Index, values.Country, ct);

            Interlocked.Increment(ref _savedCountries);
            _progress.Report(new CountrySaveProgressReport
            {
                TotalCountries = _countriesJson.Count,
                SavedCountries = _savedCountries,
                TotalBytes = _totalBytes,
                SavedBytes = _savedBytes,
            });
        }

        /// <summary>
        /// Write the images in their respective folders
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="country"></param>
        /// <param name="ct"></param>
        private async Task WriteImages(int countryId, Country country, CancellationToken ct)
        {
            /*
             * Method 1: download the images while saving the data into the DB
             */
            //var client = new HttpClient();
            //if (country.Flags.PNG != "n/a")
            //{
            //    var flagPng = await client.GetByteArrayAsync(country.Flags.PNG, ct);
            //    await File.WriteAllBytesAsync(@$"{FlagPath}\flag{countryId:000}.png", flagPng, ct);
            //    Interlocked.Add(ref _savedBytes, flagPng.LongLength);
            //}
            //if (country.Flags.SVG != "n/a")
            //{
            //    var flagSvg = await client.GetByteArrayAsync(country.Flags.SVG, ct);
            //    await File.WriteAllBytesAsync(@$"{FlagPath}\flag{countryId:000}.svg", flagSvg, ct);
            //    Interlocked.Add(ref _savedBytes, flagSvg.LongLength);
            //}
            //if (country.CoatOfArms.PNG != "n/a")
            //{
            //    var coaPng = await client.GetByteArrayAsync(country.CoatOfArms.PNG, ct);
            //    await File.WriteAllBytesAsync(@$"{COAPath}\coa{countryId:000}.png", coaPng, ct);
            //    Interlocked.Add(ref _savedBytes, coaPng.LongLength);
            //}
            //if (country.CoatOfArms.SVG != "n/a")
            //{
            //    var coaSvg = await client.GetByteArrayAsync(country.CoatOfArms.SVG, ct);
            //    await File.WriteAllBytesAsync(@$"{COAPath}\coa{countryId:000}.svg", coaSvg, ct);
            //    Interlocked.Add(ref _savedBytes, coaSvg.LongLength);
            //}
            //client.Dispose();

            /*
             * Method 2: download the images before saving the data into the DB
             */
            var imageKey = $"flag{countryId:000}.png";
            if (_images.ContainsKey(imageKey))
            {
                var flagPng = _images[imageKey];
                await File.WriteAllBytesAsync(@$"{FlagPath}\{imageKey}",
                    flagPng, ct);
                Interlocked.Add(ref _savedBytes, flagPng.LongLength);
            }
            imageKey = $"flag{countryId:000}.svg";
            if (_images.ContainsKey(imageKey))
            {
                var flagSvg = _images[imageKey];
                await File.WriteAllBytesAsync(@$"{FlagPath}\{imageKey}",
                    flagSvg, ct);
                Interlocked.Add(ref _savedBytes, flagSvg.LongLength);
            }
            imageKey = $"coa{countryId:000}.png";
            if (_images.ContainsKey(imageKey))
            {
                var coaPng = _images[imageKey];
                await File.WriteAllBytesAsync(@$"{COAPath}\{imageKey}",
                    coaPng, ct);
                Interlocked.Add(ref _savedBytes, coaPng.LongLength);
            }
            imageKey = $"coa{countryId:000}.svg";
            if (_images.ContainsKey(imageKey))
            {
                var coaSvg = _images[imageKey];
                await File.WriteAllBytesAsync(@$"{COAPath}\{imageKey}",
                    _images[$"coa{countryId:000}.svg"], ct);
                Interlocked.Add(ref _savedBytes, coaSvg.LongLength);
            }
        }
    }
}
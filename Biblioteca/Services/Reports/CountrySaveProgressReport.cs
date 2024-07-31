namespace Biblioteca.Services.Reports
{
    public class CountrySaveProgressReport : ProgressReportModel
    {
        public int TotalCountries { get; set; } = 0;
        public int SavedCountries { get; set; } = 0;
        public long TotalBytes { get; set; } = 0;
        public long SavedBytes { get; set; } = 0;

        public double PercentageCompleted()
        {
            return TotalCountries != 0 ? (double)SavedCountries / TotalCountries * 100 : 0;
        }
    }
}
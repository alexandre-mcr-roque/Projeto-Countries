namespace Biblioteca.Services.Reports
{
    public class CountryLoadProgressReport : ProgressReportModel
    {
        public int TotalCountries { get; set; } = 0;
        public int LoadedCountries { get; set; } = 0;

        public double PercentageCompleted()
        {
            return TotalCountries != 0 ? (double)LoadedCountries / TotalCountries * 100 : 0;
        }
    }
}

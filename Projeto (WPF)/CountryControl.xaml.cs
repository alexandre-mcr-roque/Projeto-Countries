using Projeto.Services;
using Projeto.Models;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.IO;
using Biblioteca.Services;

namespace Projeto
{
    /// <summary>
    /// Interaction logic for CountryControl.xaml
    /// </summary>
    public partial class CountryControl : UserControl
    {
        private CountryModel _countryModel;

        public CountryModel CountryModel => _countryModel;

        public CountryControl(CountryModel model)
        {
            _countryModel = model;
            InitializeComponent();
            LoadCountryDetails();
        }

        private void LoadCountryDetails()
        {
            _= LoadFlagAsync();
            _= LoadCOAAsync();
            _= LoadGoogleMapsAsync();
            _= LoadOpenStreetMapsAsync();

            OfficialName.Text = $"Official name: {_countryModel.Country.Name.Official}";
            AlternativeSpellings.Click += UnimplementedEvent;
            NativeNames.Click += UnimplementedEvent;

            if (_countryModel.Country.Independent != null)
                Independent.Text = $"Independent? {_countryModel.Country.Independent}";

            Status.Text = $"Status: {_countryModel.Country.Status}";
            Translations.Click += UnimplementedEvent;
            CCA2.Text = $"CCA2: {_countryModel.Country.CCA2}";
            CCA3.Text = $"CCA3: {_countryModel.Country.CCA3}";
            CCN3.Text = $"CCN3: {_countryModel.Country.CCN3}";

            if (_countryModel.Country.LatLng[0] != 0 || _countryModel.Country.LatLng[1] != 0)
            {
                Latitude.Text = $"Latitude: {Math.Round(_countryModel.Country.LatLng[0], 3)}";
                Longitude.Text = $"Longitude: {Math.Round(_countryModel.Country.LatLng[1], 3)}";
            }

            if (_countryModel.Country.CapitalInfo.LatLng[0] != 0 || _countryModel.Country.CapitalInfo.LatLng[1] != 0)
            {
                CapitalLat.Text = $"Capital Lat: {Math.Round(_countryModel.Country.CapitalInfo.LatLng[0], 3)}";
                CapitalLng.Text = $"Capital Lng: {Math.Round(_countryModel.Country.CapitalInfo.LatLng[1], 3)}";
            }

            if (_countryModel.Country.Area != -1)
                Area.Text = $"Area: {_countryModel.Country.Area} km²";

            if (_countryModel.Country.Landlocked != null)
                Landlocked.Text = $"Landlocked? {_countryModel.Country.Landlocked}";

            if (_countryModel.Country.Population != -1)
                Population.Text = $"Population: {_countryModel.Country.Population}";

            if (_countryModel.Country.Gini.Count > 0)
            {
                var gini = _countryModel.Country.Gini.First();
                Gini.Text = $"Gini: {gini.Value} ({gini.Key})";
            }

            StartOfWeek.Text = $"Start of Week: {_countryModel.Country.StartOfWeek}";
            Borders.Click += UnimplementedEvent;
            Languages.Click += UnimplementedEvent;
            Demonyms.Click += UnimplementedEvent;
            CarDetails.Click += UnimplementedEvent;
            Timezones.Click += UnimplementedEvent;

            if (_countryModel.Country.TLD.Count > 0)
                TLD.Text = $"Top-Level Domains: {String.Join(" ; ", _countryModel.Country.TLD)}";


            FIFA.Text = $"Code in FIFA: {_countryModel.Country.FIFA}";
            CIOC.Text = $"Code in IOC: {_countryModel.Country.CIOC}";
            IDDRoot.Text = $"IDD Root: {_countryModel.Country.IDD.Root}";

            IDDSuffixes.Inlines.Add(" ");   // Separate the next inline
            if (_countryModel.Country.IDD.Suffixes.Count > 5)
            {
                var inspectLink = new Hyperlink(new Run("Inspect"));
                inspectLink.Click += UnimplementedEvent;
                IDDSuffixes.Inlines.Add(inspectLink);
            }
            else
            {
                if (_countryModel.Country.IDD.Suffixes.Count == 0)
                    IDDSuffixes.Inlines.Add("n/a");
                else
                    IDDSuffixes.Inlines.Add(String.Join(", ", _countryModel.Country.IDD.Suffixes));
            }

            PostalCodeFormat.Text = $"Postal Code Format: {_countryModel.Country.PostalCode.Format}";
            PostalCodeRegex.Text = $"Postal Code Regex: {_countryModel.Country.PostalCode.Regex}";
        }

        // TODO replace all references with real event handlers
        /// <summary>
        /// Placeholder event
        /// </summary>
        private void UnimplementedEvent(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented :C", "Oops");
        }

        private async Task LoadFlagAsync()
        {
            if (_countryModel.Country.Flags.SVG == "n/a")
            {
                FlagImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/na.png"));
                return;
            }

            byte[] image;
            if (ConnectionStatus.Online)
            {
                var client = new HttpClient();
                image = await client.GetByteArrayAsync(_countryModel.Country.Flags.SVG);
                client.Dispose();
            }
            else
            {
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    DataService.FlagPath,
                    $"flag{_countryModel.Index:000}.svg"
                    );
                if (!File.Exists(path))
                {
                    FlagImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/na.png"));
                    return;
                }

                image = await File.ReadAllBytesAsync(path);
            }

            FlagImage.Source = BitmapSourceService.FromSvgByteArray(image);
        }

        private async Task LoadCOAAsync()
        {
            if (_countryModel.Country.CoatOfArms.SVG == "n/a")
            {
                COAImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/na.png"));
                return;
            }

            byte[] image;
            if (ConnectionStatus.Online)
            {
                var client = new HttpClient();
                image = await client.GetByteArrayAsync(_countryModel.Country.CoatOfArms.SVG);
                client.Dispose();
            }
            else
            {
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    DataService.COAPath,
                    $"coa{_countryModel.Index:000}.svg"
                    );
                if (!File.Exists(path))
                {
                    COAImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/na.png"));
                    return;
                }

                image = await File.ReadAllBytesAsync(path);
            }

            COAImage.Source = BitmapSourceService.FromSvgByteArray(image);
        }

        private async Task LoadGoogleMapsAsync()
        {
            if (!ConnectionStatus.Online || _countryModel.Country.Maps.GoogleMaps == "n/a")
            {
                GoogleMapsStatus.Text = " (Not Available)";
                return;
            }
            GoogleMapsStatus.Text = String.Empty;
            GoogleMapsBox.Visibility = Visibility.Visible;
            await GoogleMaps.LoadUrlAsync(_countryModel.Country.Maps.GoogleMaps);
            GoogleMaps.ZoomLevel = 3;
        }

        private async Task LoadOpenStreetMapsAsync()
        {
            if (!ConnectionStatus.Online || _countryModel.Country.Maps.OpenStreetMaps == "n/a")
            {
                OpenStreetMapsStatus.Text = " (Not Available)";
                return;
            }
            OpenStreetMapsStatus.Text = String.Empty;
            OpenStreetMapsBox.Visibility = Visibility.Visible;
            await OpenStreetMaps.LoadUrlAsync(_countryModel.Country.Maps.OpenStreetMaps);
            OpenStreetMaps.ZoomLevel = 1.5;
        }
    }
}

using Biblioteca;
using Biblioteca.Services;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Projeto
{
    public partial class ControlCountry : UserControl
    {
        private (int Index, Country? Country) _selected;
        private CancellationTokenSource? _cts;

        public (int Index, Country? Country) SelectedCountry
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateContent();
            }
        }
        public ControlCountry()
        {
            InitializeComponent();
        }

        private void UpdateContent()
        {
            _cts?.Cancel();
            _cts = new();
            if (SelectedCountry.Country == null)
            {
                PanelContent.Visible = false;
                Height = 0;
                return;
            }

            if (FormMain.Online)
            {
                _ = LoadImagesOnline(SelectedCountry.Country, _cts.Token);
                _ = LoadMaps(SelectedCountry.Country);
            }
            else
            { 
                _ = LoadImagesOffline(SelectedCountry.Country, SelectedCountry.Index, _cts.Token);
                ClearMaps();
            }

            LblOfficialName.Text = $"Official name: {SelectedCountry.Country.Name.Official}";
            LblIndependent.Text = $"Independent? {SelectedCountry.Country.Independent?.ToString() ?? "n/a"}";
            LblStatus.Text = $"Status: {SelectedCountry.Country.Status}";
            LblCCA2.Text = $"CCA2: {SelectedCountry.Country.CCA2}";
            LblCCA3.Text = $"CCA3: {SelectedCountry.Country.CCA3}";
            LblCCN3.Text = $"CCN3: {SelectedCountry.Country.CCN3}";
            LblUNMember.Text = $"UN Member? {SelectedCountry.Country.UNMember?.ToString() ?? "n/a"}";

            if (SelectedCountry.Country.Capital.Count > 1)
                LblCapital.Text = $"Capitals: {string.Join(", ", SelectedCountry.Country.Capital)}";
            else
                LblCapital.Text = $"Capital: {(SelectedCountry.Country.Capital.Count > 0 ? SelectedCountry.Country.Capital[0] : "none")}";

            LblRegion.Text = $"Region: {SelectedCountry.Country.Region}";
            LblSubregion.Text = $"Subregion: {SelectedCountry.Country.Subregion}";

            if (SelectedCountry.Country.LatLng[0] != 0 || SelectedCountry.Country.LatLng[1] != 0)
            {
                LblLatitude.Text = $"Latitude: {Math.Round(SelectedCountry.Country.LatLng[0], 3)}";
                LblLongitude.Text = $"Longitude: {Math.Round(SelectedCountry.Country.LatLng[1], 3)}";
            }
            else
            {
                LblLatitude.Text = $"Latitude: n/a";
                LblLongitude.Text = $"Longitude: n/a";
            }
            if (SelectedCountry.Country.CapitalInfo.LatLng[0] != 0 || SelectedCountry.Country.CapitalInfo.LatLng[1] != 0)
            {
                LblCapitalLat.Text = $"Capital Lat: {Math.Round(SelectedCountry.Country.CapitalInfo.LatLng[0], 3)}";
                LblCapitalLng.Text = $"Capital Lng: {Math.Round(SelectedCountry.Country.CapitalInfo.LatLng[1], 3)}";
            }
            else
            {
                LblCapitalLat.Text = $"Capital Lat: n/a";
                LblCapitalLng.Text = $"Capital Lng: n/a";
            }
            LblArea.Text = $"Area: {(SelectedCountry.Country.Area != -1 ? $"{SelectedCountry.Country.Area} km²" : "n/a")}";
            LblLandlocked.Text = $"Landlocked? {SelectedCountry.Country.Landlocked?.ToString() ?? "n/a"}";
            LblPopulation.Text = $"Population: {(SelectedCountry.Country.Population != -1 ? SelectedCountry.Country.Population : "n/a")}";
            LblStartOfWeek.Text = $"Start of Week: {SelectedCountry.Country.StartOfWeek}";

            if (SelectedCountry.Country.Gini.Count > 0)
            {
                var gini = SelectedCountry.Country.Gini.First();
                LblGini.Text = $"Gini: {gini.Value} ({gini.Key})";
            }
            else
            {
                LblGini.Text = $"Gini: n/a";
            }

            if (SelectedCountry.Country.Borders.Count > 0)
            {
                LblBorders.Text = "Borders: Inspect";
                LblBorders.LinkArea = new(9, 7);
            }
            else
            {
                LblBorders.Text = "Borders: none";
                LblBorders.LinkArea = new();
            }


            LblTLD.Text = $"Top-Level Domains: {(SelectedCountry.Country.TLD.Count > 0 ? string.Join(" ; ", SelectedCountry.Country.TLD) : "n/a")}";
            LblFIFA.Text = $"Code in FIFA: {SelectedCountry.Country.FIFA}";
            LblCIOC.Text = $"Code in IOC: {SelectedCountry.Country.CIOC}";
            LblIDDRoot.Text = $"IDD Root: {SelectedCountry.Country.IDD.Root}";
            if (SelectedCountry.Country.IDD.Suffixes.Count > 5)
            {
                LblIDDSuffixes.Text = $"Suffixes: Inspect";
                LblIDDSuffixes.LinkArea = new(10, 7);
            }
            else
            {
                LblIDDSuffixes.Text = $"Suffixes: {(SelectedCountry.Country.IDD.Suffixes.Count > 0 ? string.Join(", ", SelectedCountry.Country.IDD.Suffixes) : "n/a")}";
                LblIDDSuffixes.LinkArea = new();
            }

            LblPostalCodeFormat.Text = $"Postal Code Format: {SelectedCountry.Country.PostalCode.Format}";
            LblPostalCodeRegex.Text = $"Postal Code Regex: {SelectedCountry.Country.PostalCode.Regex}";

            PanelContent.Visible = true;
        }

        private async Task LoadImagesOnline(Country country, CancellationToken ct)
        {
            PicFlag.Image = PicFlag.InitialImage;
            PicFlag.AccessibleDescription = null;
            PicCOA.Image = PicCOA.InitialImage;
            var client = new HttpClient();
            try
            {
                var picFlag = (country.Flags.SVG != "n/a") ?
                    await BitmapService.FromSvgByteArrayAsync(await client.GetByteArrayAsync(country.Flags.SVG, ct), ct) :
                    Properties.Resources.naPng;

                ct.ThrowIfCancellationRequested();
                PicFlag.Image = picFlag;
                if (country.Flags.Alt != "n/a")
                    PicFlag.AccessibleDescription = country.Flags.Alt;

                var picCOA = (country.CoatOfArms.SVG != "n/a") ?
                    await BitmapService.FromSvgByteArrayAsync(await client.GetByteArrayAsync(country.CoatOfArms.SVG, ct), ct) :
                    Properties.Resources.naPng;

                ct.ThrowIfCancellationRequested();
                PicCOA.Image = picCOA;
            }
            catch (OperationCanceledException) { }
            catch
            {
                PicFlag.Image = Properties.Resources.naPng;
                PicCOA.Image = Properties.Resources.naPng;
            }
            finally { client.Dispose(); }
        }

        private async Task LoadImagesOffline(Country country, int index, CancellationToken ct)
        {
            PicFlag.Image = PicFlag.InitialImage;
            PicFlag.AccessibleDescription = null;
            PicCOA.Image = PicCOA.InitialImage;
            try
            {
                var picFlag = (country.Flags.SVG != "n/a") ?
                    await BitmapService.FromSvgByteArrayAsync(await File.ReadAllBytesAsync(@$"{DataService.FlagPath}\flag{index:000}.svg", ct), ct) :
                    Properties.Resources.naPng;

                ct.ThrowIfCancellationRequested();
                PicFlag.Image = picFlag;
                if (country.Flags.Alt != "n/a")
                    PicFlag.AccessibleDescription = country.Flags.Alt;

                var picCOA = (country.CoatOfArms.SVG != "n/a") ?
                    await BitmapService.FromSvgByteArrayAsync(await File.ReadAllBytesAsync(@$"{DataService.COAPath}\coa{index:000}.svg", ct), ct) :
                    Properties.Resources.naPng;

                ct.ThrowIfCancellationRequested();
                PicCOA.Image = picCOA;
            }
            catch (OperationCanceledException) { }
            catch
            {
                PicFlag.Image = Properties.Resources.naPng;
                PicCOA.Image = Properties.Resources.naPng;
            }
        }

        private void ClearMaps()
        {
            SuspendLayout();
            LblGoogleMaps.Text = "Google Maps (unavailable)";
            WebGoogleMaps.Visible = false;
            WebGoogleMaps.Height = 0;

            LblOpenStreetMaps.Text = "OpenStreetMaps (unavailable)";
            WebOpenStreetMaps.Visible = false;
            WebOpenStreetMaps.Height = 0;
            CalculateSizes();
            ResumeLayout(true);
        }

        private Task LoadMaps(Country country)
        {
            ClearMaps();
            try
            {
                if (country.Maps.GoogleMaps != "n/a")
                {
                    LblGoogleMaps.Text = "Google Maps (loading...)";
                    WebGoogleMaps.Source = new Uri(country.Maps.GoogleMaps);
                    //WebGoogleMaps.Height = WebGoogleMaps.Width * 9 / 16;
                    //Height += WebGoogleMaps.Height;
                    //WebGoogleMaps.Visible = true;
                }
                if (country.Maps.OpenStreetMaps != "n/a")
                {
                    LblOpenStreetMaps.Text = "OpenStreetMaps (loading...)";
                    WebOpenStreetMaps.Source = new Uri(country.Maps.OpenStreetMaps);
                    //WebOpenStreetMaps.Height = WebOpenStreetMaps.Width * 9 / 16;
                    //Height += WebOpenStreetMaps.Height;
                    //WebOpenStreetMaps.Visible = true;
                }
            }
            catch
            {
                ClearMaps();
            }
            return Task.CompletedTask;
        }

        private void WebPageContentLoaded(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            if (_cts?.IsCancellationRequested ?? false) return;
            WebView2 page = (WebView2)sender;

            if (page == WebGoogleMaps)
            {
                LblGoogleMaps.Text = "Google Maps";
                if (e.IsErrorPage)
                {
                    LblGoogleMaps.Text = "Google Maps (unavailable)";
                    return;
                }
            }
            if (page == WebOpenStreetMaps)
            {
                LblOpenStreetMaps.Text = "OpenStreetMaps";
                if (e.IsErrorPage)
                {
                    LblOpenStreetMaps.Text = "OpenStreetMaps (unavailable)";
                    return;
                }
            }
            page.Height = page.Width * 9 / 16;

            SuspendLayout();
            page.Visible = true;
            CalculateSizes();
            ResumeLayout(true);
        }

        private void InspectAltSpellings(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectNativeNames(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectTranslations(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectContinents(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectBorders(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectLanguages(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectDemonyms(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectCar(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectTimezones(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void InspectSuffixes(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Not implemented", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CalculateSizes()
        {
            Height = Width * 9 / 8;
            PanelContent.Font = new Font("Segoe UI", Math.Min(4F + PanelContent.Width / 100F, 20F));
            TableLayoutPanelImages.Height = 100 + PanelContent.Width / 10;
            if (WebGoogleMaps.Visible)
            {
                WebGoogleMaps.Height = WebGoogleMaps.Width * 9 / 16;
                Height += WebGoogleMaps.Height;
            }
            if (WebOpenStreetMaps.Visible)
            {
                WebOpenStreetMaps.Height = WebOpenStreetMaps.Width * 9 / 16;
                Height += WebOpenStreetMaps.Height;
            }
        }

        private void PanelContent_SizeChanged(object sender, EventArgs e) => CalculateSizes();
    }
}

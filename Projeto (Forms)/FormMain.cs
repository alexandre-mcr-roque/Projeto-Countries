using Biblioteca;
using Biblioteca.Services;
using Biblioteca.Services.Reports;
using Microsoft.Web.WebView2.Core;
using System;

namespace Projeto
{
    public partial class FormMain : Form
    {
        private Progress<ProgressReportModel> _progress;
        private Task? _saveTask;

        public static bool Online { get; private set; }

        private List<Country> _countries = [];
        public FormMain()
        {
            _progress = new();
            _progress.ProgressChanged += ReportProgress;

            InitializeComponent();
            CheckConnection();
            _ = LoadCountries();
        }

        private void CheckConnection()
        {
            Online = NetworkService.CheckConnection().Success;
            ChangeStatusText();
            LblStatus.Visible = true;
        }

        private void ChangeStatusText()
        {
            if (Online)
            {
                LblStatus.Text = "ONLINE";
                LblStatus.ForeColor = Color.MediumSeaGreen;
            }
            else
            {
                LblStatus.Text = "OFFLINE";
                LblStatus.ForeColor = Color.OrangeRed;
            }
        }

        private async Task LoadCountries()
        {
            var response = Online ? await ApiService.GetCountries(CancellationToken.None, _progress) : await DataService.LoadDataAsync(CancellationToken.None, _progress);
            if (!response.Success)
            {
                LblProgress.Text = response.Message?.ToString();
                return;
            }
            _countries = response.Message as List<Country> ?? [];
            CreateSidebar();
            if (Online)
                _saveTask = DataService.SaveDataAsync(_countries, CancellationToken.None, _progress);
        }

        private void CreateSidebar(string? filter = null)
        {
            ScrollPanelCountries.SuspendLayout();
            ScrollPanelCountries.Controls.Clear();
            var _filteredCountries = filter == null ? _countries : _countries.FindAll(c => c.Name.Common.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase));
            ScrollPanelCountries.Top = 0;
            ScrollPanelCountries.Height = _filteredCountries.Count * 50 + 50;

            // Add a "none" button
            Button button = new();
            button.Margin = new(0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Width = 200;
            button.Height = 50;
            button.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button.Text = "(Reset)";

            button.Click += async (s, e) => await UpdateCountry(null, 0);

            ScrollPanelCountries.Controls.Add(button);

            foreach (var country in _filteredCountries.OrderBy((c) => c.Name.Common))
            {
                float fontSize = 14F;
                if (country.Name.Common.Length > 16)
                    fontSize = 12F;
                //if (country.Name.Common.Length > 24)
                //    fontSize = 9F;

                button = new();
                button.Margin = new(0);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Width = 200;
                button.Height = 50;
                button.Font = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Point, 0);
                button.Text = country.Name.Common;

                button.Click += async (s, e) => await UpdateCountry(country, _countries.IndexOf(country) + 1);

                ScrollPanelCountries.Controls.Add(button);
            }
            ScrollPanelCountries.MouseWheel += ScrollPanelCountries_Scroll;
            ScrollPanelCountries.ResumeLayout(true);
        }

        private Task UpdateCountry(Country? country, int index)
        {
            LblSelectedCountry.Text = $"{country?.ToString() ?? "(Select a Country)"}";
            PanelCountry.SelectedCountry = (index, country);
            return Task.CompletedTask;
        }

        private void ScrollPanelCountries_Scroll(object? sender, MouseEventArgs e)
        {
            ScrollPanelCountries.SuspendLayout();
            int maxTop = PanelCountries.Height - ScrollPanelCountries.Height;
            int newTop = ScrollPanelCountries.Top + e.Delta/2;
            if (newTop < maxTop)
                newTop = maxTop;
            if (newTop > 0)
                newTop = 0;
            ScrollPanelCountries.Top = newTop;
            ScrollPanelCountries.ResumeLayout(true);
        }

        private void PanelCountries_Resize(object sender, EventArgs e)
        {
            ScrollPanelCountries.SuspendLayout();
            int maxTop = PanelCountries.Height - ScrollPanelCountries.Height;
            if (maxTop < 0 && ScrollPanelCountries.Top < maxTop)
                ScrollPanelCountries.Top = maxTop;
            ScrollPanelCountries.ResumeLayout(true);
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            if (_countries.Count == 0) return;
            string filter = TxtFilter.Text.Trim();
            CreateSidebar(string.IsNullOrEmpty(filter) ? null : filter);
        }

        private void ReportProgress(object? sender, ProgressReportModel e)
        {
            ProgressBar.Value = (int)Math.Floor(e.PercentageCompleted());
            if (e.GetType() == typeof(CountryLoadProgressReport))
            {
                LblProgress.Text = $"{ProgressBar.Value}% of countries loaded";
                if (ProgressBar.Value == 100) LblProgress.Text = $"Successfully loaded countries";
            }
            if (e.GetType() == typeof(CountrySaveProgressReport))
            {
                CountrySaveProgressReport report = (CountrySaveProgressReport)e;
                if (report.TotalBytes < 0)
                {
                    LblProgress.Text = "Calculating data to save...";
                    return;
                }
                LblProgress.Text = $"{ProgressBar.Value}% of countries saved ({DataService.SimplifyBytes(report.SavedBytes)}/{DataService.SimplifyBytes(report.TotalBytes)})";
                if (ProgressBar.Value == 100) LblProgress.Text = $"Successfully saved countries";
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_saveTask?.IsCompleted ?? true) return;

            e.Cancel = MessageBox.Show("The app isn't done saving!\nAre you sure you want to quit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
        }
    }
}

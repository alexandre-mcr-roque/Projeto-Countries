using Biblioteca;
using Biblioteca.Services;
using Biblioteca.Services.Reports;
using Projeto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projeto;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Task? _saveTask;
    private ICollectionView? _countries;
    private readonly List<CountryControl> _loadedControls = [];

    public MainWindow()
    {
        InitializeComponent();
        CheckConnectivity();
        _= LoadCountriesAsync();
    }

    private void CheckConnectivity()
    {
        ConnectionStatus.CheckConnectivity();
        if (ConnectionStatus.Online)
        {
            Status.Text = "ONLINE";
            Status.Foreground = Brushes.LightSeaGreen;
        }
        else
        {
            Status.Text = "OFFLINE";
            Status.Foreground = Brushes.OrangeRed;
        }
    }

    private async Task LoadCountriesAsync()
    {
        var progress = new Progress<ProgressReportModel>();
        progress.ProgressChanged += LoadProgressChanged;

        // Doing Task.Run on async methods looks counter-intuitive but it
        // separates it from the dispatcher
        Response response;
        if (ConnectionStatus.Online)
            response = await Task.Run(() => ApiService.GetCountries(CancellationToken.None, progress));
        else
            response = await Task.Run(() => DataService.LoadDataAsync(CancellationToken.None, progress));

        if (!response.Success)
        {
            ProgressStatus.Text = response.Message?.ToString();
            return;
        }
        // Unsubscribe the event
        progress.ProgressChanged -= LoadProgressChanged;

        List<Country> countries = response.Message as List<Country> ?? [];
        _countries = CollectionViewSource.GetDefaultView(
            new List<CountryModel>(countries
                .Select((country, index) => new CountryModel(index+1, country)))
            .OrderBy(m => m.Country.Name.Common)
        );
        
        CountryList.ItemsSource = _countries;
        LoadCountry();

        if (ConnectionStatus.Online)
        {
            progress.ProgressChanged += SaveProgressChanged;
            _saveTask = Task.Run(() => DataService.SaveDataAsync(countries, CancellationToken.None, progress));
        }
    }

    private void FilterTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_countries == null) return;
        _countries.Filter = FilterByCountryName;
        _countries.Refresh();
    }

    private bool FilterByCountryName(object obj)
    {
        CountryModel model = (CountryModel)obj;
        return model.Country.Name.Common.StartsWith(CountryFilter.Text, StringComparison.OrdinalIgnoreCase);
    }

    private void CountrySelected(object sender, SelectionChangedEventArgs e) => LoadCountry();
    private void LoadCountry()
    {
        if (CountryList.SelectedItem is not CountryModel model)
        {
            SelectedCountryName.Text = "(Select a Country)";
            SelectedCountryContent.Content = null;
            return;
        }

        var control = _loadedControls.Find(c => c.CountryModel.Index == model.Index);
        if (control != null)
            _loadedControls.Remove(control);
        else
            control = new CountryControl(model);

        // Keep only 5 controls active at a time
        // (the control to be loaded will be added after)
        if (_loadedControls.Count >= 5)
        {
            var last = _loadedControls[0];
            _loadedControls.RemoveAt(0);
            // Dispose the web browsers
            last.GoogleMaps.Dispose();
            last.OpenStreetMaps.Dispose();
        }

        _loadedControls.Add(control);
        SelectedCountryName.Text = model.ToString();
        SelectedCountryContent.Content = control;
    }

    private void LoadProgressChanged(object? sender, ProgressReportModel e)
    {
        double value = e.PercentageCompleted();
        Progress.Value = value;

        if (value < 100)
            ProgressStatus.Text = $"{value:0}% of countries loaded";
        else
            ProgressStatus.Text = "Successfully loaded countries";
    }

    private void SaveProgressChanged(object? sender, ProgressReportModel e)
    {
        double value = e.PercentageCompleted();
        Progress.Value = value;

        if (e is not CountrySaveProgressReport progress) return;

        if (progress.TotalBytes < 0)
        {
            ProgressStatus.Text = "Calculating data to save...";
            return;
        }

        if (value < 100)
            ProgressStatus.Text = $"{value:0}% of countries saved ({DataService.SimplifyBytes(progress.SavedBytes)}/{DataService.SimplifyBytes(progress.TotalBytes)})";
        else
            ProgressStatus.Text = "Successfully saved countries";
    }

    private void TryClose(object sender, CancelEventArgs e)
    {
        if (_saveTask != null && !_saveTask.IsCompleted)
        {
            var result = MessageBox.Show("The app isn't done saving!\nAre you sure you want to quit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) e.Cancel = true;
        }
        // Dispose the web browsers
        _loadedControls.ForEach(control =>
        {
            control.GoogleMaps.Dispose();
            control.OpenStreetMaps.Dispose();
        });
    }
}
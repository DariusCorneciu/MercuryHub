using System.Configuration;
using System.Data;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MercuryHub.Services;
using MercuryHub.ViewModels;
using MercuryHub.Views;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Fix: Access the API key using ConfigurationManager instead of Properties.Settings
        string apiKey = MercuryHub.Properties.Settings.Default.ApiKey;

        if (string.IsNullOrEmpty(apiKey))
        {
            var apiKeyPrompt = new ApiKeyPromptView();
            bool? result = apiKeyPrompt.ShowDialog();

            if (result != true)
            {
                Current.Shutdown();
                return;
            }
        }
        using (var context = new ApplicationDbContext())
        {
            context.Database.Migrate();
            SeedData.Initialize(context);
        }

        var bookingService = new BookingService(apiKey);
        await bookingService.CreateFontDeskRolesAsync();
        var mainWindow = new MainWindow();
        mainWindow.Show();


    }
}


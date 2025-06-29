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
using MercuryHub.ViewModels;
using MercuryHub.CustomControls;
using System.Windows.Threading;
using MercuryHub.Services;
using MercuryHub.Models;
using Microsoft.EntityFrameworkCore;

namespace MercuryHub;

/// <summary>  
/// Interaction logic for MainWindow.xaml  
/// </summary>  
public partial class MainWindow : Window
{
    private DispatcherTimer _syncTimer;
    private MainWindowViewModel _viewModel;
    private BookingService _bookingService = new BookingService(MercuryHub.Properties.Settings.Default.ApiKey);
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel();
        DataContext = _viewModel;
        CustomControls.ToastManager.Instance = ToastManager;

        SyncAndUpdateViewModel();
        SetupAutoSyncTimer();
    }

    private void SetupAutoSyncTimer()
    {
        _syncTimer = new DispatcherTimer();
        _syncTimer.Interval = TimeSpan.FromMinutes(5); 
        _syncTimer.Tick += async (sender, args) =>
        {
            await _bookingService.SyncReservationsAsync();
            await UpdateReservationsAsync();
        };
        _syncTimer.Start();
    }
    public async void SyncAndUpdateViewModel()
    {
        ToastManager.Show("Reservations Synced!", ToastType.Success);
        await _bookingService.SyncReservationsAsync();
       await UpdateReservationsAsync();
       

    }

    private async Task UpdateReservationsAsync()
    {
        using var _localContext = new ApplicationDbContext();

        var reservations =await _localContext.Reservations.Include(r=>r.requestedRooms).Include(p=>p.Client).ToListAsync();
        Application.Current.Dispatcher.Invoke(() =>
        {
            _viewModel.Reservations.Clear();
            foreach (var r in reservations)
                _viewModel.Reservations.Add(r);
        });
    }
}


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MercuryHub.Models;
using MercuryHub.Services;
using Microsoft.AspNetCore.Identity;
using MercuryHub.CustomControls;
using MercuryHub.ViewModels;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Threading;


namespace MercuryHub.ViewModels;


public class NavBarViewModel : ViewModelBase
{
    private string _username;
  
    private string _role;

    private bool _isAdmin;

    public event EventHandler<string>? ChangeView;
    private bool _isPopupOpen { get; set; }

    public bool IsPopupOpen
    {
        get { return _isPopupOpen; }
        set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); }
    }
    private string _selectedHotelName { get; set; }

    public string SelectedHotelName
    {
        get => _selectedHotelName;
        set
        {
            _selectedHotelName = value;
            OnPropertyChanged(nameof(SelectedHotelName));
        }
    }

    public ObservableCollection<string> _properties { get; set; }

    public ObservableCollection<string> Properties
    {
        get => _properties;
        set
        {
            _properties = value;
            OnPropertyChanged(nameof(Properties));
        }
    }
    public bool isAdmin
    {
        get => _isAdmin;
        set
        {
            _isAdmin = value;
            OnPropertyChanged(nameof(isAdmin));
        }
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }
    public string UserRole
    {
        get => _role;
        set
        {
            _role = value;
            OnPropertyChanged(nameof(UserRole));
        }
    }


  
    public ICommand ChangeViewCommand { get; }
    public ICommand ChangeViewShowCommand { get; }
    public ICommand ChangeViewEditCommand { get; }

    public ICommand OpenPopUpCommand { get; }

    
    public ICommand SyncCommand { get; }
    private DateTime _lastExecuted = DateTime.MinValue;
    private DispatcherTimer _timer;
    public NavBarViewModel(string username,string role)
    {
        SelectedHotelName = string.Empty;
        // Height="720" Width="1280"
        height = -1;
        width = -1;
        Username = username;
        UserRole =role;
        var principal = Thread.CurrentPrincipal;
        IsPopupOpen = false;
        if (principal != null)
        {
            if (principal.IsInRole("Admin"))
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
           
        }
        ChangeViewShowCommand = new RelayCommand(ExecuteChangeViewShow);
        ChangeViewEditCommand = new RelayCommand(ExecuteChangeViewEdit);

        OpenPopUpCommand = new RelayCommand(ExecuteOpenPopUp);
        ChangeViewCommand = new RelayCommand(ExecuteChangeView);
        SyncCommand = new RelayCommand(ExecuteSyncCommand, CanExecuteSync);
        var db = new ApplicationDbContext();
        _isPopupOpen = false;
        _properties = new ObservableCollection<string>();
     
        var pr = db.Properties.ToList();
        foreach (var property in pr) {
            _properties.Add(property.Name);
        }
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(10);
        _timer.Tick += (s, e) => CommandManager.InvalidateRequerySuggested();
        _timer.Start();

        ToastManager.Show($"Welcome to Mercury Hub, {username}!", ToastType.Success);
        
    }

    private void ExecuteOpenPopUp(object obj)
    {
      try
        {
           
            SelectedHotelName = (string)obj;
           
            _isPopupOpen = true;
            IsPopupOpen = true;
         
        }
        catch(Exception ex)
        {
            ToastManager.Show("Error at popup Execute!", ToastType.Error);
        }
    }

    private void ExecuteChangeViewShow(object obj)
    {
        var name = (string)obj;
        if (!string.IsNullOrEmpty(name))
        {
            var composed = $"Diagram:{name}";

            ChangeView.Invoke(this, composed);
            ToastManager.Show($"Changed to page {name} - Reservation Pannel!", ToastType.Info);
        }
        else
        {
            ToastManager.Show($"Failed to change page, name is null!", ToastType.Error);
        }
        IsPopupOpen = false;
    }

    private void ExecuteChangeViewEdit(object obj)
    {
        var name = (string)obj;
        if (!string.IsNullOrEmpty(name))
        {
            var composed = $"Edit:{name}";

            ChangeView.Invoke(this, composed);
            ToastManager.Show($"Changed to page {name} - Reservation Edit!", ToastType.Info);
        }
        else
        {
            ToastManager.Show($"Failed to change page, name is null!", ToastType.Error);
        }
        IsPopupOpen = false;
    }

  

    private bool CanExecuteSync(object obj){
        return (DateTime.Now - _lastExecuted).TotalMinutes >= 2;

    }

    private void ExecuteSyncCommand(object obj)
    {
        _lastExecuted = DateTime.Now;
        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.SyncAndUpdateViewModel();
        CommandManager.InvalidateRequerySuggested();
    }

    private void ExecuteChangeView(object obj)
    {
        var name = (string)obj;
        if (!string.IsNullOrEmpty(name))
        {
            ChangeView.Invoke(this, name);
            ToastManager.Show($"Changed to page {name}", ToastType.Info);
        }
        else
        {
            ToastManager.Show($"Failed to change page, name is null!", ToastType.Error);
        }

        
    }

  

  

}

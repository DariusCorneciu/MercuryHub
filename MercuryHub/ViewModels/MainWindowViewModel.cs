using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MercuryHub.Models;
using MercuryHub.Services;
using MercuryHub.Views;

namespace MercuryHub.ViewModels
{


    public class MainWindowViewModel : ViewModelBase
    {
        private UserControl? _currentView;
        private LoginView _loginView;
        private DashboardView? _dashboardView;
        private UserControl? _navbar;
        public ObservableCollection<Reservation> Reservations { get; set; }


        private List<Property> Properties = new List<Property>();
        public UserControl? CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }
        public UserControl? Navbar
        {
            get => _navbar;
            set
            {
                if (_navbar != value)
                {
                    _navbar = value;
                    OnPropertyChanged(nameof(Navbar));
                }
            }
        }

        public MainWindowViewModel()
        {
            Reservations = new ObservableCollection<Reservation>();
            InitializeViews();
        }

        private void InitializeViews()
        {
            _loginView = new LoginView();
            var loginVM = (LoginViewModel)_loginView.DataContext;
            loginVM.LoginSucceeded += OnLoginSucceeded;
            CurrentView = _loginView;
            setCurrentViewSize(loginVM);


        }

        private void OnLoginSucceeded(object? sender, Tuple<string, string> tuple)
        {
            _dashboardView = new DashboardView(tuple.Item1, tuple.Item2);
            Navbar = _dashboardView;
             var navBarViewModel = (NavBarViewModel)_dashboardView.DataContext;
             setCurrentViewSize(navBarViewModel);
            navBarViewModel.ChangeView += OnChangedView;

            
            
            var db = new ApplicationDbContext();
            var prop = db.Properties.ToList();
            foreach(var p in prop)
            {
                Properties.Add(p);
            }


            CurrentView = new GuestView();
        }

        private void OnChangedView(object? sender,string name)
        {
           
            switch (name)
            {
                case "CreateAccount":
                    CurrentView = new CreateAccount();
                    break;
                case "Guests":
                    CurrentView = new GuestView(); break;
                case "Reports":
                    CurrentView = new ReportsView();break;
               default:
                    {
                        var spliter = name.Split(':');
                        if(spliter.Length >= 2)
                        {
                            if (Properties.Select(p => p.Name).Contains(spliter[1]))
                            {
                                var property = Properties.Where(p => p.Name == spliter[1]).First();


                                if (spliter[0] == "Diagram")
                                {
                                    CurrentView = new ReservationsView(property, Reservations);
                                }else
                                {
                                    CurrentView = new ReservationEditor(property);
                                }
                            }
                        }
                        
                    }break;
                   

            }
            
        }
        

        private void setCurrentViewSize(ViewModelBase request)
        {

            if(request.height == -1 || request.width == -1)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;

            }
            else
            {
                Application.Current.MainWindow.Width = request.width;
                Application.Current.MainWindow.Height = request.height;
            }
                
            
        }
        

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MercuryHub.ViewModels;

namespace MercuryHub.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
        public DashboardView(string username,string role) : this()
        {
            InitializeComponent();
            var viewModel = new NavBarViewModel(username,role);
            DataContext = viewModel;
            if (viewModel.isAdmin)
            {
                CreateAccountItem.Visibility = Visibility.Visible;
            }
            else
            {
                CreateAccountItem.Visibility = Visibility.Collapsed;
            }
        }
    }
}

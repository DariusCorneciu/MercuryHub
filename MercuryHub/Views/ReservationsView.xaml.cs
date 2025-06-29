using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MercuryHub.CustomControls;
using MercuryHub.Models;
using MercuryHub.ViewModels;

namespace MercuryHub.Views
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    /// 

    public partial class ReservationsView : UserControl
    {
        public ReservationsView(Property property, ObservableCollection<Reservation> reservations)
        {
            InitializeComponent();
            DataContext = new ReservationViewModel(property,reservations);
        }

        private void ReservationItemClick(object sender, RoutedEventArgs e)
        {
            
            var reservation = ((FrameworkElement)sender).DataContext;

            var res = (ReservationDTO)reservation;
            var panel = new ReservationDetailsPanel(res);
           

           
            RightPanel.Content = panel;
            RightPanel.Visibility = Visibility.Visible;
        }
        private void BorderLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var reservation = ((FrameworkElement)sender).DataContext;
            var view = (ReservationViewModel)DataContext;
            view.IsPopupOpen = true;

            var res = (ReservationDTO)reservation;
            view.CancelReservation = res;
            view.CancelReason = null;
        }

    }
}

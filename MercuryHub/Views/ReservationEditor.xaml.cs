using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using MercuryHub.Helpers;
using MercuryHub.Models;
using MercuryHub.ViewModels;

namespace MercuryHub.Views
{
   
    public partial class ReservationEditor : UserControl
    {
     
        public ReservationEditor(Property property)
        {
            InitializeComponent();
            DataContext = new ReservationEditorViewModel(property);
        }
        private void ResItemClick(object sender, RoutedEventArgs e)
        {

            var guest = ((FrameworkElement)sender).DataContext;

            var gue = (Reservation)guest;
            var panel = new EditResDetailsPanel(gue);
            RightPanel.Content = panel;
            RightPanel.Visibility = Visibility.Visible;
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberRestictionHelper.NumberOnly_PreviewTextInput(sender, e);
        }

        private void NumberOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            NumberRestictionHelper.NumberOnly_Pasting(sender, e);
        }

       
        private void DoubleOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberRestictionHelper.DoubleOnly_PreviewTextInput(sender, e);
        }

        private void DoubleOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            NumberRestictionHelper.DoubleOnly_Pasting(sender, e);
        }

        




    }
}

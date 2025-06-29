using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MercuryHub.Helpers;
using MercuryHub.Models;
using MercuryHub.ViewModels;

namespace MercuryHub.CustomControls
{
    /// <summary>
    /// Interaction logic for GuestDetailsPanel.xaml
    /// </summary>
    public partial class EditResDetailsPanel : UserControl
    {
        public EditResDetailsPanel(Reservation value)
        {
            DataContext = new EditResDetailsPanelViewModel(value);
            InitializeComponent();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var view = (EditResDetailsPanelViewModel)DataContext;

            view.AvabileRooms.Clear();
            view.SelectedRooms.Clear();
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

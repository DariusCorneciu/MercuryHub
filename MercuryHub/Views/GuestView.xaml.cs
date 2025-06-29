using System;
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
using MercuryHub.CustomControls;
using MercuryHub.ViewModels;

namespace MercuryHub.Views
{
    /// <summary>
    /// Interaction logic for GuestView.xaml
    /// </summary>
    public partial class GuestView : UserControl
    {
        public GuestView()
        {
            InitializeComponent();
            DataContext = new GuestViewModel();
        }

        private void GuestItemClick(object sender, RoutedEventArgs e)
        {

            var guest = ((FrameworkElement)sender).DataContext;
          
            var gue = (GuestDTO)guest;
            var panel = new GuestDetailsPanel(gue);

            



            RightPanel.Content = panel;
            RightPanel.Visibility = Visibility.Visible;
        }

    }
}

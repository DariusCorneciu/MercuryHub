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
using MercuryHub.Models;
using MercuryHub.Services;
using MercuryHub.ViewModels;

namespace MercuryHub.CustomControls
{
    /// <summary>
    /// Interaction logic for ReservationDetailsPanel.xaml
    /// </summary>
    public partial class ReservationDetailsPanel : UserControl
    {
        public ReservationDetailsPanel(ReservationDTO value)
        {
            DataContext = new ReservationDetailPanelViewModel(value);
            InitializeComponent();
            
        }

        

       



   






        }
}

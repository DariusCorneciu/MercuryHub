using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MercuryHub.Services;

namespace MercuryHub.Views
{
    /// <summary>
    /// Interaction logic for ApiKeyPromptView.xaml
    /// </summary>
    public partial class ApiKeyPromptView : Window
    {
        public ApiKeyPromptView()
        {
            InitializeComponent();
        }
        private async void SaveApiKey_Click(object sender, RoutedEventArgs e)
        {
            var key = ApiKeyTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(key))
            {
                var service = new BookingService(key);
                var response = await service.TestApiKeyAsync();
                if (response)
                {
                    Properties.Settings.Default.ApiKey = key;
                    Properties.Settings.Default.Save();

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unknown key");
                    this.DialogResult = false;
                }

                
            }
            else
            {
                MessageBox.Show("Empty Key");
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.DialogResult != true)
            {
                e.Cancel = true; 
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;

namespace MercuryHub.CustomControls
{
    public partial class ToastManager : UserControl
    {
        private static ToastManager _instance;
        public static ToastManager Instance
        {
            get => _instance;
            set => _instance = value;
        }

        public ToastManager()
        {
            InitializeComponent();
        }

        public void ShowToast(string message, ToastType type = ToastType.Info)
        {
            var toast = new ToastMessage
            {
                Message = message,
                Type = type,
                Opacity = 0
            };

            ToastContainer.Children.Add(toast);
            toast.Show();

            // Auto-remove after 5 seconds
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            timer.Tick += (s, e) =>
            {
                timer.Stop();
                if (ToastContainer.Children.Contains(toast))
                {
                    var animation = new System.Windows.Media.Animation.DoubleAnimation
                    {
                        From = 1,
                        To = 0,
                        Duration = TimeSpan.FromSeconds(0.3)
                    };

                    animation.Completed += (sender, args) => ToastContainer.Children.Remove(toast);
                    toast.BeginAnimation(UIElement.OpacityProperty, animation);
                }
            };

            timer.Start();
        }

        public static void Show(string message, ToastType type = ToastType.Info)
        {
            if (Instance == null)
            {
                throw new InvalidOperationException("ToastManager.Instance has not been initialized");
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Instance.ShowToast(message, type);
            });
        }
    }
} 
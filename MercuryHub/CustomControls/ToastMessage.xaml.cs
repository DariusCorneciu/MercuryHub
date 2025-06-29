using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MercuryHub.CustomControls
{
    public partial class ToastMessage : UserControl
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastMessage), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(ToastType), typeof(ToastMessage), new PropertyMetadata(ToastType.Info, OnTypeChanged));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public ToastType Type
        {
            get => (ToastType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public ToastMessage()
        {
            InitializeComponent();
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ToastMessage toast)
            {
                toast.UpdateIcon();
            }
        }

        private void UpdateIcon()
        {
            switch (Type)
            {
                case ToastType.Success:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.CheckCircle;
                    Icon.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case ToastType.Error:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                    Icon.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case ToastType.Warning:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning;
                    Icon.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case ToastType.Info:
                default:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Information;
                    Icon.Foreground = new SolidColorBrush(Colors.Blue);
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Panel;
            if (parent != null)
            {
                var animation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                animation.Completed += (s, _) => parent.Children.Remove(this);
                BeginAnimation(OpacityProperty, animation);
            }
        }

        public void Show()
        {
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            BeginAnimation(OpacityProperty, animation);
        }
    }

    public enum ToastType
    {
        Info,
        Success,
        Warning,
        Error
    }
} 
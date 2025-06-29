using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace MercuryHub.Helpers
{
    
    public static class NumberRestictionHelper
        {
        public static void DoubleOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !IsTextDouble(fullText);
        }

        public static void DoubleOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextDouble(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }
        public static void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        public static void NumberOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }



        private static bool IsTextDouble(string text)
        {
            return Regex.IsMatch(text, @"^\d*([.,]\d*)?$");
        }

        private static bool IsTextNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
    }
}

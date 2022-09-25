using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace TreeSize.App
{
    public class FolderIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;

            if (value is bool isFolder)
            {
                visibility = isFolder ? Visibility.Visible : Visibility.Collapsed;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

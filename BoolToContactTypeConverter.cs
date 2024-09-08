using System;
using System.Globalization;
using System.Windows.Data;

namespace VendorRad
{
    public class BoolToContactTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Assuming true is for 'Customer' and false is for 'Vendor'
            if (value is bool isChecked)
            {
                return isChecked ? "Customer Details" : "Vendor Details";
            }
            return "Contact Details";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfDocViewer.Model
{
   [Serializable]
   public  class BoolToRowHeightConverter : IValueConverter
   {
       private static readonly BoolToRowHeightConverter _instance = new BoolToRowHeightConverter();
       public static BoolToRowHeightConverter Instance
       {
           get
           {
               return _instance;
           }
       }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return new GridLength(1, GridUnitType.Star);
            else
                return GridLength.Auto;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        //public static readonly BoolToRowHeightConverter Instance = new BoolToRowHeightConverter();
            

    }
}

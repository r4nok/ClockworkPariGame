using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfApplication.View.Converters
{
    public class Converter_MultipleCommandParameters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is ViewModels.EditType && values[1] is Model.Tournament)
            {
                return new Tuple<ViewModels.EditType, Model.Tournament>((ViewModels.EditType)values[0], (Model.Tournament)values[1]);
            }

            if (values[0] is ViewModels.EditType && values[1] is Model.Team)
            {
                return new Tuple<ViewModels.EditType, Model.Team>((ViewModels.EditType)values[0], (Model.Team)values[1]);
            }

            if (values[0] is Model.Tournament && values[1] is Model.Team)
            {
                return new Tuple<Model.Tournament, Model.Team>((Model.Tournament)values[0], (Model.Team)values[1]);
            }

            if (values[0] is ListBox && values[1] is Model.Tournament)
            {
                return new Tuple<ListBox, Model.Tournament>((ListBox)values[0], (Model.Tournament)values[1]);
            }

            if (values[0] is int && values[1] is int)
            {
                return new Tuple<int, int>((int)values[0], (int)values[1]);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

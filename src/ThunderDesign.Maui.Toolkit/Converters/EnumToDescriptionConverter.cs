using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderDesign.Net.ToolBox.Extentions;

namespace ThunderDesign.Maui.Toolkit.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null");
            }
            else if (!value.GetType().IsEnum)
            {
                throw new ArgumentException("Value must be an enum type", nameof(value));
            }

            return ((Enum)value).GetDescription();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

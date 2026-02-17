using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using GymBro.Domain.Entities;

namespace GymBro.App.Converters
{
    public class EquipmentToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<Equipment> equipment && equipment.Any())
            {
                return string.Join(", ", equipment.Select(e => e.Name));
            }
            return "нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
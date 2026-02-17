using System;
using System.Globalization;
using System.Windows.Data;

namespace GymBro.App.Converters
{
    /// <summary>
    /// Конвертер для преобразования числовой сложности в звездочки
    /// </summary>
    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int difficulty)
            {
                switch (difficulty)
                {
                    case 1: return "★☆☆☆☆";
                    case 2: return "★★☆☆☆";
                    case 3: return "★★★☆☆";
                    case 4: return "★★★★☆";
                    case 5: return "★★★★★";
                    default: return "☆☆☆☆☆";
                }
            }
            return "☆☆☆☆☆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
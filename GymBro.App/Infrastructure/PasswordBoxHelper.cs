using System.Windows;
using System.Windows.Controls;

namespace GymBro.App.Infrastructure
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static void SetBoundPassword(DependencyObject element, string value) =>
            element.SetValue(BoundPasswordProperty, value);

        public static string GetBoundPassword(DependencyObject element) =>
            (string)element.GetValue(BoundPasswordProperty);

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                // Подписываемся на событие изменения пароля, чтобы обновлять свойство
                passwordBox.PasswordChanged -= PasswordChanged;
                // Не устанавливаем пароль, чтобы не сбрасывать курсор
                // passwordBox.Password = (string)e.NewValue ?? "";
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}
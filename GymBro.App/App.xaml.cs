using GymBro.Business.Infrastructure;
using GymBro.App.Views;
using System;
using System.Windows;

namespace GymBro.App
{
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Не даём приложению завершиться при закрытии окна логина,
            // иначе главное окно может не успеть открыться.
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Инициализация базы данных
            try
            {
                var factory = new ManagersFactory();
                factory.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}\n\nInner: {ex.InnerException?.Message}");
            }

            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                try
                {
                    var mainWindow = new MainWindow();
                    MainWindow = mainWindow;
                    ShutdownMode = ShutdownMode.OnMainWindowClose;
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании главного окна: {ex.Message}\n\n{ex.StackTrace}");
                    Shutdown();
                }
            }
            else
            {
                Shutdown();
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Необработанное исключение в UI: {e.Exception.Message}\n\n{e.Exception.StackTrace}");
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show($"Критическая ошибка: {ex?.Message}\n\n{ex?.StackTrace}");
        }
    }
}

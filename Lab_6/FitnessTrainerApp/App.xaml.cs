using System.Windows;
using FitnessTrainerApp.Views;

namespace FitnessTrainerApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Створюємо і відкриваємо вікно входу
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
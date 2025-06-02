using FitnessTrainerApp.Data;
using FitnessTrainerApp.Models;
using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace FitnessTrainerApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            using var conn = Database.GetConnection();
            string query = "SELECT id FROM users WHERE username=@username AND password=@password";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int userId = reader.GetInt32("id");
                reader.Close();

                // Відкрити головне вікно (занять)
                var workoutWindow = new WorkoutWindow(userId);
                workoutWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильний логін або пароль.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
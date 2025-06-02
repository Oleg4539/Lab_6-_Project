using FitnessTrainerApp.Data;
using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace FitnessTrainerApp.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заповніть усі поля!");
                return;
            }

            using var conn = Database.GetConnection();
            string checkQuery = "SELECT COUNT(*) FROM users WHERE username=@username";
            var checkCmd = new MySqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@username", username);
            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists > 0)
            {
                MessageBox.Show("Такий користувач уже існує.");
                return;
            }

            string insertQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";
            var insertCmd = new MySqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@username", username);
            insertCmd.Parameters.AddWithValue("@password", password); // У реальному додатку - хешуй!

            if (insertCmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Реєстрація успішна!");
                this.Close(); // Закриваємо реєстраційне вікно
            }
            else
            {
                MessageBox.Show("Помилка при реєстрації.");
            }
        }
    }
}

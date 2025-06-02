using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace FitnessTrainerApp.Data
{
    public class Database
    {
        private const string connectionString = "server=localhost;user=root;database=fitnesstrainerapp;password=;";
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка з'єднання з базою даних: " + ex.Message);
            }
            return conn;
        }
    }
}

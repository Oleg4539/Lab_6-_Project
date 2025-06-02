using FitnessTrainerApp.Data;
using FitnessTrainerApp.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FitnessTrainerApp.Views
{
    public partial class WorkoutWindow : Window
    {
        private readonly int userId;
        private List<Workout> workouts = new();

        public WorkoutWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            DayComboBox.ItemsSource = new[] { "Понеділок", "Вівторок", "Середа", "Четвер", "П’ятниця", "Субота", "Неділя" };
            TimeComboBox.ItemsSource = new[] { "08:00", "09:00", "10:00", "11:00", "12:00", "14:00", "16:00", "18:00" };
            TypeComboBox.ItemsSource = new[] { "Присідання", "Біг", "Жим лежачи", "Плавання", "Велотренажер" };

            LoadWorkouts();
        }

        private void LoadWorkouts()
        {
            workouts.Clear();
            WorkoutListView.Items.Clear();

            using var conn = Database.GetConnection();
            string query = "SELECT * FROM workouts WHERE user_id=@userId";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var workout = new Workout
                {
                    Id = reader.GetInt32("id"),
                    UserId = userId,
                    DayOfWeek = reader.GetString("day_of_week"),
                    Time = reader.GetString("time"),
                    Type = reader.GetString("type")
                };
                workouts.Add(workout);
            }

            foreach (var w in workouts)
                WorkoutListView.Items.Add(w);
        }

        private void AddWorkout_Click(object sender, RoutedEventArgs e)
        {
            if (DayComboBox.SelectedItem is null || TimeComboBox.SelectedItem is null || TypeComboBox.SelectedItem is null)
            {
                MessageBox.Show("Заповніть усі поля.");
                return;
            }

            using var conn = Database.GetConnection();
            string query = "INSERT INTO workouts (user_id, day_of_week, time, type) VALUES (@userId, @day, @time, @type)";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@day", DayComboBox.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@time", TimeComboBox.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@type", TypeComboBox.SelectedItem.ToString());

            cmd.ExecuteNonQuery();
            LoadWorkouts();
        }

        private void EditWorkout_Click(object sender, RoutedEventArgs e)
        {
            if (WorkoutListView.SelectedItem is not Workout selected) return;

            using var conn = Database.GetConnection();
            string query = "UPDATE workouts SET day_of_week=@day, time=@time, type=@type WHERE id=@id";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@day", DayComboBox.SelectedItem?.ToString() ?? selected.DayOfWeek);
            cmd.Parameters.AddWithValue("@time", TimeComboBox.SelectedItem?.ToString() ?? selected.Time);
            cmd.Parameters.AddWithValue("@type", TypeComboBox.SelectedItem?.ToString() ?? selected.Type);
            cmd.Parameters.AddWithValue("@id", selected.Id);

            cmd.ExecuteNonQuery();
            LoadWorkouts();
        }

        private void DeleteWorkout_Click(object sender, RoutedEventArgs e)
        {
            if (WorkoutListView.SelectedItem is not Workout selected) return;

            using var conn = Database.GetConnection();
            string query = "DELETE FROM workouts WHERE id=@id";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", selected.Id);

            cmd.ExecuteNonQuery();
            LoadWorkouts();
        }
    }
}

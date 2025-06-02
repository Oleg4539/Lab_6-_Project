namespace FitnessTrainerApp.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DayOfWeek { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}
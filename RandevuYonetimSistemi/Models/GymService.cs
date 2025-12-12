namespace RandevuYonetimSistemi.Models
{
    public class GymService
    {
        //ID UU9ID olur
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public int FitnessCenterId { get; set; }
        public FitnessCenter fitnessCenter { get; set; } = null!;
        public ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
   
    }
}

namespace RandevuYonetimSistemi.Models
{
    public class FitnessCenter
    {
        //Spor Salonlarının tabloları için gerekli şeyler oluşturulmuştur.Bazı değerlerin boş geçilemeyeceği özellikle belirtilmiştir.
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Adress { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string WorkingHours { get; set; } = null!; // Örn: "08:00-23:00" şeklinde tanımlanabilir.
        public ICollection<GymService> Services { get; set; } = new List<GymService>();
        //Spor salonundaki yapılan işler bir collection sınıfında tutulmaktadır.
        public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
        //Spor salonu eğitimcileri de bir collection sınıfında tutulmaktadır.
    }
}

namespace RandevuYonetimSistemi.Models
{
    public class Member
    {
        public int Id { get; set; }
        //null! ifadeleri ile mevcut yerim boş bırakıpmayacağı anlatılmıştır.
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        // Body info (AI önerileri için)
        public double? HeightCm { get; set; }
        public double? WeightKg { get; set; }
        public string? BodyType { get; set; } // ektomorf, mezomorf vb. (opsiyonel)

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}

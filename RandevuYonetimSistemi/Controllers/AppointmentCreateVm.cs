using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Models.ViewModels
{
    public class AppointmentCreateVm
    {
        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Tarih seçiniz.")]
        public DateOnly? Date { get; set; }

        [Required(ErrorMessage = "Saat seçiniz.")]
        public TimeOnly? Time { get; set; }
    }
}

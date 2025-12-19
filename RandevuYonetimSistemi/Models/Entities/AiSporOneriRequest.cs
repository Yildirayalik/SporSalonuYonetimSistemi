namespace FitnessWebSitesi.Models
{
    public class AiSporOneriRequest
    {
        public string Hedef { get; set; }       // kilo verme, kas yapma vb
        public string Seviye { get; set; }      // başlangıç / orta / ileri
        public int GunSayisi { get; set; }       // haftada kaç gün
    }
}
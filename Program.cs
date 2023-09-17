using System;

namespace MesaiUygulamasi
{
    public class Personel
    {
        public TimeSpan GirisSaati { get; set; }
        public TimeSpan CikisSaati { get; set; }

        public TimeSpan HesaplaCalismaSuresi()
        {
            return CikisSaati - GirisSaati;
        }
    }

    public class Mesai
    {
        private const double SaatlikMesaiUcreti = 50;
        private static readonly TimeSpan MesaiBaslangic = new TimeSpan(9, 0, 0);
        private static readonly TimeSpan MesaiBitis = new TimeSpan(18, 0, 0);
        private static readonly TimeSpan Mola = new TimeSpan(1, 0, 0);

        public static double HesaplaMesaiUcreti(TimeSpan calismaSuresi)
        {
            if (calismaSuresi > (MesaiBitis - MesaiBaslangic))
            {
                TimeSpan mesaiSuresi = calismaSuresi - (MesaiBitis - MesaiBaslangic) - Mola;
                return mesaiSuresi.TotalHours * SaatlikMesaiUcreti;
            }
            return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TimeZoneInfo germanyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime germanyNow = TimeZoneInfo.ConvertTime(DateTime.Now, germanyTimeZone);

            Console.WriteLine($"Almanya Saati: {germanyNow.TimeOfDay}");

            Personel personel = new Personel
            {
                GirisSaati = SorSaat("Personelin giriş saatini HH:mm formatında giriniz:"),
                CikisSaati = SorSaat("Personelin çıkış saatini HH:mm formatında giriniz:")
            };

            TimeSpan calismaSuresi = personel.HesaplaCalismaSuresi() - Mesai.Mola;
            double mesaiUcreti = Mesai.HesaplaMesaiUcreti(calismaSuresi);

            Console.WriteLine($"Personel {calismaSuresi.TotalHours:0.##} saat çalıştı ve {mesaiUcreti:0.##} TL mesai ücreti aldı.");
        }

        static TimeSpan SorSaat(string mesaj)
        {
            TimeSpan saat;
            Console.WriteLine(mesaj);
            while (!TimeSpan.TryParse(Console.ReadLine(), out saat))
            {
                Console.WriteLine("Hatalı format! Lütfen HH:mm formatında giriniz:");
            }
            return saat;
        }
    }
}

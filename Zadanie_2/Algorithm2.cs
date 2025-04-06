using System.Numerics;
using System.Security.Cryptography;

namespace Zadanie_2
{
    public class Algorithm2
    {
        public Dictionary<int, List<Osobnik>> HistoriaPopulacji = new Dictionary<int, List<Osobnik>>();
        const int rozmiar_populacji = 13;
        const int rozmiarTurnieju = 3;

        const int liczbaGeneracji = 100;
        const int LBnP = 4; // Liczba bitów na parametr
        const int LBnOs = 12; // Liczba wszystkich bitów(chromosomów) 
        const double ZDMin = 0;
        const double ZDMax = 3;

        public Dictionary<int, List<Osobnik>> Start()
        {
            var populacja = StworzPopulacje(rozmiar_populacji);
            for (int i = 0; i < liczbaGeneracji; i++)
            {
                List<Osobnik> nowaPopulacja = new List<Osobnik>();
                //Tworzenie nowej populacji 
                for (int j = 0; j < rozmiar_populacji - 1; j++)
                {
                    Osobnik zwyciezca = Turniej(populacja, rozmiarTurnieju);
                    nowaPopulacja.Add(zwyciezca);

                }


                (var dziecko1, var dziecko2) = Osobnik.Krzyzowanie(nowaPopulacja[0], nowaPopulacja[1]);
                nowaPopulacja[0] = dziecko1;
                nowaPopulacja[1] = dziecko2;//Krzyżowanie osobnik 1 z 2 


                (var dziecko3, var dziecko4) = Osobnik.Krzyzowanie(nowaPopulacja[2], nowaPopulacja[3]);
                nowaPopulacja[2] = dziecko3;
                nowaPopulacja[3] = dziecko4; //Krzyżowanie osobnik 3 z 4 

                (var dziecko9, var dziecko10) = Osobnik.Krzyzowanie(nowaPopulacja[8], nowaPopulacja[9]);
                nowaPopulacja[8] = dziecko9;
                nowaPopulacja[9] = dziecko10; //Krzyżowanie osobnik 9 z 10

                (var dziecko12, var dziecko13) = Osobnik.Krzyzowanie(nowaPopulacja[10], nowaPopulacja[11]);
                nowaPopulacja[10] = dziecko12;
                nowaPopulacja[11] = dziecko13; //Krzyżowanie osobnik 12 z 13



                nowaPopulacja[4] = nowaPopulacja[4].Mutacja();
                nowaPopulacja[5] = nowaPopulacja[5].Mutacja();
                nowaPopulacja[6] = nowaPopulacja[6].Mutacja();
                nowaPopulacja[7] = nowaPopulacja[7].Mutacja();
                nowaPopulacja[8] = nowaPopulacja[8].Mutacja();
                nowaPopulacja[9] = nowaPopulacja[9].Mutacja();
                nowaPopulacja[10] = nowaPopulacja[10].Mutacja();
                nowaPopulacja[11] = nowaPopulacja[11].Mutacja();


                //Zachowanie najlepszego osobnika (hot deck)
                Osobnik najlepszy = populacja.OrderByDescending(o => o.Dopasowanie).First();
                nowaPopulacja.Add(najlepszy);



                var najlepszeDopasowanie = nowaPopulacja.OrderByDescending(o => o.Dopasowanie).First();
                double srednieDopasowanie = nowaPopulacja.Average(o => o.Dopasowanie);
                Console.WriteLine($"Generacja {i + 1}: Najlepsze = {najlepszeDopasowanie.Dopasowanie}, Średnie = {srednieDopasowanie}");
                Console.WriteLine($"Pa: {najlepszeDopasowanie.Pa}, Pb: {najlepszeDopasowanie.Pb},Pc: {najlepszeDopasowanie.Pc}");
                populacja = nowaPopulacja;
                nowaPopulacja.Add(najlepszy);
                HistoriaPopulacji.Add(i, nowaPopulacja);
            }
            return HistoriaPopulacji;

        }

        private List<Osobnik> StworzPopulacje(int wielkoscPopulacji)
        {
            Random rnd = new Random();

            var populacja = new List<Osobnik>(wielkoscPopulacji);
            for (int j = 0; j < wielkoscPopulacji; j++)
            {
                ushort chromosomy = (ushort)rnd.Next(0, 4096); // na 12bitach maksymalna liczba to 4096
                populacja.Add(new Osobnik(chromosomy, LBnP, LBnOs, ZDMin, ZDMax));
            }

            return populacja;
        }

        private Osobnik Turniej(List<Osobnik> populacja, int rozmiarTurnieju)
        {
            Random rnd = new Random();

            var turniej = new List<Osobnik>();
            var indexy = new HashSet<int>();
            while (indexy.Count < rozmiarTurnieju)
            {
                int index;
                do
                {
                    index = rnd.Next(populacja.Count);
                } while (indexy.Contains(index));
                indexy.Add(index);
                turniej.Add(populacja[index]);
            }
            return turniej.OrderByDescending(osobnik => osobnik.Dopasowanie).First();
        }
    }

    public class Osobnik
    {
        static Random rnd = new Random();

        public double Pa;
        public double Pb;
        public double Pc;

        public double Dopasowanie;
        public ushort Chromosomy { get; }
        public int LBnP { get; }
        public int LBnOs { get; }
        public double ZDMin { get; }
        public double ZDMax { get; }
        public double ZD => ZDMax - ZDMin;

        public Osobnik(ushort chromosomy, int LBnP, int LBnOs, double ZDMin, double ZDMax)
        {
            Chromosomy = chromosomy;
            this.LBnP = LBnP;
            this.LBnOs = LBnOs;
            this.ZDMin = ZDMin;
            this.ZDMax = ZDMax;


            Pa = Dekodowanie((chromosomy & 0b111100000000) >> 8);  // Bity 11-8
            Pb = Dekodowanie((chromosomy & 0b000011110000) >> 4);  // Bity 7-4
            Pc = Dekodowanie(chromosomy & 0b000000001111);         // Bity 3-0
            Dopasowanie = ObliczDopasowanie();
        }
        public Osobnik()
        {

        }

        double Dekodowanie(int chromosomy)
        {
            if (chromosomy == 0)
            {
                return 0; // Wartość minimalna
            }

            double ctmp = 0;
            for (int i = 0; i < LBnP; i++)
            {
                int maska = 1 << i; // Maska do wyodrębnienia bitu
                if ((chromosomy & maska) != 0)
                {
                    ctmp += Math.Pow(2, i);
                }
            }

            double parametr = ZDMin + (ctmp / (Math.Pow(2, LBnP) - 1)) * ZD;
            return parametr;
        }

        private double ObliczDopasowanie()
        {
            double przystosowanie(double x)
            {
                return Pa * Math.Sin(Pb * x + Pc);
            }

            var probki = BazaDanych.Probki;
            double suma = 0;
            foreach (List<double> pomiar in probki)
            {
                var x = pomiar[0];
                var y = pomiar[1];
                suma += Math.Pow(y - przystosowanie(x), 2);
            }
            return suma;

        }

        public Osobnik Mutacja()
        {
            int b_punkt = rnd.Next(LBnOs);
            int maska = 1 << b_punkt;
            ushort mutacja = (ushort)(Chromosomy ^ maska);

            return new Osobnik(mutacja, LBnP, LBnOs, ZDMin, ZDMax);
        }
        public static (Osobnik, Osobnik) Krzyzowanie(Osobnik rodzic1, Osobnik rodzic2, int LBnOs)
        {

            int punktCiecia = rnd.Next(0, LBnOs - 2);
            ushort maska_dol = (ushort)((1 << punktCiecia) - 1);
            ushort maska_gora = (ushort)~maska_dol;

            ushort chromosom1 = (ushort)((rodzic1.Chromosomy & maska_gora) | (rodzic2.Chromosomy & maska_dol));
            ushort chromosom2 = (ushort)((rodzic2.Chromosomy & maska_gora) | (rodzic1.Chromosomy & maska_dol));

            Osobnik dziecko1 = new Osobnik();
            Osobnik dziecko2 = new Osobnik();


            return (dziecko1,dziecko2);
        }
    }
}
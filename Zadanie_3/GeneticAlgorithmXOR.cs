using Zadanie_3;

namespace Zadanie_2
{
    public class GeneticAlgorithmXOR
    {
        public Dictionary<int, List<OsobnikXOR>> HistoriaPopulacji = new Dictionary<int, List<OsobnikXOR>>();
        const int rozmiar_populacji = 13;
        const int rozmiarTurnieju = 3;

        const int liczbaGeneracji = 1_000;
        const int LBnP = 4; // Liczba bitów na parametr
        const int LBnOs = 12; // Liczba wszystkich bitów(chromosomów) 
        const double ZDMin = -10;
        const double ZDMax = 10;

        public Dictionary<int, List<OsobnikXOR>> Start()
        {
            var populacja = StworzPopulacje(rozmiar_populacji);
            for (int i = 0; i < liczbaGeneracji; i++)
            {
                List<OsobnikXOR> nowaPopulacja = new List<OsobnikXOR>();
                //Tworzenie nowej populacji 
                for (int j = 0; j < rozmiar_populacji - 1; j++)
                {
                    OsobnikXOR zwyciezca = Turniej(populacja, rozmiarTurnieju);
                    nowaPopulacja.Add(zwyciezca);
                }

                (var dziecko1, var dziecko2) = OsobnikXOR.Krzyzowanie(
                    nowaPopulacja[0],
                    nowaPopulacja[1]);

                nowaPopulacja[0] = dziecko1;
                nowaPopulacja[1] = dziecko2;//Krzyżowanie osobnik 1 z 2 

                (var dziecko3, var dziecko4) = OsobnikXOR.Krzyzowanie(
                    nowaPopulacja[2],
                    nowaPopulacja[3]);

                nowaPopulacja[2] = dziecko3;
                nowaPopulacja[3] = dziecko4; //Krzyżowanie osobnik 3 z 4 

                (var dziecko9, var dziecko10) = OsobnikXOR.Krzyzowanie(
                    nowaPopulacja[8],
                    nowaPopulacja[9]);

                nowaPopulacja[8] = dziecko9;
                nowaPopulacja[9] = dziecko10; //Krzyżowanie osobnik 9 z 10

                (var dziecko12, var dziecko13) = OsobnikXOR.Krzyzowanie(
                    nowaPopulacja[10],
                    nowaPopulacja[11]);

                nowaPopulacja[10] = dziecko12;
                nowaPopulacja[11] = dziecko13; //Krzyżowanie osobnik 12 z 13

                for (int l = 4; l < nowaPopulacja.Count; l++)
                {
                    nowaPopulacja[l] = nowaPopulacja[l].Mutacja();
                }

                //Zachowanie najlepszego osobnika (hot deck)
                OsobnikXOR najlepszy = populacja.OrderBy(o => o.Dopasowanie).First();
                nowaPopulacja.Add(najlepszy);
                populacja = nowaPopulacja;
                HistoriaPopulacji.Add(i, nowaPopulacja);
            }

            return HistoriaPopulacji;
        }

        private List<OsobnikXOR> StworzPopulacje(int wielkoscPopulacji)
        {
            Random rnd = new Random();

            var populacja = new List<OsobnikXOR>(wielkoscPopulacji);
            for (int j = 0; j < wielkoscPopulacji; j++)
            {
                var buffer = new byte[36];
                rnd.NextBytes(buffer);
                long chromosomy = BitConverter.ToInt64(buffer);
                populacja.Add(new OsobnikXOR(chromosomy, LBnP, LBnOs, ZDMin, ZDMax));
            }

            return populacja;
        }

        private OsobnikXOR Turniej(List<OsobnikXOR> populacja, int rozmiarTurnieju)
        {
            Random rnd = new Random();

            var turniej = new List<OsobnikXOR>();
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
            return turniej.OrderBy(osobnik => osobnik.Dopasowanie).First();
        }
    }

    public class OsobnikXOR
    {
        static Random rnd = new Random();

        public double Dopasowanie;
        public double[][] wagi;

        public long Chromosomy { get; }
        public int LBnP { get; }
        public int LBnOs { get; }
        public double ZDMin { get; }
        public double ZDMax { get; }
        public double ZD => ZDMax - ZDMin;

        public OsobnikXOR(long chromosomy, int LBnP, int LBnOs, double ZDMin, double ZDMax)
        {
            Chromosomy = chromosomy;
            this.LBnP = LBnP;
            this.LBnOs = LBnOs;
            this.ZDMin = ZDMin;
            this.ZDMax = ZDMax;

            var x1 = Dekodowanie((int)((chromosomy & 0xF00000000) >> 32));  // Bits 35-32 (4 bits)
            var x2 = Dekodowanie((int)((chromosomy & 0x0F0000000) >> 28));  // Bits 31-28
            var x3 = Dekodowanie((int)((chromosomy & 0x00F000000) >> 24));  // Bits 27-24
            var x4 = Dekodowanie((int)((chromosomy & 0x000F00000) >> 20));  // Bits 23-20
            var x5 = Dekodowanie((int)((chromosomy & 0x0000F0000) >> 16));  // Bits 19-16
            var x6 = Dekodowanie((int)((chromosomy & 0x00000F000) >> 12));  // Bits 15-12
            var x7 = Dekodowanie((int)((chromosomy & 0x000000F00) >> 8));   // Bits 11-8
            var x8 = Dekodowanie((int)((chromosomy & 0x0000000F0) >> 4));   // Bits 7-4
            var x9 = Dekodowanie((int)(chromosomy & 0x00000000F));          // Bits 3-0

            var wagi_1 = new double[] { x1, x2, x3 };
            var wagi_2 = new double[] { x4, x5, x6 };
            var wagi_3 = new double[] { x7, x8, x9 };

            wagi = new double[][] { wagi_1, wagi_2, wagi_3 };

            Dopasowanie = ObliczDopasowanie();
        }

        double Dekodowanie(int chromosomy)
        {
            if (chromosomy == 0)
            {
                return 0; 
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
            double przystosowanie(double[] wejscia, double[][] wagi)
            {
                var siecNeuronowa = new SiecNeuronowa(wejscia, wagi);
                return siecNeuronowa.Start();
            }

            var bledyPomiarowe = new List<(double y, double fy, double bladPomiaru)>();
            var probki = BazaDanych.probki;

            double suma = 0;

            int i = 0;
            foreach (double[] probka in probki)
            {
                var we1 = probka[0];
                var we2 = probka[1];
                var fy = przystosowanie(new double[] { we1, we2 }, wagi);

                var roznicaPomiaru = Math.Pow(we2 - fy, 2);
                bledyPomiarowe.Add((we2, fy, roznicaPomiaru));

                suma += roznicaPomiaru;

                i++;
            }

            return suma;
        }

        public OsobnikXOR Mutacja()
        {
            int b_punkt = rnd.Next(LBnOs);
            int maska = 1 << b_punkt;
            long mutacja = (long)(Chromosomy ^ maska);

            return new OsobnikXOR(mutacja, LBnP, LBnOs, ZDMin, ZDMax);
        }

        public static (OsobnikXOR, OsobnikXOR) Krzyzowanie(
            OsobnikXOR rodzic1,
            OsobnikXOR rodzic2)
        {
            int LBnOs = rodzic1.LBnOs;
            int punktCiecia = rnd.Next(0, LBnOs - 2);
            ushort maska_dol = (ushort)((1 << punktCiecia) - 1);
            ushort maska_gora = (ushort)~maska_dol;

            ushort chromosom1 = (ushort)((rodzic1.Chromosomy & maska_gora) | (rodzic2.Chromosomy & maska_dol));
            ushort chromosom2 = (ushort)((rodzic2.Chromosomy & maska_gora) | (rodzic1.Chromosomy & maska_dol));

            OsobnikXOR dziecko1 = new OsobnikXOR(chromosom1, rodzic1.LBnP, rodzic1.LBnOs, rodzic1.ZDMin, rodzic1.ZDMax);
            OsobnikXOR dziecko2 = new OsobnikXOR(chromosom2, rodzic2.LBnP, rodzic2.LBnOs, rodzic2.ZDMin, rodzic2.ZDMax);


            return (dziecko1, dziecko2);
        }
    }
}
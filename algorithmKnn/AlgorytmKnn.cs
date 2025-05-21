using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace algorithmKnn
{
    internal class AlgorytmKnn
    {

        double[][] dane;
        private int[] klasy;

        int K = 3;
        public enum Metryka
        {
            Euklidesowa,
            Manhattan
        }

        public AlgorytmKnn(double[][] daneWejsciowe)
        {
            // Rozdzielenie danych na atrybuty i klasy
            dane = daneWejsciowe.Select(row => row.Take(row.Length - 1).ToArray()).ToArray();
            klasy = daneWejsciowe.Select(row => (int)row.Last()).ToArray();
            // Normalizacja danych przy inicjalizacji
            NormalizujDane();
        }

        private void NormalizujDane()
        {
            int numerAtrybutu = dane[0].Length - 1;

            for (int attr = 0; attr < numerAtrybutu; attr++)
            {
                // Szukanie min max 
                double min = dane.Min(dane => dane[attr]);
                double max = dane.Max(dane => dane[attr]);

                // Noralizacja
                for (int i = 0; i < dane.Length; i++)
                {
                    dane[i][attr] = (dane[i][attr] - min) / (max - min);
                }
            }
        }

        static int KlasyfikacjaKNN(double[][] daneTreningowe, double[] probkaTestowa, int k, Metryka metryka)
        {
            //obliczanie odległości
            var odleglosci = new (double dystans, int klasa)[daneTreningowe.Length];

            for (int i = 0; i < daneTreningowe.Length; i++)
            {
                double odleglosc = Wyliczdystans(
                    probkaTestowa,
                    daneTreningowe[i].Take(daneTreningowe[i].Length - 1).ToArray(), // pomijamy klasę
                    metryka);

                odleglosci[i] = (odleglosc, (int)daneTreningowe[i].Last());
            }

            //Najbliżsi sąsiedzi
            var najblizsiSasiedzi = odleglosci
                .OrderBy(d => d.dystans)
                .Take(k)
                .ToArray();

            // najczęściej występująca klasa
            var wynikiGlosowania = najblizsiSasiedzi
                .GroupBy(n => n.klasa)
                .Select(g => new { Klasa = g.Key, Liczba = g.Count() })
                .OrderByDescending(g => g.Liczba)
                .ToList();

            // rozstrzyganie remisu
            if (wynikiGlosowania.Count > 1 && wynikiGlosowania[0].Liczba == wynikiGlosowania[1].Liczba)
            {
                return wynikiGlosowania[0].Klasa;
            }

            return wynikiGlosowania[0].Klasa;
        }
        public int Klasyfikuj(double[] probka, Metryka metryka )
        {
            return KlasyfikacjaKNN(
                dane.Zip(klasy, (d, k) => d.Append((double)k).ToArray()).ToArray(),
                probka,
                K,
                metryka
            );
        }

        static double Wyliczdystans(double[] v1, double[] v2, Metryka metryka)
        {
            switch (metryka)
            {
                case Metryka.Euklidesowa:
                    return MetrykaClass.MetrykaEuklidesowa(v1, v2);
                case Metryka.Manhattan:
                    return MetrykaClass.MetrykaManhattan(v1, v2);
                default:
                    throw new ArgumentException("Nieznana metryka");
            }
        }


    }
}





namespace algorithmKnn
{
    internal class AlgorytmKnn
    {
        double[][] dane;
        private int[] klasy;
        private double[] minWartosci; 
        private double[] maxWartosci;

        public enum Metryka
        {
            Euklidesowa,
            Manhattan,
            Chebysheva
        }

        public AlgorytmKnn(double[][] daneWejsciowe)
        {
            // Walidacja danych
            if (daneWejsciowe == null || daneWejsciowe.Length == 0)
                throw new ArgumentException("Dane wejściowe nie mogą być puste");

            // Rozdzielenie danych na atrybuty i klasy
            dane = daneWejsciowe.Select(row => row.Take(row.Length - 1).ToArray()).ToArray();
            klasy = daneWejsciowe.Select(row => (int)row.Last()).ToArray();

            // Inicjalizacja tablic dla min/max
            int liczbaAtrybutow = dane[0].Length;
            minWartosci = new double[liczbaAtrybutow];
            maxWartosci = new double[liczbaAtrybutow];

            NormalizujDane();
        }

        private void NormalizujDane()
        {
            int numerAtrybutu = dane[0].Length;

            for (int attr = 0; attr < numerAtrybutu; attr++)
            {
                // Szukanie min max 
                double min = dane.Min(row => row[attr]);
                double max = dane.Max(row => row[attr]);

               
                minWartosci[attr] = min;
                maxWartosci[attr] = max;

                // Normalizacja
                for (int i = 0; i < dane.Length; i++)
                {
                    if (max == min) 
                        dane[i][attr] = 0;
                    else
                        dane[i][attr] = (dane[i][attr] - min) / (max - min);
                }
            }
        }

        private double[] NormalizujProbke(double[] probka)
        {
            double[] znormalizowana = new double[probka.Length];

            for (int attr = 0; attr < probka.Length; attr++)
            {
                if (maxWartosci[attr] == minWartosci[attr])
                    znormalizowana[attr] = 0;
                else
                    znormalizowana[attr] = (probka[attr] - minWartosci[attr]) / (maxWartosci[attr] - minWartosci[attr]);
            }

            return znormalizowana;
        }

        public int Klasyfikuj(double[] probka, Metryka metryka, int k)
        {
            double[] probkaZnormalizowana = NormalizujProbke(probka);

            // Obliczanie odległości
            var odleglosci = new (double dystans, int klasa)[dane.Length];
            for (int i = 0; i < dane.Length; i++)
            {
                double odleglosc = Wyliczdystans(probkaZnormalizowana, dane[i], metryka);
                odleglosci[i] = (odleglosc, klasy[i]);
            }

            // Najbliżsi sąsiedzi
            var najblizsiSasiedzi = odleglosci
                .OrderBy(d => d.dystans)
                .Take(k)
                .ToArray();

            // Najczęściej występująca klasa
            var wynikiGlosowania = najblizsiSasiedzi
                .GroupBy(n => n.klasa)
                .Select(g => new { Klasa = g.Key, Liczba = g.Count() })
                .OrderByDescending(g => g.Liczba)
                .ToList();

            // Rozstrzyganie remisu
            if (wynikiGlosowania.Count > 1 && wynikiGlosowania[0].Liczba == wynikiGlosowania[1].Liczba)
            {
                return -1; // Obiekt niesklasyfikowany
            }

            return wynikiGlosowania[0].Klasa;
        }

        static double Wyliczdystans(double[] v1, double[] v2, Metryka metryka)
        {
            switch (metryka)
            {
                case Metryka.Euklidesowa:
                    return MetrykaClass.MetrykaEuklidesowa(v1, v2);
                case Metryka.Manhattan:
                    return MetrykaClass.MetrykaManhattan(v1, v2);
                case Metryka.Chebysheva:
                    return MetrykaClass.MetrykaChebysheva(v1, v2);
                default:
                    throw new ArgumentException("Nieznana metryka");
            }
        }
    }
}
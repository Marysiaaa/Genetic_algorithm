
namespace WstecznPropagacja
{
    public class SiecNeuronowa
    {
        private readonly double[] wejscia;
        private readonly double[] wyjscia;

        private readonly double[][] wagi;
        public double beta; //parametr funkcjii aktywacji 
        public double SzybkoscUczenia { get; set; } = 0.1;

        public SiecNeuronowa(double[] wejscia, double[][] wagi, double beta)
        {
            this.wejscia = wejscia;
            this.wagi = wagi;
            this.beta = beta;
        }

        public double Start()
        {
            var neuron1 = new Neuron(wejscia, wagi[0], 1);
            var neuron2 = new Neuron(wejscia, wagi[1], 1);
            var n1 = neuron1.FunkcjaAktywacji();
            var n2 = neuron2.FunkcjaAktywacji();
            var neuron3 = new Neuron(new double[] { n1, n2 }, wagi[2], 1);
            var n3 = neuron3.FunkcjaAktywacji();

            return n3;
        }
        public void Propagacja(double[] wejscia, double[] oczekiwaneWyjscia, double szybkoscUczenia)
            
        {
            wejscia = BazaDanych.wejscia[];
            oczekiwaneWyjscia = BazaDanych.oczekiwaneWyjscia[];

            foreach (var i in wejscia)
            {
                wyjscia[i] = szybkoscUczenia * wejscia[i] * (oczekiwaneWyjscia[i] - wyjscia[i]);
            }
        }

        public void Ucz(double[][]wejscia, double[][] oczekiwaneWyjscia, double szybkoscUczenia) 
        {
            
        }

        public double ObliczBlad(double[][] wejscia, double[] oczekiwaneWyjscia)
        {
            double blad = 0;
            for (int i = 0; i < wejscia.Length; i++)
            {
                double wyjscie = Propagacja(wejscia[i]);
                for (int j = 0; j < oczekiwaneWyjscia.Length; j++)
                {
                    blad += (oczekiwaneWyjscia[i][j] - wyjscie[j]) * (oczekiwaneWyjscia[i][j] - wyjscie[j]);
                }
            }
            return blad;
        }
    }

    public class Neuron
    {
        private readonly double[] wejscia;
        private readonly double[] wagi;
        private readonly double beta;

        public Neuron(double[] wejscia, double[] wagi, double beta)
        {
            this.wejscia = wejscia;
            this.wagi = wagi;
            this.beta = beta;
        }

        public double FunkcjaAktywacji()
        {
            return 1d / (1d + Math.Exp(-beta * SumaAktywacji()));

        }
        public double SumaAktywacji()
        {
            double suma = wagi[0];

            for (int i = 0; i < wejscia.Length; i++)
            {
                suma += wejscia[i] * wagi[i + 1];
            }

            return suma;
        }
    }
}









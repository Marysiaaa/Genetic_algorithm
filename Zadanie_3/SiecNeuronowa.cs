namespace Zadanie_3
{
    public class SiecNeuronowa
    {
        private readonly double[] wejscia;
        private readonly double[][] wagi;
        public double beta; //parametr funkcjii aktywacji 

        public SiecNeuronowa(double[] wejscia, double[][] wagi)
        {
            this.wejscia = wejscia;
            this.wagi = wagi;
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









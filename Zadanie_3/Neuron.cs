namespace Zadanie_3
{
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









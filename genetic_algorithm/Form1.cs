namespace genetic_algorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class GeneticAlgoritm
        {
            static Random random = new Random();
            const int rozmiar_populacji = 100;
            const double Prawdopodobienstwo_mutacji = 0.01;
            const int Liczba_generacji = 20;
            const double Min_X = 0;
            const double Max_X = 100;
            class Osobnik
            {
                public double X1, X2;

                public double Dopasowanie;
                public Osobnik(double x1,double x2)
                {
                    X1 = x1;
                    X2 = x2;
                    Dopasowanie = ObliczDopasowanie(X1, X2);
                }
                public static double ObliczDopasowanie(double x1, double x2)
                {
                    return Math.Sin(x1 * 0.05) + Math.Sin(x2 * 0.05) + 0.4 * Math.Sin(x1 * 0.15) * Math.Sin(x2 * 0.15);

                }
                static Osobnik Krzyzowanie(Osobnik rodzic1, Osobnik rodzic2)
                {
                    double x1 = (rodzic1.X1 + rodzic2.X1) / 2;
                    double x2 = (rodzic1.X2 + rodzic2.X2) / 2;
                    return new Osobnik(x1, x2);
                }

                static Osobnik Mutacja(Osobnik osobnik)
                {
                    double x1 = osobnik.X1 + (random.NextDouble() - 0.5) * 2 * (Max_X - Min_X) * Prawdopodobienstwo_mutacji;
                    double x2 = osobnik.X2 + (random.NextDouble() - 0.5) * 2 * (Max_X - Min_X) * Prawdopodobienstwo_mutacji ;
                    x1 = Math.Max(Min_X, Math.Min(Max_X, x1));
                    x2 = Math.Max(Min_X, Math.Min(Max_X, x2));
                    return new Osobnik(x1, x2);
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
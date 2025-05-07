public class SiecNeuronowa
{
    private Neuron neuron1;
    private Neuron neuron2;
    private Neuron neuron3;

    private double parametrUczenia = 0.3;

    public List<WynikUczenia> wynikiUczenia = new List<WynikUczenia>();

    public void Start()
    {
        var probki = new double[][]
          {
               new double[] { 0, 0, 0 },
               new double[] { 0, 1, 1 },
               new double[] { 1, 0, 1 },
              new double[] { 1, 1, 0 },
          };

        var beta = 1;

        Random rnd = new Random();

        // Inicjalizacja wag losowymi wartościami
        var wagi1 = new double[] { rnd.NextDouble() * 10 - 5, rnd.NextDouble() * 5 - 1, rnd.NextDouble() * 10 - 5 };
        var wagi2 = new double[] { rnd.NextDouble() * 10 - 5, rnd.NextDouble() * 5 - 1, rnd.NextDouble() * 10 - 5 };
        var wagi3 = new double[] { rnd.NextDouble() * 10 - 5, rnd.NextDouble() * 5 - 1, rnd.NextDouble() * 10 - 5 };

        neuron1 = new Neuron(wagi1, beta);
        neuron2 = new Neuron(wagi2, beta);
        neuron3 = new Neuron(wagi3, beta);

        //Trenowanie sieci
        int epoka = 0;
        int maxEpoka = 50000;
        while (epoka < maxEpoka)
        {
            foreach (var probka in probki)
            {
                TrenujSiec(epoka, probka[0], probka[1], probka[2]);
            }
            epoka++;
        }
    }

    private void TrenujSiec(int epoka, double x1, double x2, double oczekiwaneWyjscie)
    {
        //Propagacja wprzód 
        double wyjscie1 = neuron1.FunkcjaAktywacji(new double[] { x1, x2 });
        double wyjscie2 = neuron2.FunkcjaAktywacji(new double[] { x1, x2 });
        double wyjscieSieci = neuron3.FunkcjaAktywacji(new double[] { wyjscie1, wyjscie2 });

        double deltaNeuron3 = parametrUczenia * (oczekiwaneWyjscie - wyjscieSieci);

        double deltaNeuron1 = deltaNeuron3 * neuron3.Wagi[1] * neuron1.PochodnaFunkcjiAktywacji(new double[] { x1, x2 });
        double deltaNeuron2 = deltaNeuron3 * neuron3.Wagi[2] * neuron2.PochodnaFunkcjiAktywacji(new double[] { x1, x2 });

        neuron3.AktualizujWagi(new double[] { wyjscie1, wyjscie2 }, deltaNeuron3);
        neuron1.AktualizujWagi(new double[] { x1, x2 }, deltaNeuron1);
        neuron2.AktualizujWagi(new double[] { x1, x2 }, deltaNeuron2);

        wynikiUczenia.Add(new WynikUczenia()
        {
            x1 = x1,
            x2 = x2,
            Epoka = epoka,
            oczekiwane_wyjscie = oczekiwaneWyjscie,
            blad_wyjscia = oczekiwaneWyjscie - wyjscieSieci,
            wyjscie_sieci = wyjscieSieci
        });
    }
}

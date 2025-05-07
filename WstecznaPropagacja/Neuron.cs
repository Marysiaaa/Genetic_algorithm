public class Neuron
{
    private readonly double[] _wagi;
    private readonly double _beta;

    public double[] Wagi => _wagi;
    public Neuron(double[] wagi, double beta)
    {
        _wagi = wagi;
        _beta = beta;
    }


    public double FunkcjaAktywacji(double[] wejscia)
    {
        var suma = _wagi[0];

        for (int i = 0; i < wejscia.Length; i++)
        {
            suma += wejscia[i] * _wagi[i + 1];
        }
        return 1f / (1f + Math.Exp(-_beta * suma)); //sigmoid - aktywacja neuronu 
    }


    public double PochodnaFunkcjiAktywacji(double[] wejscia)
    {
        double wyjscie = FunkcjaAktywacji(wejscia);
        return _beta * wyjscie * (1 - wyjscie);
    }

    public void AktualizujWagi(double[] wejscia, double delta)
    {
        _wagi[0] += delta * 1;

        for (int i = 0; i < wejscia.Length; i++)
        {
            _wagi[i + 1] += delta * wejscia[i];
        }
    }
}
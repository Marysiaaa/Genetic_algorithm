using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetic_algorithm;

internal class GeneticAlgoritm
{
    static Random random = new Random();

    Dictionary<int, List<Osobnik>> HistoriaPopulacji = new Dictionary<int, List<Osobnik>>();
    const int rozmiar_populacji = 100;
    static int rozmiarTurnieju = 2;

    const double Prawdopodobienstwo_mutacji = 0.01;
    const int liczbaGeneracji = 20;
    const double Min_X = 0;
    const double Max_X = 100;
    const int LBnP = 3; // Liczba bitów na parametr
    const double ZDMin = -1;
    const double ZDMax = 2;

    public void Zadanie_1()
    {
        var populacja = StworzPopulacje(liczbaGeneracji);
    }

    private List<Osobnik> StworzPopulacje(int wielkoscPopulacji)
    {
        Random rnd = new Random();

        var populacja = new List<Osobnik>(wielkoscPopulacji);
        for (int j = 0; j < wielkoscPopulacji; j++)
        {
            byte chromosomy = (byte)rnd.Next(0, 64);
            populacja.Add(new Osobnik(chromosomy, Min_X, Max_X, LBnP, ZDMin, ZDMax)); ;
        }

        return populacja;
    }
}

public class Osobnik
{
    public double X1;
    public double X2;
    public double Dopasowanie;
    const double Prawdopodobienstwo_mutacji = 0.01;

    public double Min_X { get; }
    public double Max_X { get; }
    public int LBnP { get; }
    public double ZDMin { get; }
    public double ZDMax { get; }
    public double ZD => ZDMax - ZDMin;

    public Osobnik(byte chromosomy, double Min_X, double Max_X, int LBnP, double ZDMin, double ZDMax)
    {
        X1 = Dekodowanie((chromosomy & 0b111000) >> 3);
        X2 = Dekodowanie(chromosomy & 0b000111);
        Dopasowanie = ObliczDopasowanie();
        this.Min_X = Min_X;
        this.Max_X = Max_X;
        this.LBnP = LBnP;
        this.ZDMin = ZDMin;
        this.ZDMax = ZDMax;
    }
    public Osobnik(double x1, double x2)
    {
        X1 = x1;
        X2 = x2;
        Dopasowanie = ObliczDopasowanie();
    }
    int Kodowanie(double pm)
    {
        pm = Math.Max(pm, ZDMin);
        pm = Math.Min(pm, ZDMax);
        int ctmp = (int)Math.Round((pm - ZDMin) / ZD * (Math.Pow(2, LBnP) - 1));
        return ctmp;
    }

    double Dekodowanie(int cb)
    {
        double ctmp = cb;
        return ZDMin + (ctmp / (Math.Pow(2, LBnP) - 1)) * ZD;
    }

    public double ObliczDopasowanie()
    {
        return Math.Sin(X1 * 0.05) + Math.Sin(X2 * 0.05) + 0.4 * Math.Sin(X1 * 0.15) * Math.Sin(X2 * 0.15);

    }

    static Osobnik Krzyzowanie(Osobnik rodzic1, Osobnik rodzic2)
    {
        double x1 = (rodzic1.X1 + rodzic2.X1) / 2;
        double x2 = (rodzic1.X2 + rodzic2.X2) / 2;
        return new Osobnik(x1, x2);
    }

    public Osobnik Mutacja(Osobnik osobnik)
    {
        var random = new Random();
        double x1 = osobnik.X1 + (random.NextDouble() - 0.5) * 2 * (Max_X - Min_X) * Prawdopodobienstwo_mutacji;
        double x2 = osobnik.X2 + (random.NextDouble() - 0.5) * 2 * (Max_X - Min_X) * Prawdopodobienstwo_mutacji;
        x1 = Math.Max(Min_X, Math.Min(Max_X, x1));
        x2 = Math.Max(Min_X, Math.Min(Max_X, x2));
        return new Osobnik(x1, x2);
    }
}
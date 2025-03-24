using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetic_algorithm;

internal class GeneticAlgoritm
{
    public Dictionary<int, List<Osobnik>> HistoriaPopulacji = new Dictionary<int, List<Osobnik>>();
    const int rozmiar_populacji = 9;
    const int rozmiarTurnieju = 2;

    const int liczbaGeneracji = 20;
    const int LBnP = 3; // Liczba bitów na parametr
    const int LBnOs = 6; // Liczba wszystkich bitów
    const double ZDMin = 0;
    const double ZDMax = 100;

    public Dictionary<int, List<Osobnik>>  Start()
    {
        var populacja = StworzPopulacje(rozmiar_populacji);
        for (int i = 0; i < liczbaGeneracji; i++)
        {
            List<Osobnik> nowaPopulacja = new List<Osobnik>();
            //Tworzenie nowej populacji 
            for (int j = 0; j < rozmiar_populacji - 1; j++)
            {
                Osobnik zwyciezca = Turniej(populacja, rozmiarTurnieju);
                Osobnik mutant = zwyciezca.Mutacja();
                nowaPopulacja.Add(mutant);
            }

            //Zachowanie najlepszego osobnika (hot deck)
            Osobnik najlepszy = populacja.OrderByDescending(o => o.Dopasowanie).First();
            nowaPopulacja.Add(najlepszy);
            var najlepszeDopasowanie = nowaPopulacja.OrderByDescending(o => o.Dopasowanie).First();
            double srednieDopasowanie = nowaPopulacja.Average(o => o.Dopasowanie);
            Console.WriteLine($"Generacja {i + 1}: Najlepsze = {najlepszeDopasowanie.Dopasowanie}, Średnie = {srednieDopasowanie}");
            Console.WriteLine($"X1: {najlepszeDopasowanie.X1}, X2: {najlepszeDopasowanie.X2}");
            populacja = nowaPopulacja;
            nowaPopulacja.Add(najlepszy);
            HistoriaPopulacji.Add(i, nowaPopulacja);
        }
        return HistoriaPopulacji;
       
    }

    private List<Osobnik> StworzPopulacje(int wielkoscPopulacji)
    {
        Random rnd = new Random();

        var populacja = new List<Osobnik>(wielkoscPopulacji);
        for (int j = 0; j < wielkoscPopulacji; j++)
        {
            byte chromosomy = (byte)rnd.Next(0, 64); // na 6 bitach maksymalna liczba to 64
            populacja.Add(new Osobnik(chromosomy, LBnP, LBnOs, ZDMin, ZDMax));
        }

        return populacja;
    }

    private Osobnik Turniej(List<Osobnik> populacja, int rozmiarTurnieju)
    {
        Random rnd = new Random();

        var turniej = new List<Osobnik>();
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
        return turniej.OrderByDescending(osobnik => osobnik.Dopasowanie).First();
    }
}

public class Osobnik
{
    static Random rnd = new Random();

    public double X1;
    public double X2;
    public double Dopasowanie;

    public byte Chromosomy { get; }
    public int LBnP { get; }
    public int LBnOs { get; }
    public double ZDMin { get; }
    public double ZDMax { get; }
    public double ZD => ZDMax - ZDMin;

    public Osobnik(byte chromosomy, int LBnP, int LBnOs, double ZDMin, double ZDMax)
    {
        Chromosomy = chromosomy;
        this.LBnP = LBnP;
        this.LBnOs = LBnOs;
        this.ZDMin = ZDMin;
        this.ZDMax = ZDMax;
        X1 = Dekodowanie((chromosomy & 0b111000) >> 3);
        X2 = Dekodowanie(chromosomy & 0b000111);
        Dopasowanie = ObliczDopasowanie();
    }

    double Dekodowanie(int chromosomy)
    {
        if (chromosomy == 0)
        {
            return 0; // Wartość minimalna
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
        return Math.Sin(X1 * 0.05) + Math.Sin(X2 * 0.05) + 0.4 * Math.Sin(X1 * 0.15) * Math.Sin(X2 * 0.15);
    }

    public Osobnik Mutacja()
    {
        int b_punkt = rnd.Next(LBnOs);
        int maska = 1 << b_punkt;
        byte mutacja = (byte)(Chromosomy ^ maska);

        return new Osobnik(mutacja, LBnP, LBnOs, ZDMin, ZDMax);
    }
}
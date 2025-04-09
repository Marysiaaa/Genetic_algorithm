using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Zadanie_2;

namespace genetic_algorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Generacja",
                HeaderText = "Generacja",
                Width = 80
            });


            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "X1",
                HeaderText = "X1",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "X2",
                HeaderText = "X2",
                Width = 150
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Dopasowanie",
                HeaderText = "Dopasowanie",
                Width = 50
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Average",
                HeaderText = "Average",
                Width = 60
            });
        }
        private void InitializeDataGridView2()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Generacja",
                HeaderText = "Generacja",
                Width = 80
            });


            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Pa",
                HeaderText = "Pa",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Pb",
                HeaderText = "Pb",
                Width = 150
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Pc",
                HeaderText = "Pc",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Dopasowanie",
                HeaderText = "Dopasowanie",
                Width = 50
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Average",
                HeaderText = "Average",
                Width = 60
            });
        }

        private void InitializeDataGridView3()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Generacja",
                HeaderText = "Generacja",
                Width = 80
            });


            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x1",
                HeaderText = "x1",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x2",
                HeaderText = "x2",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x3",
                HeaderText = "x3",
                Width = 150
            }); dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x4",
                HeaderText = "x4",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x5",
                HeaderText = "x5",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x6",
                HeaderText = "x6",
                Width = 150
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x7",
                HeaderText = "x7",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x8",
                HeaderText = "x8",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "x9",
                HeaderText = "x9",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Dopasowanie",
                HeaderText = "Dopasowanie",
                Width = 50
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Average",
                HeaderText = "Average",
                Width = 60
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var algorytm = new GeneticAlgoritm();

            var wynik = algorytm.Start();

            var lista = new List<Model>();
            foreach (var rekord in wynik)
            {
                lista.Add(new Model
                {
                    Average = rekord.Value.Average(o => o.Dopasowanie),
                    Dopasowanie = rekord.Value.Max(o => o.Dopasowanie),
                    Generacja = rekord.Key + 1,
                    X1 = rekord.Value.OrderByDescending(o => o.Dopasowanie).First().X1,
                    X2 = rekord.Value.OrderByDescending(o => o.Dopasowanie).First().X2,

                });

            }
            dataGridView1.DataSource = lista;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var algorytm = new Algorithm2();

            var historiaPopulacji = algorytm.Start();

            var lista = new List<Model2>();
            foreach (var populacja in historiaPopulacji)
            {
                var najbardziejDopasowany = populacja.Value
                    .OrderBy(v => v.Dopasowanie)
                    .First();

                lista.Add(new Model2
                {
                    Average = populacja.Value.Average(o => o.Dopasowanie),
                    Dopasowanie = najbardziejDopasowany.Dopasowanie,
                    Generacja = populacja.Key + 1,
                    Pa = najbardziejDopasowany.Pa,
                    Pb = najbardziejDopasowany.Pb,
                    Pc = najbardziejDopasowany.Pc
                });

            }
            dataGridView2.DataSource = lista;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var algorytm = new GeneticAlgorithmXOR();
            var historiaPopulacji = algorytm.Start();
            var lista = new List<Model3>();
            foreach (var populacja in historiaPopulacji)
            {
                var najbardziejDopasowany = populacja.Value
                    .OrderBy(v => v.Dopasowanie)
                    .First();

                lista.Add(new Model3
                {
                    Average = populacja.Value.Average(o => o.Dopasowanie),
                    Dopasowanie = najbardziejDopasowany.Dopasowanie,
                    Generacja = populacja.Key + 1,
                    x1 = najbardziejDopasowany.wagi[0][0],
                    x2 = najbardziejDopasowany.wagi[0][1],
                    x3 = najbardziejDopasowany.wagi[0][2], //[[], [], []]
                    x4 = najbardziejDopasowany.wagi[1][0],
                    x5 = najbardziejDopasowany.wagi[1][1],
                    x6 = najbardziejDopasowany.wagi[1][2],
                    x7 = najbardziejDopasowany.wagi[2][0],
                    x8 = najbardziejDopasowany.wagi[2][1],
                    x9 = najbardziejDopasowany.wagi[2][2],
                });

            }
            dataGridView3.DataSource = lista;
        }
    }
}
public class Model
{
    public int Generacja { get; set; }
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Dopasowanie { get; set; }
    public double Average { get; set; }

}
public class Model2
{
    public int Generacja { get; set; }
    public double Pa { get; set; }
    public double Pb { get; set; }
    public double Pc { get; set; }
    public double Dopasowanie { get; set; }
    public double Average { get; set; }
}
public class Model3
{
    public int Generacja { get; set; }
    public double x1 { get; set; }
    public double x2 { get; set; }
    public double x3 { get; set; }
    public double x4 { get; set; }
    public double x5 { get; set; }
    public double x6 { get; set; }

    public double x7 { get; set; }
    public double x8 { get; set; }
    public double x9 { get; set; }



    public double Dopasowanie { get; set; }
    public double Average { get; set; }
}
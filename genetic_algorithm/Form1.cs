using OpenTK.Graphics.OpenGL;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

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
            // Set up the DataGridView properties
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Generacja",
                HeaderText = "Generacja",
                Width = 80
            });

            // Add columns
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


        private void button1_Click(object sender, EventArgs e)
        {
            // wykonaj algorytm, który zwróci nam listê generacji z osobnikami
            // przemapuj rezultat na model dopasowany do naszej tabelki

            // dataGridView1.DataSource = nasz model (List<Model>)

            var algorytm = new GeneticAlgoritm();

           var wynik=  algorytm.Start();

            var lista= new List<Model>();
            foreach (var rekord in wynik)
            {
                lista.Add(new Model
                {
                    Average = rekord.Value.Average(o => o.Dopasowanie),
                    Dopasowanie=rekord.Value.Max(o=>o.Dopasowanie),
                    Generacja=rekord.Key + 1,
                    X1 = rekord.Value.OrderByDescending(o=>o.Dopasowanie).First().X1,
                    X2= rekord.Value.OrderByDescending(o=>o.Dopasowanie).First().X2,

                });

            }
            dataGridView1.DataSource = lista;




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
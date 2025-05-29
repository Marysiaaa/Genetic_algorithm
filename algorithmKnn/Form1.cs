using System.Windows.Forms;

namespace algorithmKnn
{
    public partial class Form1 : Form
    {
        private AlgorytmKnn knn;
        private double[][] daneZKlasami;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();


            daneZKlasami = DaneIris.dane;
            knn = new AlgorytmKnn(daneZKlasami);

            comboBox1.DataSource = Enum.GetValues(typeof(AlgorytmKnn.Metryka));

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
                DataPropertyName = "Atrybut1",
                HeaderText = "Atrybut1",
                Width = 80
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Atrybut2",
                HeaderText = "Atrybut2",
                Width = 80
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Atrybut3",
                HeaderText = "Atrybuty3",
                Width = 80
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Atrybut4",
                HeaderText = "Atrybut4",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "RzeczywistaKlasa",
                HeaderText = "RzeczywistaKlasa",
                Width = 80
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PrzewidzianaKlasa",
                HeaderText = "PrzewidzianaKlasa",
                Width = 150
            });


        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Nie poda³eœ parametru K");
                return;
            }

            if (!int.TryParse(textBox2.Text, out int k)) 
            {
                MessageBox.Show("Parametr K jest liczb¹");
                return;
            }
          

            var wybranaMetryka = (AlgorytmKnn.Metryka)comboBox1.SelectedItem;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            int poprawne = 0;

            int numAtrybutow = daneZKlasami[0].Length - 1;

            for (int i = 0; i < numAtrybutow; i++)
                dataGridView1.Columns.Add($"attr{i}", $"Atrybut {i + 1}");

            dataGridView1.Columns.Add("rzeczywista", "Rzeczywista klasa");
            dataGridView1.Columns.Add("przewidziana", "Przewidziana klasa");

            for (int i = 0; i < daneZKlasami.Length; i++)
            {
                var test = daneZKlasami[i];
                var testAtrybuty = test.Take(test.Length - 1).ToArray();
                int rzeczywista = (int)test.Last();

                var treningowe = daneZKlasami
                    .Where((_, idx) => idx != i)
                    .Select(row => row.ToArray())
                    .ToArray();

                var alg = new AlgorytmKnn(treningowe);
                int przewidziana = alg.Klasyfikuj(testAtrybuty, wybranaMetryka, k);

                if (przewidziana == rzeczywista)
                    poprawne++;

                var row = testAtrybuty.Select(x => x.ToString("F2")).ToList();
                row.Add(rzeczywista.ToString());
                row.Add(przewidziana.ToString());
                dataGridView1.Rows.Add(row.ToArray());
            }


            double skutecznosc = 100.0 * poprawne / daneZKlasami.Length;

            textBox1.Text = ($"Skutecznoœæ: {skutecznosc:F2}%");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Model
    {
        public double[] Atrybut1 { get; set; }
        public double[] Atrybut2 { get; set; }
        public double[] Atrybut3 { get; set; }
        public double[] Atrybut4 { get; set; }

        public int RzeczywistaKlasa { get; set; }
        public int PrzewidzianaKlasa { get; set; }
      

    }
}

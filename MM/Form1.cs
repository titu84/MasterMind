using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        char[] colors = new char[] { 'r', 'y', 'g', 'b', 'm', 'c' };
        char[] secret = new char[4];
        private void Form1_Load(object sender, EventArgs e)
        {
            maskedTextBox1.Text = "0009";
            addSecret();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Dodanie próby do DataGrida
            dgv.Rows.Add(comboBox1.Text, comboBox2.Text, comboBox3.Text, comboBox4.Text);

            //Sprawdzanie trafień
            bool?[] hits = getHits(new char[] { comboBox1.Text.ToChar(), comboBox2.Text.ToChar(), comboBox3.Text.ToChar(), comboBox4.Text.ToChar() });
           
            string hitsStr = "";
            for (int i = 0; i < hits.Where(a => a.HasValue && a.Value == true).Count(); i++)
            {
                hitsStr += "•";
            }
            for (int i = 0; i < hits.Where(a => a.HasValue && a.Value == false).Count(); i++)
            {
                hitsStr += "○";
            }
            //kolorki w gridzie
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Style.BackColor = getColor(row.Cells[0].Value.ToString());
                row.Cells[1].Style.BackColor = getColor(row.Cells[1].Value.ToString());
                row.Cells[2].Style.BackColor = getColor(row.Cells[2].Value.ToString());
                row.Cells[3].Style.BackColor = getColor(row.Cells[3].Value.ToString());                
            }
            int lastRow = dgv.Rows.GetLastRow(DataGridViewElementStates.Visible); // Index ostatniego row w Gridzie
            dgv.Rows[lastRow].Cells[4].Value = hitsStr; // Dopisanie kropek wskazujących ilość poprawnych odpowiedzi
            dgv.ClearSelection();
            if (hitsStr == "••••")
            {
                MessageBox.Show("Gratulacje wygrałeś!!!");
                addSecret();                
                return;
            }
            if (dgv.RowCount == int.Parse(maskedTextBox1.Text))
            {
                MessageBox.Show("Przegrałeś, koniec gry!\n Sekretny kod to: " + string.Join("_", secret));
                addSecret();               
            } 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxColor(comboBox1);
        }       

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxColor(comboBox2);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxColor(comboBox3);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxColor(comboBox4);
        }
        private void comboBoxColor(ComboBox cb)
        {
            cb.BackColor = getColor(cb.SelectedItem.ToString());
        }
        private Color getColor(string text)
        {
            switch (text)
            {
                case "r":
                    return Color.Red;
                case "y":
                    return Color.Yellow;
                case "g":
                    return Color.Green;
                case "b":
                    return Color.LightBlue;
                case "m":
                    return Color.Magenta;
                case "c":
                    return Color.Cyan;
                default:
                    return Color.White;
            }                   
        }

        private void comboBox1_BackColorChanged(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void comboBox2_BackColorChanged(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void comboBox3_BackColorChanged(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void comboBox4_BackColorChanged(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void button1_Click(object sender, EventArgs e)
        {                       
            addSecret();            
        }

        private void addSecret()
        {           
            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {
                int index = rnd.Next(5);
                secret[i] = colors[index];
            }
            dgv.Rows.Clear();
        }
        private bool?[] getHits(char[] input)
        {
            bool?[] result = new bool?[4];
            // null  - brak trafienia
            // false - trafienie ale na złym polu
            // true  - trafienie pełne

            for (int i = 0; i < 4; i++)
            {
                if (secret[i] == input[i])
                {
                    result[i] = true;                   
                }
                else if (secret[i] != input[i] && secret.Contains(input[i]))
                {
                    result[i] = false;
                }
                else
                {
                    result[i] = null;
                }
            }
            return result;
        }
    }
    public static class Extensions
    {
        public static char ToChar(this string value)
        {
            return Convert.ToChar(value);
        }
    }
}

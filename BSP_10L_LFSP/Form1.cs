using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BSP_10L_LFSP
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bmp;
        Pen DrawPen = new Pen(Color.Red, 5);
        Pen PenLine = new Pen(Color.Red, 1);
        Pen PenLineCon = new Pen(Color.Blue, 1);
        public Form1()
        {
            InitializeComponent();
        }

        public void DrawPoints(uint[] mas)
        {
            g = pictureBox1.CreateGraphics();

            int otstup_x = 32;
            int l_otstup = 10;
            int otstup_y = 20;

            int height_picture = Convert.ToInt32(mas.Max<uint>()) + 30;
            bmp = new Bitmap(mas.Length* otstup_x + l_otstup, height_picture);

            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = height_picture;
            g = Graphics.FromImage(bmp);

            pictureBox1.Image = bmp;
            g.Clear(Color.White);



            string label_counts = "   ";
            string label_numbers = "   ";

            for (int i = 0; i < mas.Length; i++)
            {

                label_counts += mas[i].ToString();
                for (int j = 8 - mas[i].ToString().Length; j > 0; j--)
                    label_counts += " ";

                label_numbers += i.ToString();
                for (int j = 9 - i.ToString().Length; j > 0; j--)
                    label_numbers += " ";

                int main_otstup = i * otstup_x + l_otstup;

                g.DrawEllipse(DrawPen, main_otstup - 2, height_picture - mas[i] - otstup_y, 4, 4);
                g.DrawLine(PenLine, new Point(main_otstup, height_picture - (int)mas[i] - otstup_y), new Point(main_otstup, height_picture));

                if (i != mas.Length - 1)
                {
                    g.DrawLine(PenLineCon, new Point(main_otstup, height_picture - (int)mas[i] - otstup_y), new Point(main_otstup + otstup_x, height_picture - (int)mas[i+1] - otstup_y));
                }
            }
            label1.Text = label_counts;
            label2.Text = label_numbers;

            pictureBox1.Refresh();

        }

        public class LFSRGenerator
        {
            private uint state;
            private readonly uint polynomial;

            public LFSRGenerator(uint seed, uint polynomial)
            {
                if (seed == 0)
                    throw new ArgumentException("Seed cannot be zero", nameof(seed));

                this.state = seed;
                this.polynomial = polynomial;
            }

            public uint Next()
            {
                uint buff_state = state & polynomial;
                if ((buff_state == 0b11001) || (buff_state == 0b10101) || (buff_state == 0b01101) || (buff_state == 0b00001) || (buff_state == 0b10000) || (buff_state == 0b01000) || (buff_state == 0b00100) || (buff_state == 0b11100))
                    state = (state >> 1) | 0b10000000;//128 = 0b10000000
                else
                    state = state >> 1;

                return state;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            // Пример неприводимого многочлена: x^8 + x^4 + x^3 + x^2 + 1 (0x1D в шестнадцатеричном виде, 29 десятичной)
            uint polynomial = 29; // 8-битный

            uint seed = Convert.ToUInt32(textBox1.Text);
            if (seed == 0)
            {
                MessageBox.Show("Начальное состояние не может быть 0"); 
                return;
            }

            var generator = new LFSRGenerator(seed, polynomial);

            uint[] result = new uint[256];
            result[0] = seed;

            for (int i = 1; i < 256; i++)
                result[i] = generator.Next();
            DrawPoints(result);
            FillListBox(result);
        }

        private void FillListBox(uint[] result)
        {
            listBox1.Items.Clear();
            for(int i = 0; i < result.Length; i++) 
                listBox1.Items.Add(result[i].ToString());
        }
    }
}

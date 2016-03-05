using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            label1.Text = "Gen: " + generation.ToString();

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
        }

        Timer timer;

        Graphics g;
        BufferedGraphicsContext bufferedGraphicsContext;
        BufferedGraphics bufferedGraphics;

        int rec_Width;
        int rec_Height;
        const int field_size = 30;
        int generation = 0;

        bool[,] arrLife = new bool[field_size, field_size];
        bool[,] arrLifeTemp = new bool[field_size, field_size];

        int countValue = field_size;

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (e.X / rec_Width);
            int y = (e.Y / rec_Height);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                bufferedGraphics.Graphics.FillRectangle(Brushes.Green, 
                    x * rec_Width + 2, y * rec_Height + 2, rec_Width - 3, rec_Height - 3);
                arrLife[x, y] = true;
            }
            else
            {
                bufferedGraphics.Graphics.FillRectangle(SystemBrushes.Control,
                    x * rec_Width + 2, y * rec_Height + 2, rec_Width - 3, rec_Height - 3);
                arrLife[x, y] = false;
            }

            bufferedGraphics.Render();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromHwnd(panel1.Handle);
            bufferedGraphicsContext = new BufferedGraphicsContext();
            bufferedGraphics = bufferedGraphicsContext.Allocate(g, 
                new Rectangle(0, 0, panel1.Size.Width, panel1.Size.Height));
            bufferedGraphics.Graphics.Clear(BackColor);
            Pen p = new Pen(Color.Black);
            rec_Width = panel1.Width / field_size;
            rec_Height = panel1.Height / field_size;
            for (int i = 0; i <= panel1.Width; i += rec_Width)
                for (int j = 0; j <= panel1.Height; j += rec_Height)
                {
                    bufferedGraphics.Graphics.DrawRectangle(p, i, j, rec_Width, rec_Height);
                }

            bufferedGraphics.Render();
        }

        private void GoLife()
        {
            for (int i = 0; i < countValue; i++)
                for (int j = 0; j < countValue; j++)
                {
                    int sosedi = Sosedi(i, j);

                    if (sosedi == 3 && (!arrLife[i, j])) { arrLifeTemp[i, j] = true; }
                    if ((sosedi == 3 || sosedi == 2) && arrLife[i, j]) { arrLifeTemp[i, j] = true; }
                    if (sosedi < 2) { arrLifeTemp[i, j] = false; }
                    if (sosedi > 3) { arrLifeTemp[i, j] = false; }
                }

            for (int i = 0; i < countValue; i++)
                for (int j = 0; j < countValue; j++)
                {
                    if (arrLife[i, j] != arrLifeTemp[i, j])
                    {
                        arrLife[i, j] = arrLifeTemp[i, j];
                        if (arrLife[i, j])
                        {
                            bufferedGraphics.Graphics.FillRectangle(Brushes.Green, 
                                i * rec_Width + 2, j * rec_Height + 2, rec_Width - 3, rec_Height - 3);
                        }
                        else
                        {
                            bufferedGraphics.Graphics.FillRectangle(SystemBrushes.Control, 
                                i * rec_Width + 2, j * rec_Height + 2, rec_Width - 3, rec_Height - 3);
                        }

                        bufferedGraphics.Render();
                    }
                }

            generation++;
            label1.Text = "Gen: " + generation.ToString();
        }

        private int Sosedi(int i, int j)
        {
            int sosedi = 0;
            int limit = countValue - 1;

            if (i > 0 && arrLife[i - 1, j]) { sosedi++; }
            if (i > 0 && j > 0 && arrLife[i - 1, j - 1]) { sosedi++; }
            if (i > 0 && j < countValue - 1 && arrLife[i - 1, j + 1]) { sosedi++; }
            if (j < countValue - 1 && arrLife[i, j + 1]) { sosedi++; }
            if (j > 0 && arrLife[i, j - 1]) { sosedi++; }
            if (i < countValue - 1 && arrLife[i + 1, j]) { sosedi++; }
            if (i < countValue - 1 && j < countValue - 1 && arrLife[i + 1, j + 1]) { sosedi++; }
            if (i < countValue - 1 && j > 0 && arrLife[i + 1, j - 1]) { sosedi++; }

            return sosedi;
        }

        private void run_btn_Click(object sender, EventArgs e)
        {
            timer.Start();

            run_btn.Enabled = false;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            GoLife();
        }

        private void stop_btn_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
                run_btn.Enabled = true;
            }
        }

        private void next_Step_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
                run_btn.Enabled = true;
            }

            GoLife();
        }
    }
}

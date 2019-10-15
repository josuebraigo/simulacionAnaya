using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace practica01
{
    public partial class Form1 : Form
    {

        int cont = 0;
        int x_c, y_c, x_r, y_r, r = 0;
        Bitmap myBitmap;
        Graphics g;
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int posX = e.X;
            int posY = e.Y;

            //myBitmap.SetPixel(posX, posY, Color.Black);

            pictureBox1.Image = myBitmap;

            if (cont == 0)
            {
                x_c = posX;
                y_c = posY;

                cont++;

            }
            else if (cont == 1)
            {
                x_r = posX;
                y_r = posY;

                calculateRatio();
                drawWithBresenham(x_c, y_c, r);
                drawWithDDA1(x_c + r, y_c, r);
                drawWithDDA2(x_c - r, y_c, r);
                pictureBox1.Image = myBitmap;

                cont = 0;
            }
        }

        public Form1()
        {
            InitializeComponent();

            myBitmap = new Bitmap("C:/Users/casti/Documents/7mo Semestre/06 Simulación Por Computadora/2da Oportunidad/practica01/practica01/fondo-blanco.jpg");
            cont = 0;

            g = Graphics.FromImage(myBitmap);
            pictureBox1.Image = myBitmap;
        }

        private void calculateRatio()
        {
            int dx = x_r - x_c;
            int dy = y_r - y_c;

            int x0 = x_c;
            int x1 = x_r;
            int y0 = y_c;
            int y1 = y_r;

            float m_temp;
            r = 0;

            r++;
            if (Math.Abs(dx) > Math.Abs(dy))
            {          // pendiente < 1
                float m = (float)dy / (float)dx;
                float b = y0 - m * x0;
                if (dx < 0)
                    dx = -1;
                else
                    dx = 1;
                while (x0 != x1)
                {
                    x0 += dx;
                    m_temp = (m * x0) + b;
                    y0 = (int)Math.Round(m_temp);
                    r++;
                }
            }
            else
            if (dy != 0)
            {
                float m = (float)dx / (float)dy;
                float b = x0 - m * y0;
                if (dy < 0)
                    dy = -1;
                else
                    dy = 1;
                while (y0 != y1)
                {
                    y0 += dy;
                    x0 = (int)Math.Round(m * y0 + b);
                    r++;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void setOctants(int x, int y, int xc, int yc, Color color)
        {
            if (((y + xc) < myBitmap.Width && (y + xc) > 0) && ((x + yc) < myBitmap.Height && (x + yc) > 0))
            {
                myBitmap.SetPixel(y + xc, x + yc, color);   
            }
            if (((x + xc) < myBitmap.Width && (x + xc) > 0) && ((y + yc) < myBitmap.Height && (y + yc) > 0))
            {
                myBitmap.SetPixel(x + xc, y + yc, color);
            }
            if (((xc - x) < myBitmap.Width && (xc - x) > 0) && ((y + yc) < myBitmap.Height && (y + yc) > 0))
            {
                myBitmap.SetPixel(xc - x, y + yc, color);
            }
            if (((xc - y) < myBitmap.Width && (xc - y) > 0) && ((yc + x) < myBitmap.Height && (yc + x) > 0))
            {
                myBitmap.SetPixel(xc - y, yc + x, color);
            }
            if (((xc - y) < myBitmap.Width && (xc - y) > 0) && ((yc - x) < myBitmap.Height && (yc - x) > 0))
            {
                myBitmap.SetPixel(xc - y, yc - x, color);
            }
            if (((xc - x) < myBitmap.Width && (xc - x) > 0) && ((yc - y) < myBitmap.Height && (yc - y) > 0))
            {
                myBitmap.SetPixel(xc - x, yc - y, color);
            }
            if (((xc + x) < myBitmap.Width && (xc + x) > 0) && ((yc - y) < myBitmap.Height && (yc - y) > 0))
            {
                myBitmap.SetPixel(xc + x, yc - y, color);
            }
            if (((xc + y) < myBitmap.Width && (xc + y) > 0) && ((yc - x) < myBitmap.Height && (yc - x) > 0))
            {
                myBitmap.SetPixel(xc + y, yc - x, color);
            }
        }

        private void drawWithBresenham(int xc, int yc, int r)
        {
            int x, y, p;
            x = 0;
            y = r;
            p = 1 - r;

            setOctants(x, y, xc, yc, Color.Blue);

            while (x < y)
            {
                x++;
                if (p < 0)
                {
                    p = p + (2 * x) + 1;
                }
                else
                {
                    y--;
                    p = p + 2 * (x - y) + 1;
                }

                setOctants(x, y, xc, yc, Color.Blue);
            }


        }

        private void drawWithDDA1(int xc, int yc, int r)
        {
            int x = 0;
            int y;

            y = (int)Math.Sqrt(Math.Abs((r * r) - (x * x)));
            while (x <= y)
            {
                y = (int)Math.Sqrt(Math.Abs((r * r) - (x * x)));
                setOctants(x, y, xc, yc, Color.Red);
                x++;
            }
        }

        private void drawWithDDA2(int xc, int yc, int r)
        {
            int xi = xc - r;
            int yi = yc - r;
            int xf = xc + r;
            int yf = yc + r;
            while (xi <= xf)
            {
                yi = (int)Math.Sqrt(Math.Abs(Math.Pow(r, 2) - Math.Pow(xi - xc, 2))) + yc;
                if (xi > 0 && xi < myBitmap.Width && yi > 0 && yi < myBitmap.Height)
                {
                    myBitmap.SetPixel(xi, yi, Color.Black);
                }
                if ((yc - (yi - yc)) > 0 && (yc - (yi - yc)) < myBitmap.Height && xi > 0 && xi < myBitmap.Width)          {
                    myBitmap.SetPixel(xi, yc - (yi - yc), Color.Black);
                }
                xi++;
            }

            xi = xc - r;
            yi = yc - r;
            xf = xc + r;
            yf = yc + r;

            while (yi <= yf)
            {
                xi = (int)Math.Sqrt(Math.Abs(Math.Pow(r, 2) - Math.Pow(yi - yc, 2))) + xc;
                if(xi > 0 && xi < myBitmap.Width && yi > 0 && yi < myBitmap.Height)
                {
                    myBitmap.SetPixel(xi, yi, Color.Black);
                }
                if((xc - (xi - xc)) > 0 && (xc - (xi - xc)) < myBitmap.Width && yi > 0 && yi < myBitmap.Height)
                {
                    myBitmap.SetPixel(xc - (xi - xc), yi, Color.Black);
                }
                yi++;
            }

            /*y = yc - r;
            x = xc;
            yf = yc + r;
            setPixel(x, y);
            while (y >= yf)
            {
                x = calculo();
                setPixel(x, y);
                y++;
            }*/
        }

    }
}

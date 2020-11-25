using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        

        // Draw a rotated string at a particular position.
        private void DrawRotatedTextAt(Graphics gr, float angle,
            string txt, int x, int y, Font the_font, Brush the_brush)
        {

            int centrey = this.Height / 2;
            int centrex = this.Width / 2;

            x = x + centrex;
            y = y + centrey;

            // Save the graphics state.
            GraphicsState state = gr.Save();
            gr.ResetTransform();

            // Rotate.
            gr.RotateTransform(angle);

            // Translate to desired position. Be sure to append
            // the rotation so it occurs after the rotation.
            gr.TranslateTransform(x, y, MatrixOrder.Append);

            // Draw the text at the origin.
             gr.DrawString(txt, the_font, the_brush, 0, 0);

            // Restore the graphics state.
            gr.Restore(state);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawMeTheHexagons(e.Graphics);
        }

        private void drawMeTheHexagons(Graphics gr)
        { 

            // Call the OnPaint method of the base class.  
            //base.OnPaint(e);
            // Call methods of the System.Drawing.Graphics object.  
            
            double outside = 0.95; // maximum size on screen
            double[] fromCentre = new double[] { 37, 40, 55, 73, 85, 104, 120,138,156,159,195,198,214,232 };
            string[] centre = new string[] {"I","J","K","L","G","H" };
            string[] overlap = new string[] { "none", "none", "clock", "anti", "clock", "clock","anti","none","none", "anti", "none", "none", "none" };
            string[,] woods = new string[,] {   { "40","","","","","Gombeira",""},
                                                { "100","","","","","Oak",""},
                                                { "100","Cherry","Ash","Beech","Yew","Walnut","Lacewood"},
                                                { "100","Lacewood","Beech","Ash","Walnut","Yew","Cherry"},
                                                { "100","Beech","Lacewood","Walnut","Ash","Cherry","Yew"},
                                                { "100","Yew","Walnut","Lacewood","Cherry","Ash","Beech"},
                                                { "100","Walnut","Yew","Cherry","Lacewood","Beech","Ash"},
                                                { "100","","","","","Oak",""},
                                                { "40","","","","","Walnut",""},
                                                { "100","C","D","E","F","A","B"},
                                                { "40","","","","","Walnut",""},
                                                { "100","","","","","Oak",""},
                                                { "100","","","","","Oak",""} };

            double maximumWidth = fromCentre[fromCentre.Length - 1];
            int baseFontSize = 12;
            Font textFont = new Font("Comic Sans MS", 10);
            Brush textBrush = Brushes.RosyBrown;

            drawStar(outside,fromCentre[0], maximumWidth, gr,centre,textFont,textBrush);
            //int baseFontSize = 20;
            
            for (int f =0; f < fromCentre.Length-1 ; f++)

            {
                int fontSize = (int)((float.Parse(woods[f, 0])/100)*baseFontSize);
                Font useFont = new Font("Comic Sans MS", fontSize);
                
                drawHexagon(outside,fromCentre[f], fromCentre[f+1], maximumWidth, overlap[f], woods,f,useFont,textBrush, gr);
                
            }
            drawHexagon(outside,maximumWidth, maximumWidth ,maximumWidth, "none", woods,99,textFont,textBrush ,gr);
            
            
            //DrawRotatedTextAt(e.Graphics, 60, "Walnut", 100, 100, textFont, textBrush);
        }

        private void drawStar(double outside,double fromCentre,double maximumWidth, Graphics gr, string[] centre, Font textFont, Brush textBrush)
        {
            double outsideWidth = (this.Height * outside);
            double actualWidth = ((fromCentre / maximumWidth) * outsideWidth) / 2;
            for (int i = 0; i < 6; i++)
            {
                int[] coord = new int[4];
                int h = i + 1;
                double degrees = (double)((2 * Math.PI) / 6) * i;
                int x = (int)(actualWidth * Math.Cos(degrees));
                int y = (int)(actualWidth * Math.Sin(degrees));
                int xT = (int)((actualWidth/1.5) * Math.Cos(degrees + (Math.PI / 9)));
                int yT = (int)((actualWidth / 1.5) * Math.Sin(degrees + (Math.PI / 9)));

                double degreesT = (degrees / (2 * Math.PI)) * 360 + 120;
                int xh = 0;
                int yh = 0;

                coord[0] = x;
                coord[1] = y;
                coord[2] = xh;
                coord[3] = yh;

                drawLine(coord, gr);
                DrawRotatedTextAt(gr, (float)degreesT, centre[i], xT, yT, textFont, textBrush);
            }
        }

        private void drawHexagon(double outside,double innerWidth, double outerWidth, double maximumWidth, string overlap, string[,] woods, int index,Font textFont, Brush textBrush,Graphics gr)
        {
            
            double outsideWidth = (this.Height * outside);
            innerWidth = ((innerWidth / maximumWidth) * outsideWidth)/2;
            outerWidth = ((outerWidth / maximumWidth) * outsideWidth) / 2;
            double widthDifference = outerWidth - innerWidth;

            for (int i = 0; i < 6; i++)
            {
                int[] coord = new int[4];
                int h = i + 1;
                double degrees = (double)((2 * Math.PI) / 6) * i;
                int x = (int)(innerWidth * Math.Cos(degrees));
                int y = (int)(innerWidth * Math.Sin(degrees));
                double degreesOffset = (double)((2 * Math.PI) / 6) * (i + 2);
                if (overlap == "anti")
                {
                    x = x - (int)(widthDifference * Math.Cos(degreesOffset));
                    y = y - (int)(widthDifference * Math.Sin(degreesOffset));

                }

                double degreesEnd = (double)((2 * Math.PI) / 6) *( i+1);
                degreesOffset = (double)((2 * Math.PI) / 6) * (i+2);
                int xh = (int)((innerWidth * Math.Cos(degreesEnd)) );
                int yh = (int)((innerWidth * Math.Sin(degreesEnd)) );
                if (overlap == "clock")
                {
                    xh = xh + (int)(widthDifference * Math.Cos(degreesOffset));
                    yh = yh + (int)(widthDifference * Math.Sin(degreesOffset));

                }
                coord[0] = x;
                coord[1] = y;
                coord[2] = xh;
                coord[3] = yh;

                drawLine(coord,gr);


                if (index != 99)
                {
                float textDegrees = (float)(degrees / (2 * Math.PI) * 360)+120;
                if (textDegrees > 360) { textDegrees = textDegrees - 360; }

                string woodText = woods[index, i + 1];
                Size textSize = TextRenderer.MeasureText(gr, woodText, textFont);

                double textDegreesPos = degrees;// + (Math.PI / 6);
                double textWidth = ((innerWidth + outerWidth) / 2);
                textWidth = textWidth + ((textSize.Height) / 1.75);
                int xT1 = (int)( textWidth * Math.Cos(textDegreesPos));
                int yT1 = (int)( textWidth * Math.Sin(textDegreesPos));
                int xT2 = (int)(textWidth * Math.Cos(textDegreesPos + (Math.PI / 3)));
                int yT2 = (int)(textWidth * Math.Sin(textDegreesPos+(Math.PI /3)));
                int xT = (xT1 + xT2) / 2;
                int yT = (yT1 + yT2) / 2;

                    //DrawRotatedTextAt(e.Graphics, textDegrees, woodText, xT, yT, textFont, Brushes.Coral);
                    //DialogResult result = MessageBox.Show(textDegrees.ToString());               
                    xT = xT + (int)((textSize.Width / 4) * (Math.Cos(textDegreesPos-(Math.PI/3))));
                    yT = yT + (int)((textSize.Width / 4) * (Math.Sin(textDegreesPos-(Math.PI/3))));

                    
                    DrawRotatedTextAt(gr, textDegrees, woodText, xT, yT, textFont, textBrush);
                }

            }
                

        }

        private void drawLine (int[] coordinates, Graphics gr ) //PaintEventArgs e)
        {
            int centrey = this.Height / 2;
            int centrex = this.Width / 2;

            PointF start = new Point(coordinates[0]+centrex, coordinates[1]+ centrey);
            PointF end = new Point(coordinates[2]+ centrex, coordinates[3]+ centrey);
            Pen blackPen = new Pen(Color.Black, 1);

            gr.DrawLine(blackPen, start, end);
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            Graphics gr = panel1.CreateGraphics();

            drawMeTheHexagons(gr);
        }
    }
}

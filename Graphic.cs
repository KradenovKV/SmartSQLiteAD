using System.Linq;
using System.Drawing;
using System.Windows.Forms;


namespace SmartSQLite
{

    class Graphic
    {
        //private Form1 f1;

        //свойства
        int width;
        int height;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        //**************************

        //public string Attribute { get; set; }

        //private static string[] sMonitorID;
        //public string[] SMonitorID { get { return sMonitorID; } private set { SMonitorID = value; } }
        public int iTop;
        public int iLeft;

        public int iWidthPix;
        //{
        //    get { return iWidthPix; }
        //    private set { iWidthPix = value; } //private
        //}

        public int iHeightPix { get; set; }
        //{
        //    get { return iHeightPix; }
        //    set { iHeightPix = value; } //private set { iHeightPix = value; }
        //}

        public Graphic (int iTopNew, int iLeftNew, int iHeightPixNew, int iWidthPixNew)
        {
            iTop = iTopNew;
            iLeft = iLeftNew;
            iWidthPix = iWidthPixNew;
            iHeightPix = iHeightPixNew;
        }


        //************
        public void GraphShow(int[] aSmartVal, string sFrstDate, string sLastDate)
        {
            Pen RedPen = new Pen(Color.Red, 3);
            Bitmap BM = new Bitmap(iWidthPix, iHeightPix);
            //string sFileBmp = sPathStart + "\\bmp0.bmp";
            //BM.Save(sFileBmp);
            Form1.PicBox.Height = iHeightPix;
            Form1.PicBox.Width = iWidthPix;
            Form1.PicBox.Top = iTop; // iGridsTop + 10;
            Form1.PicBox.Left = iLeft;
            Form1.PicBox.BorderStyle = BorderStyle.FixedSingle;
            Form1.PicBox.Show();
            Form1.PicBox.Enabled = true;
            Form1.PicBox.Visible = true;
            Form1.Graph = Graphics.FromImage(BM);
            Form1.Graph.Clear(Color.White);
            Form1.PicBox.Image = BM;

            int iTpoints = aSmartVal.Length - 1;
            float X0, Y0, X1, Y1, Y, dT;
            Pen BluePen = new Pen(Color.Blue, 1);
            float dX = iWidthPix / iTpoints;//кол-во отрезков на 1 меньше точек
            float dTmax = iTpoints + 1;
            float Ymax = aSmartVal.Max();
            float Ymin = aSmartVal.Min();// 0-минимальное значение по Y

            for (int iP = 0; iP < iTpoints; iP++)
            {
                Y = iHeightPix - (aSmartVal[iP + 1] / Ymax * iHeightPix); // aSmartVal[iP+1]; //(aSmartVal[iP+1] / Ymax * iHeightPix) //  ((aSmartVal[iP+1] - 0) / ((dTmax - 0) * iHeightPix));
              // Form1.Graph.DrawLine(BluePen, iP * dX, 0, iP * dX, iHeightPix);
                {
                    dT = aSmartVal[iP + 1] - aSmartVal[iP];
                    X0 = dX * iP;
                    Y0 = iHeightPix - (aSmartVal[iP]) / Ymax * iHeightPix;
                    X1 = dX * (iP + 1);
                    Y1 = iHeightPix - (aSmartVal[iP + 1]) / Ymax * iHeightPix;
                    Form1.Graph.DrawLine(RedPen, X0, Y0, X1, Y1);
                }
                Form1.PicBox.Image = BM;
            }
            int i = iTpoints; 
            //Form1.Graph.DrawLine(BluePen, i * dX, 0, i * dX, iHeightPix);
        }



    }
}

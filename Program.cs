using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SmartSQLite
{
    static class Program
    {

        //public static Form1 f1; // переменная, которая будет содержать ссылку на форму Form1

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GraphicScene
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            timeBeginPeriod(1);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            timeEndPeriod(1);
            Properties.Settings.Default.Save();
        }

        [DllImport("Winmm.dll")]
        private static extern long timeBeginPeriod(uint uPeriod);
        [DllImport("Winmm.dll")]
        private static extern long timeEndPeriod(uint uPeriod);


    }
}

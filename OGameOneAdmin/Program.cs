using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using LibCommonUtil;

namespace OGameOneAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MemoryManager.AttachApp(true, true, new TimeSpan(0, 30, 0).Ticks, new TimeSpan(0, 2, 0).Ticks);
            OneInstance.Run(new MainForm(), "OGameOneAdmin");
        }
    }
}

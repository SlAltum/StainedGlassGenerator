using System.Collections.Specialized;
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace StainedGlassGenerator{

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        // ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }    
}

}
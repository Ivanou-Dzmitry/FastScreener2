using System.Runtime.InteropServices;

namespace FastScreener2
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int value);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                SetProcessDpiAwareness(2);  // Per-Monitor DPI Awareness
            }
            catch
            {
                SetProcessDPIAware();  // Fallback for older Windows versions
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FS2MainForm());
        }
    }
}
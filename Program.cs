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
            bool createdNew;

            using (Mutex mutex = new Mutex(true, "FastScreener2_UniqueAppName", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("FastScreener is already running!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; // Exit if another instance is already running
                }

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
                ApplicationConfiguration.Initialize(); // Optional config
                Application.Run(new FS2MainForm());
            }
        }

    }
}
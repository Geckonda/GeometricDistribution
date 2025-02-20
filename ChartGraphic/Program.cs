using System.Runtime.InteropServices;

namespace ChartGraphic
{
    internal static class Program
    {

        [STAThread]
        public static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChartForm());
        }
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
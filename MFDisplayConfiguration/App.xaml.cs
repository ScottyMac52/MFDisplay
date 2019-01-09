using log4net;
using MFDSettingsManager.Helpers;
using MFDSettingsManager.Interfaces;
using System.Reflection;
using System.Windows;

namespace MFDisplayConfiguration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ILogConfig
    {

        /// <summary>
        /// Logger for the application
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Logger = LogHelper.GetLogger(typeof(App).Name, GetLoggerName("MFDisplayConfiguration"));
            Logger.Info($"Starting {Assembly.GetExecutingAssembly().Location}");
            var mainWnd = new MainWindow()
            {
                Logger = Logger
            };
            mainWnd.ShowDialog();
            Shutdown(0);
        }
                
        /// <summary>
        /// Get the name of the logger
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetLoggerName(string fileName)
        {
            return LogHelper.GetLoggerFileName(fileName);
        }
    }
}

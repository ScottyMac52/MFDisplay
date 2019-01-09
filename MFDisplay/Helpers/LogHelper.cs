using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Text;

namespace MFDisplay.Helpers
{
    /// <summary>
    /// <seealso cref="ILog"/> support
    /// </summary>
    public static class LogHelper
    {
        static LogHelper()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        /// <summary>
        /// Gets the ILog instance and the specified appenders
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ILog GetLogger(string logName, string fileName)
        {
            var log = LogManager.Exists(logName);

            if (log != null) return log;

            var appenderName = $"{logName}Appender";
            log = LogManager.GetLogger(logName);
            ((Logger)log.Logger).AddAppender(GetRollingFileAppender(appenderName, fileName, Level.All));
            ((Logger)log.Logger).AddAppender(GetConsoleAppender("ColoredConsoleAppender", Level.All));
            return log;
        }

        /// <summary>
        /// Gets a colored console appender
        /// </summary>
        /// <param name="appenderName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ColoredConsoleAppender GetConsoleAppender(string appenderName, Level level)
        {
            var layout = new PatternLayout { ConversionPattern = "%date{dd.MM.yyyy HH:mm:ss}  [%-5level]  %message%newline" };
            layout.ActivateOptions();

            var appender = new ColoredConsoleAppender()
            {
                Layout = layout,
                Name = appenderName,
                Target = "Console.Out",
                Threshold = level
            };

            appender.ActivateOptions();
            return appender;
        }

        /// <summary>
        /// Gets a rolling file appender
        /// </summary>
        /// <param name="appenderName"></param>
        /// <param name="fileName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static RollingFileAppender GetRollingFileAppender(string appenderName, string fileName, Level level)
        {
            var layout = new PatternLayout { ConversionPattern = "%date{dd.MM.yyyy HH:mm:ss.fff}  [%-5level]  %message%newline" };
            layout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                Name = appenderName,
                File = fileName,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaxSizeRollBackups = 2,
                MaximumFileSize = "500KB",
                Layout = layout,
                ImmediateFlush = true,
                LockingModel = new FileAppender.MinimalLock(),
                Encoding = Encoding.UTF8,
                Threshold = level
            };

            appender.ActivateOptions();

            return appender;
        }

    }
}

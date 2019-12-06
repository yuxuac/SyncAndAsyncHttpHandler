using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace SyncAndAsyncHttpHandler.Base
{
    public static class Logging
    {
        public static void Setup(Level level)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            // 文件Appender
            PatternLayout filePatternLayout = new PatternLayout
            {
                ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss} [%thread] %-5level %-75.75logger - %message%newline"
            };
            filePatternLayout.ActivateOptions();
            RollingFileAppender fileAppender = new RollingFileAppender
            {
                Encoding = Encoding.UTF8,
                AppendToFile = true,
                File = @".\log\",
                RollingStyle = RollingFileAppender.RollingMode.Date,
                StaticLogFileName = false,
                PreserveLogFileNameExtension = true,
                Layout = filePatternLayout,
                DatePattern = "yyyyMMdd'.log'"
            };
            fileAppender.ActivateOptions();

            // 控制台Appender
            PatternLayout consolePatternLayout = new PatternLayout
            {
                ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss} [%thread] - %message%newline"
            };
            consolePatternLayout.ActivateOptions();
            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = consolePatternLayout;
            consoleAppender.ActivateOptions();

            // 添加Appenders
            hierarchy.Root.AddAppender(fileAppender);
            hierarchy.Root.AddAppender(consoleAppender);
            hierarchy.Root.Level = level;
            hierarchy.Configured = true;
        }

        private static Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();

        private static object locker = new object();

        public static ILog Write
        {
            get
            {
                Type triggerType = new StackTrace().GetFrames().Skip(1).First().GetMethod().DeclaringType;

                if (triggerType == null)
                    triggerType = typeof(Logging);


                if (!Loggers.ContainsKey(triggerType))
                {
                    lock (locker)
                    {
                        if (!Loggers.ContainsKey(triggerType))
                        {
                            Loggers.Add(triggerType, LogManager.GetLogger(triggerType));
                        }
                    }
                }

                return Loggers[triggerType];
            }
        }
    }
}
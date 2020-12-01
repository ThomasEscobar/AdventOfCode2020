using System.IO;
using Serilog;
using Serilog.Core;

namespace AdventOfCode.Logging
{
    public class CustomLogging
    {
        public static Logger Init(string outputFilePath = null)
        {
            var loggerConfig = new LoggerConfiguration();

            loggerConfig = loggerConfig.MinimumLevel.Verbose();

            // Add file as a target
            if (!string.IsNullOrEmpty(outputFilePath))
            {
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                loggerConfig.WriteTo.File(outputFilePath);
            }

            // Add console as a target
            loggerConfig.WriteTo.Console(outputTemplate: "{Level} - {Message} {Exception} {NewLine}");

            var logger = loggerConfig.CreateLogger();
            Log.Logger = logger;
            return logger;
        }
    }
}
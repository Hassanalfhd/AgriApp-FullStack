using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_Shared.Interfaces;
using Agricultural_For_CV_Shared.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Agricultural_For_CV_BLL.Services
{
    public class LoggingService : ILoggingService
    {

        private readonly string _logDirectory;

        
        public LoggingService(IOptions<AppSettings> settings, string logDirectory = "wwwroot/Logs")
        {

            logDirectory = $"{settings.Value.wwwrootPath}/Log";
            
            if(!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            
        }

        private string GetLogFilePath()
        {
            return Path.Combine(_logDirectory, $"logs_{DateTime.UtcNow:yyyy:MM:dd}.txt");
        }


        private async Task WriteLogAsync(string level, string message, Exception? ex = null)
        {
            var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}  [{level}  {message}]";
            try
            {
                if (ex != null)
                {
                    logMessage += $"| Exception : {ex.Message}  {ex.StackTrace}";
                }

                string filePath = GetLogFilePath();

                await File.AppendAllTextAsync(filePath, logMessage + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine($"Logging failed: {ex?.Message}");

            }
        }
        public async Task LogInfoAsync(string message)
        {
            await WriteLogAsync("INFO", message);

        }



        public async  Task LogWarningAsync(string message)
        {
            await WriteLogAsync("WARNING", message);

        }


        public async Task LogErrorAsync(string message, Exception? ex = null)
        {
            await WriteLogAsync("ERROR", message, ex);

        }

    }
}

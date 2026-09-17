using System.Collections.Concurrent;
using System.Text;

namespace MovieHubMvc.Logging;

/// <summary>
/// Minimal file logger: writes to Logs/app-yyyy-MM-dd.log
/// Path template may contain {date} placeholder.
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly string _pathTemplate;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();
    private static readonly object FileLock = new();

    public FileLoggerProvider(string pathTemplate)
    {
        _pathTemplate = pathTemplate;
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _pathTemplate));

    public void Dispose() => _loggers.Clear();

    private sealed class FileLogger : ILogger
    {
        private readonly string _category;
        private readonly string _pathTemplate;

        public FileLogger(string category, string pathTemplate)
        {
            _category = category;
            _pathTemplate = pathTemplate;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var path = _pathTemplate.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            var line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] {_category}: {formatter(state, exception)}" +
                (exception != null ? $" | {exception}" : "") +
                Environment.NewLine;

            lock (FileLock)
            {
                File.AppendAllText(path, line, Encoding.UTF8);
            }
        }
    }
}

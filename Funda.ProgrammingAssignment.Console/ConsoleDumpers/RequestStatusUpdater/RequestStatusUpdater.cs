using System.Drawing;
using Funda.ProgrammingAssignment.ServiceProxy.Services.RequestStatusUpdater;
using ShellProgressBar;

namespace Funda.ProgrammingAssignment.Console.ConsoleDumpers.RequestStatusUpdater
{
    public class ProgressBarRequestStatusUpdater : IRequestStatusUpdater
    {
        private ProgressBarOptions _options;
        private ProgressBar _progressbar;
        private string _progressBarText;

        public void Setup(string text)
        {
            _options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true,
                CollapseWhenFinished = false,
                EnableTaskBarProgress = false
            };

            _progressBarText = text;
        }

        public void Initialize(int totalPages, string text = null)
        {
            if (_options == null)
                Setup("");

            _progressbar = new ProgressBar(totalPages, text ?? _progressBarText, _options);

        }

        public void Tick()
        {
            _progressbar?.Tick();
        }
    }

    public class SimpleDumperProgressBarRequestStatusUpdater : IRequestStatusUpdater
    {
        private string _progressBarText;
        private int _totalPages;
        private int _currentPage;

        public void Setup(string text)
        {
            _progressBarText = text;
        }

        public void Initialize(int totalPages, string text = null)
        {
            _totalPages = totalPages;
            _currentPage = 0;
            if (!string.IsNullOrEmpty(text))
                _progressBarText = text;
        }

        public void Tick()
        {
            _currentPage += 1;
            PrintProgress(5);
        }
        public void PrintProgress(int everyNum)
        {
            if (_currentPage % everyNum == 0)
            {
                Colorful.Console.WriteLine($"{_currentPage}/{_totalPages} - {_currentPage / (float)_totalPages * 100f:0}% - {_progressBarText}", Color.DarkGreen);
            }
        }
    }
}
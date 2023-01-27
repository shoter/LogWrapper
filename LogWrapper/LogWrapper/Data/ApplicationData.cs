using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LogWrapper.Data
{
    public class ApplicationData : INotifyPropertyChanged
    {
        public List<string> Logs { get; set; } = new();
        public List<string> SelectedLogs => LogsPerCategory[SelectedCategory];

        public Dictionary<string, List<string>> LogsPerCategory { get; set; } = new();

        public ObservableCollection<string> Categories { get; } = new();

        public string SelectedCategory { get; set; } = "*";

        public ApplicationData()
        {
            this.LogsPerCategory.Add("*", new());
            OnTotalChange();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetLogs(List<string> logs)
        {
            this.Logs = logs;
            this.LogsPerCategory.Clear();
            this.LogsPerCategory.Add("*", logs);
            this.SelectedCategory = "*";
            OnTotalChange();
        }

        public void GroupLogs(Regex regex)
        {
            LogsPerCategory.Clear();
            LogsPerCategory["*"] = new List<string>();

            foreach (var log in Logs)
            {
                var matches = regex.Matches(log);
                string category = "*";
                if (matches.Any())
                {
                    category = matches.First().Groups[1].Value;
                }

                if (!LogsPerCategory.ContainsKey(category))
                {
                    LogsPerCategory[category] = new List<string>();
                }

                LogsPerCategory[category].Add(log);
            }

            SelectedCategory = "*";
            OnTotalChange();
        }

        private void OnTotalChange()
        {
            Categories.Clear();
            foreach(var key in LogsPerCategory.Keys)
            {
                Categories.Add(key);
            }
            NotifyPropertyChanged(nameof(SelectedLogs));
            NotifyPropertyChanged(nameof(SelectedCategory));
            NotifyPropertyChanged(nameof(LogsPerCategory));
            NotifyPropertyChanged(nameof(Categories));
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void FocusCategory(string category)
        {
            SelectedCategory = category;
            OnTotalChange();
        }
    }
}

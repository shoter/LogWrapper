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
        public ObservableCollection<string> SelectedLogs { get; } = new();

        public ObservableCollection<string> Categories { get; } = new();

        private Regex groupingRegex = new Regex("");

        public HashSet<string> SelectedCategories { get; } = new();
 
        public ApplicationData()
        {
            OnTotalChange();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetLogs(List<string> logs)
        {
            this.Logs = logs;
            GroupLogs(groupingRegex);
        }

        public void GroupLogs(Regex regex)
        {
            Categories.Clear();
            Categories.Add("*");
            groupingRegex = regex;

            foreach (var log in Logs)
            {
                var category = GetCategory(log);
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }

            OnTotalChange();
        }

        private string GetCategory(string log)
        {
            var matches = groupingRegex.Matches(log);
            if (matches.Any() && matches.First().Groups.Count > 0)
            {
                return matches.First().Groups[1].Value;
            }
            return "none";
        }

        private void OnTotalChange()
        {
            NotifyPropertyChanged(nameof(SelectedLogs));
            NotifyPropertyChanged(nameof(Categories));
            GatherLogsfromCategories();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void FocusCategories(IEnumerable<string> categories)
        {
            foreach(var c in categories)
            {
                SelectedCategories.Add(c);
            }

            GatherLogsfromCategories();

            NotifyPropertyChanged(nameof(SelectedLogs));
        }

        internal void UnfocusCategories(IEnumerable<string> categories)
        {
            foreach (var c in categories)
            {
                SelectedCategories.Remove(c);
            }

            GatherLogsfromCategories();

            NotifyPropertyChanged(nameof(SelectedLogs));
        }

        private void GatherLogsfromCategories()
        {
            SelectedLogs.Clear();
            if (SelectedCategories.Contains("*"))
            {
                foreach (var log in Logs)
                {
                    SelectedLogs.Add(log);
                }
                return;
            }
            foreach(var log in Logs)
            {
                var category = GetCategory(log);
                if(SelectedCategories.Contains(category))
                {
                    SelectedLogs.Add(log);
                }
            }

        }
    }
}

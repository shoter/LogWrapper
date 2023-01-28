using LogWrapper.Components;
using LogWrapper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogWrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationData data = new();
        public MainWindow()
        {
            InitializeComponent();

            CopyFromClipboard(null, null);
            data.GroupLogs(new Regex("<(.*)>"));

            this.DataContext = data;
        }

        private void CopyFromClipboard(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                StringBuilder sb = new();
                List<string> logs = new();
                foreach(var character in clipboardText)
                {
                    if(character == '\r')
                    {
                        continue;
                    }
                    if(character == '\n')
                    {
                        logs.Add(sb.ToString());
                        sb.Clear();
                    }
                    sb.Append(character);
                }

                data.SetLogs(logs);
            }
        }

        private void Group(object sender, RoutedEventArgs e)
        {
            var regex = CreateRegexFromTemplateInput();
            data.GroupLogs(regex);
        }

        private Regex CreateRegexFromTemplateInput()
        {
            string groupText = GroupTemplateInput.Text;
            return new Regex(groupText);
        }

        private void OnCategoryFocus(object sender, SelectionChangedEventArgs e)
        {
            data.FocusCategories(e.AddedItems.Cast<string>());
            data.UnfocusCategories(e.RemovedItems.Cast<string>());
        }
    }
}

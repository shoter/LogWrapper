using LogWrapper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace LogWrapper.Components
{
    /// <summary>
    /// Interaction logic for SimpleLogViewer.xaml
    /// </summary>
    public partial class SimpleLogViewer : UserControl
    {
        public SimpleLogViewer()
        {
            InitializeComponent();
        }

        public void SetLogs(List<string> logs)
        {
            this.DataContext = logs;
        }

    }
}

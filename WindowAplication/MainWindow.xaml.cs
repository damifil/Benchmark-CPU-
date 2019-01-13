using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine;
using WindowAplication.Model;

namespace WindowAplication
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadScores();
            // DiggingEngine a = new DiggingEngine();
            // a.DiggingTestParallel(Algorithm.sha256d);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LoadScores()
        {
            using (Database1Entities db = new Database1Entities())
            {
                var scores = db.Scores;
                List<KeyValuePair<string, int>> scoresList = new List<KeyValuePair<string, int>>();
                foreach (var item in scores)
                    scoresList.Add(new KeyValuePair<string, int>(item.Name, Convert.ToInt32(item.Score)));

                ((BarSeries)mcChart.Series[0]).ItemsSource = scoresList;
            }
        }
    }
}

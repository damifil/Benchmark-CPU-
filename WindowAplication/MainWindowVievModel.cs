using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowAplication.Model;

namespace WindowAplication
{
    public class MainWindowVievModel : INotifyPropertyChanged
    {
        private ICommand runtest;
        private ICommand loadDiagrams;
        internal MainWindowModel model;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunTestCommand
        {
            get
            {
                return runtest;
            }
            set
            {
                runtest = value;
            }
        }

        public ICommand LoadDiagramsCommand
        {
            get
            {
                return loadDiagrams;
            }
            set
            {
                loadDiagrams = value;
            }
        }


        public System.Collections.Generic.List<KeyValuePair<string, int>> Score { get { return model.scoreToShow; } set { model.scoreToShow = value; OnPropertyRaised("Score"); } }

        public Algorithm Algorithm { get { return model.Parametrs.algorithm; } set { model.Parametrs.algorithm = value; OnPropertyRaised("Algorithm"); } }
        public int NumberOfZeroInBegin { get { return model.Parametrs.numberOfZeroInBegin; } set { model.Parametrs.numberOfZeroInBegin = value; OnPropertyRaised("NumberOfZeroInBegin"); } }
        public int StepToFInd { get { return model.Parametrs.stepToFInd; } set { model.Parametrs.stepToFInd = value; OnPropertyRaised("StepToFInd"); } }
        public int IncreasNoZTo { get { return model.Parametrs.increasNoZTo; } set { model.Parametrs.increasNoZTo = value; OnPropertyRaised("IncreasNoZTo"); } }
        public int ValueToChangeTimeInSearch { get { return model.Parametrs.valueToChangeTimeInSearch; } set { model.Parametrs.valueToChangeTimeInSearch = value; OnPropertyRaised("ValueToChangeTimeInSearch"); } }
        public int NumberOfRepeating { get { return model.Parametrs.numberOfRepeating; } set { model.Parametrs.numberOfRepeating = value; OnPropertyRaised("NumberOfRepeating"); } }
        public bool IsNotRunTest { get { return isNotRunTest; } set { isNotRunTest = value; OnPropertyRaised("IsNotRunTest"); } }
        private bool isNotRunTest = true;
        public string ColorColumn { get { return model.ColorColumnModel; } set { model.ColorColumnModel = value; OnPropertyRaised("ColorColumnBinding"); } }

        public string CPUName { get { return model.CPUInfromation; } set { } }

        public MainWindowVievModel()
        {
            model = new MainWindowModel();
            RunTestCommand = new RelayCommand(new Action<object>(RunTest));
            LoadDiagramsCommand = new RelayCommand(new Action<object>(LoadData));
        }
        public async void RunTest(object obj)
        {
            if (model.CheckCorectOfDataToTest())
            {
                IsNotRunTest = false;
                await Task.Factory.StartNew(() => { model.Runtest(); });
                Score = model.scoreToShow;
                IsNotRunTest = true;
                ShowDiagram();
                model.LoadTests();
                Score = model.scoreToShow;
            }
            else
            {
                MessageBox.Show("Niepoprawny format danych do testu, popraw dane");
            }
        }

        public void LoadData(object obj)
        {
            model.LoadTests();
            Score = model.scoreToShow;
            // tutaj akacja by wyświetlić diagram XD
        }


        private void ShowDiagram()
        {
            var scoreToShow = model.score;
            var cpuInform = model.CPUInfromation;
            var parametr = model.Parametrs;

            //tutaj zapisać do bd można 
        }

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}

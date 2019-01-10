using System;
using System.Windows.Input;
namespace WindowAplication
{
    public class MainWindowVievModel
    {
        private ICommand runtest;
        private ICommand loadDiagrams;
        private readonly MainWindowModel model = new MainWindowModel();
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


        

        public MainWindowVievModel()
        {
            RunTestCommand = new RelayCommand(new Action<object>(RunTest));
            LoadDiagramsCommand = new RelayCommand(new Action<object>(LoadData));
        }
        public void RunTest(object obj)
        {
            model.Runtest();
            ShowDiagram();
        }
        
        public void LoadData(object obj)
        {
            // tutaj akacja by wyświetlić diagram XD
        }

        private void ShowDiagram()
        {
            
           var scoreToShow= model.score;
        }


    }
}

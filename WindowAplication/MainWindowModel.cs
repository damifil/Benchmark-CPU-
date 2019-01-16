using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Management;
using System.Windows;
using WindowAplication.Model;
using System.Windows.Controls.DataVisualization.Charting;

namespace WindowAplication
{
    class MainWindowModel
    {

        public MainWindowModel()
        {
            Parametrs = new TestParametr();
            GetCPUInformation();
            scoreToShow = new List<KeyValuePair<string, int>>();
        }
        public long[] score;

        public TestParametr Parametrs;
        public bool parallelTest = true;
        public string CPUInfromation = "";
        public System.Collections.Generic.List<KeyValuePair<string, int>> scoreToShow;
        public string ColorColumnModel = "#ff00ff";
        public string lastScore = "";

        public void Runtest()
        {
            score = new long[Parametrs.numberOfRepeating];
            DiggingEngine engine = new DiggingEngine();
            // Parallel.For(0, Parametrs.numberOfRepeating, i =>
            for (int i = 0; i < Parametrs.numberOfRepeating; i++)
            {
                if (parallelTest)
                {
                    score[i] = engine.DiggingTestParallel(Parametrs);
                    //MessageBox.Show(score[i].ToString());
                }
                else
                    score[i] = engine.DiggingTest(Parametrs);
            }//);

            SaveTest();
        }

        public void GetCPUInformation()
        {
            ManagementClass mgt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection procs = mgt.GetInstances();
            foreach (ManagementObject item in procs)
                CPUInfromation = item.Properties["Name"].Value.ToString();
          //  MessageBox.Show(CPUInfromation);
            if (String.IsNullOrEmpty(CPUInfromation))
            {
                CPUInfromation = "UNKNOWN";
            }
        }

        public void LoadTests()
        {
            FillDiagram();
        }

        private void FillDiagram()
        {
            using (Database1Entities db = new Database1Entities())
            {
                string nameTest = GetNameTest();
                var scores = db.Scores.Where(x => x.Test == nameTest);
                List<KeyValuePair<string, int>> scoresList = new List<KeyValuePair<string, int>>();

                foreach (var item in scores)
                {
                    string name = item.Name;
                    if(CPUInfromation == item.Name && lastScore == item.Score)
                    {
                        string tempName = name;
                        name = "[My] " + tempName;
                    }
                    scoresList.Add(new KeyValuePair<string, int>(name, Convert.ToInt32(item.Score)));
                }
                scoreToShow = scoresList;
            }
        }

        private long TotalTime()
        {
            long totalTime = 0;
            foreach (var item in score)
                totalTime += item;
            return totalTime;
        }

        private void SaveTest()
        {
            lastScore = TotalTime().ToString();

            scoreToShow = new System.Collections.Generic.List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>(CPUInfromation, (int)score.Sum())
            };
          
            using (Database1Entities db = new Database1Entities())
            {
                db.Scores.Add(new Scores {
                    Name = CPUInfromation,
                    Date = DateTime.Now,
                    Score = TotalTime().ToString(),
                    Test = GetNameTest()});
                db.SaveChanges();
            }
            
        }

        public string GetNameTest()
            => Parametrs.algorithm + " "
                    + Parametrs.stepToFInd + " "
                    + Parametrs.increasNoZTo + " "
                    + Parametrs.numberOfRepeating + " "
                    + Parametrs.numberOfZeroInBegin;

    }
}

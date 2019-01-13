using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Management;
using System.Windows;
using WindowAplication.Model;

namespace WindowAplication
{
    class MainWindowModel
    {

        public MainWindowModel()
        {
            Parametrs = new TestParametr();
            GetCPUInformation();
        }
        public long[] score;

        public TestParametr Parametrs;
        public bool parallelTest = true;
        public string CPUInfromation = "";
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
            using (Database1Entities db = new Database1Entities())
            {
                db.Scores.Add(new Scores {
                    Name = CPUInfromation,
                    Date = DateTime.Now,
                    Score = TotalTime().ToString(),
                    Test = Parametrs.algorithm + " " 
                        + Parametrs.increasNoZTo + " " 
                        + Parametrs.numberOfRepeating + " " 
                        + Parametrs.numberOfZeroInBegin + " " 
                        + Parametrs.valueToChangeTimeInSearch});
                db.SaveChanges();
            }
        }

    }
}

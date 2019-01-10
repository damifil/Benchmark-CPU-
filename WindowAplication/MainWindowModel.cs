using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using System.Management;
using System.Windows;

namespace WindowAplication
{
    class MainWindowModel
    {

        public MainWindowModel()
        {
            GetCPUInformation();
        }
        public long[] score;
        
        public TestParametr Parametrs = new TestParametr();
        public bool parallelTest = true;
        public string CPUInfromation = "";
        public void Runtest()
        {
            score = new long[Parametrs.numberOfRepeating];
            DiggingEngine engine = new DiggingEngine();
            for (int i = 0; i < Parametrs.numberOfRepeating; i++)
            {
                if (parallelTest)
                    score[i] = engine.DiggingTestParallel(Parametrs);
                else
                    score[i] = engine.DiggingTest(Parametrs);
            }
            // tutaj zapisać do jakiejś BD score
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
        public void SaveTest()
        {

        }

    }



}

using System;
using System.Threading.Tasks;

namespace Engine
{
    public class Engine
    {
        static void Main(string[] args)
        {
            DiggingEngine engine = new DiggingEngine();
         //   Console.WriteLine(engine.DiggingTestParallel(Algorithm.sha256));
          //  Console.WriteLine(engine.DiggingTest(Algorithm.sha256));

          var   score = new long[5];
            var parallelTest = true;
            Parallel.For(0, 5, i =>
            {
                if (parallelTest)
                {
                    Console.WriteLine("paralel " + i);
                    score[i] = engine.DiggingTestParallel(Algorithm.sha256d);
                    Console.WriteLine(i+" "+score[i]);
                    //MessageBox.Show(score[i].ToString());
                }
                else
                    score[i] = engine.DiggingTest(Algorithm.sha256d);
            });

            Console.ReadKey();

        }
    }
}

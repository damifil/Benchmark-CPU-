using System;

namespace Engine
{
    public class Engine
    {
        static void Main(string[] args)
        {
            DiggingEngine engine = new DiggingEngine();
            Console.WriteLine(engine.DiggingTestParallel(Algorithm.sha256));
            Console.WriteLine(engine.DiggingTest(Algorithm.sha256));
           
           
            
            Console.ReadKey();

        }
    }
}

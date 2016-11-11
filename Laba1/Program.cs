using System;
using System.Collections.Generic;
using System.IO;

namespace GA_Modified
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter file = new System.IO.StreamWriter("Result(MGA, N=100, C=1.0).txt");
            try
            {
                for (int i = 10; i <= 100; i+=10)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        ModifiedAlgorithm algorithm = new ModifiedAlgorithm(20, 8, 100, new int[] { -2, 2 }, new int[] { -2, 2 }, 1.0, (double)i / 100);
                        Tuple<double[], double, int, double> result = algorithm.start();
                        file.WriteLine("Experiment #{0}", i / 10);
                        file.WriteLine("Mutation probability: {0}", (double)i / 100);
                        file.WriteLine("Launch #{0}", j);
                        file.WriteLine("Fitness value: {0}", result.Item2);
                        file.WriteLine("Iterations count: {0}", result.Item3);
                        file.WriteLine("---------------------------------------------------------------");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Error occured: {0}", e.Message));
            }
            finally
            {
                file.Close();
            }
            file.Close();
            Console.WriteLine("Done!");
            Console.Read();
        }
    }
}













//file.WriteLine("Average Fitness Value: {0}", result.Item4);
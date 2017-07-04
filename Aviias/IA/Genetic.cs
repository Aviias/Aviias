using AForge;
using AForge.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias.IA
{
    class Genetic
    {
        static UserFunction userFunction = new UserFunction();
        static int populationSize = 10;
        static int chromosomeLength = 5; //(nb d'actions)
        static int iterations = 100;

        static void SearchSolution()
        {
            Population population = new Population(populationSize, new BinaryChromosome(chromosomeLength),
             userFunction, (ISelectionMethod)new RankSelection());
            // set optimization mode
            userFunction.Mode = OptimizationFunction1D.Modes.Maximization;

            int i = 1;
            // solution
            double[,] data = new double[populationSize, 2];

            DisplayData(data);

            while (i < iterations)
            {
                population.RunEpoch();      //run crossover(), mutate() and selection()

                // show current solution
                for (int j = 0; j < populationSize; j++)
                {
                    data[j, 0] = userFunction.Translate(population[j]);
                    data[j, 1] = userFunction.OptimizationFunction(data[j, 0]);
                }
                i++;
            }
            DisplayData(data);
        }

        static void DisplayData(double[,] data)
        {
            for (int i = 0; i < populationSize; i++)
            {
                Console.Write(data[i, 0] + " ");
                Console.WriteLine(data[i, 1]);
            }
        }
        /*
        static void Main(string[] args)
        {
            SearchSolution();
            Console.ReadLine();
        }
        */
    }

        public class UserFunction : OptimizationFunction1D      //calcule les scores
        {
            public UserFunction() : base(new Range(0, 255)) { }

            public override double OptimizationFunction(double x)
            {
                return Math.Cos(x / 23) * Math.Sin(x / 50) + 2;
            }
        }


    }

    



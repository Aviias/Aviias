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
        static int chromosomeLength = 5;             //[%action1, %action2,...]
        public static IChromosome chronos = new ShortArrayChromosome(chromosomeLength);
        internal Population Population = new Population(8, chronos, new FitnessRepartitionActions(), new EliteSelection());
        public static ShortArrayChromosome Meilleur = null;
        static internal int[] _points = new int[5];

        internal Genetic()
        {
            //RunGeneration();
        }

        static public void AddPoints(Monster monster)
        {
            _points = monster._points;
        }

        internal void RunGeneration()
        {
            Population.RunEpoch();
            for (int i = 0; i < Population.Size; i++)
            {
                Population[i].Evaluate(new FitnessRepartitionActions());
            }
            Meilleur = (ShortArrayChromosome)Population.BestChromosome;
        }
    }

    class FitnessRepartitionActions : IFitnessFunction
    {
        public double Evaluate(IChromosome chromosome)
        {
            int a = 0;
            int[] points = GetPoints(chromosome);
            // ushort[] genes = ((ShortArrayChromosome)chromosome).Value;
            for (int i = 0; i < points.Length; i++)
            {
                a += points[i];
             //   else a -= 2 * points[i];
            }
            if (a < 0) a = 0;
            return a;
        }

        static int[] GetPoints(IChromosome chromosome)
        {
           // ushort[] genes = ((ShortArrayChromosome)chromosome).Value;
            int[] points = new int[4];

           /* for (int i = 0; i < 4; i++)
            {
                points[i] = 2 * genes[i];
            }*/
            return Genetic._points;
        }
    }

    class RouletteEliteSelection : ISelectionMethod
    {
        public void ApplySelection(List<IChromosome> chromosomes, int size)
        {
            // On commence par reprendre systématiquement le meilleur chromosome
            List<IChromosome> NouvelleGeneration = new List<IChromosome>();
            double BestFitness = 0.0;
            foreach (IChromosome Chromosome in chromosomes)
                BestFitness = Math.Max(BestFitness, Chromosome.Fitness);

            IChromosome MeilleurChromosome = chromosomes.Find(delegate (IChromosome Chromosome)
            {
                return Chromosome.Fitness == BestFitness;
            });

            NouvelleGeneration.Add(MeilleurChromosome);
            double TotalDesFitness = 0.0;
            chromosomes.ForEach(delegate (IChromosome Chromosome)
            {
                TotalDesFitness += Chromosome.Fitness;
            });

            // Ensuite, on choisit au hasard le reste de la population, en donnant d'autant
            // plus de chance d'appartenir à la nouvelle génération que la fitness est élevée.
            Random Generateur = new Random(DateTime.Now.Second + DateTime.Now.Millisecond);
            while (--size > 0)
            {
                double PositionHasard = Generateur.NextDouble() * TotalDesFitness;
                double FitnessCumulee = 0.0;
                foreach (IChromosome Chromosome in chromosomes)
                {
                    FitnessCumulee += Chromosome.Fitness;
                    if (FitnessCumulee > PositionHasard)
                    {
                        NouvelleGeneration.Add(Chromosome);
                        break;
                    }
                }
            }

            chromosomes.Clear();
            chromosomes.AddRange(NouvelleGeneration);
        }
    }
}


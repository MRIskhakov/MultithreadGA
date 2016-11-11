using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_Modified
{
    class ModifiedAlgorithm: BaseAlgorithm
    {
        public ModifiedAlgorithm(int endIterationsCount, int precision, int populationsCount, int[] xBounds, int[] yBounds, double mutationProbability, double crossingoverProbability) :
            base(endIterationsCount, precision, populationsCount, xBounds, yBounds, mutationProbability, crossingoverProbability)
        {
        }

        protected override void crossingover()
        {
            List<Chromosome> parentsCopy = new List<Chromosome>(this.parents);



            for (int i = 0; i < parentsCopy.Count; i++)
            {
                Chromosome parent1 = parentsCopy[0];
                Chromosome parent2 = null;
                double p1 = fitnessFunction(parent1);
                double min1 = 0;
                double min2 = 0;
                double p2 = fitnessFunction(parentsCopy[1]);
                min1 = Math.Abs(p1 - p2);
                parent2 = parentsCopy[1];
                for (int j = 2; j < parentsCopy.Count; j++)
                {
                    p2 = fitnessFunction(parentsCopy[j]);
                    min2 = Math.Abs(p1 - p2);

                    if (min1 < min2)
                    {
                        parent2 = parentsCopy[j];
                    }
                }

                parentsCopy.Remove(parent1);
                parentsCopy.Remove(parent2);
                int crossPoint1 = this.random.Next(0, parent1.chromosomeLength - 3);
                int crossPoint2 = this.random.Next(crossPoint1, parent1.chromosomeLength - 2);
                int crossPoint3 = this.random.Next(crossPoint2, parent1.chromosomeLength - 1);
                int crossPoint4 = this.random.Next(crossPoint3, parent1.chromosomeLength);

                
                    Chromosome crossChromosome1 = Chromosome.getCrossedChromosome(parent1, parent2, crossPoint1, crossPoint2, crossPoint3, crossPoint4);
                    Chromosome crossChromosome2 = Chromosome.getCrossedChromosome(parent2, parent1, crossPoint1, crossPoint2, crossPoint3, crossPoint4);
                    
                        this.childrens.Add(crossChromosome1);
                         this.childrens.Add(crossChromosome2);



            }





        }
        //One point inversion
        protected override void mutation()
        {
            {
                
                this.childrens.ForEach(chromosome =>
                {
                    if (isProbable(this.mutationProbability))
                    {
                        int randomValue1 = random.Next(0, chromosome.chromosomeLength-2);
                        chromosome.xGens[randomValue1] = chromosome.xGens[randomValue1] == 0 ? 1 : 0;
                        randomValue1 = random.Next(0, chromosome.chromosomeLength);
                        chromosome.yGens[randomValue1] = chromosome.yGens[randomValue1] == 0 ? 1 : 0;
                        int randomValue2 = random.Next(randomValue1, chromosome.chromosomeLength-1);
                        chromosome.xGens[randomValue2] = chromosome.xGens[randomValue2] == 0 ? 1 : 0;
                        randomValue2 = random.Next(randomValue2, chromosome.chromosomeLength);
                        chromosome.yGens[randomValue2] = chromosome.yGens[randomValue2] == 0 ? 1 : 0;
                        int randomValue3 = random.Next(randomValue2, chromosome.chromosomeLength);
                        chromosome.xGens[randomValue3] = chromosome.xGens[randomValue3] == 0 ? 1 : 0;
                        randomValue3 = random.Next(randomValue3, chromosome.chromosomeLength);
                        chromosome.yGens[randomValue3] = chromosome.yGens[randomValue3] == 0 ? 1 : 0;

                    }
                });
            }
        }

        
        protected override void selection()
        {
            

            List<Chromosome> result = new List<Chromosome>();
            List<Chromosome> par = new List<Chromosome>();
            List<Chromosome> chil = new List<Chromosome>();
            result.AddRange(this.parents);
            result.AddRange(this.childrens);


            result.ForEach(chromosome => chromosome.fitnessValue = this.fitnessFunction(chromosome));

            

            for (int i = 0; i < this.initialPopulationCount; i++)
            {
               
               int value = random.Next(0, 100);
                int value1 = random.Next(0, 68);
                if (fitnessFunction(parents[value]) < fitnessFunction(childrens[value1]))
                    parents[value] = childrens[value1];
            }
            this.childrens.Clear();
        }
    }
}

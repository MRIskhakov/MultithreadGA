using System;
using System.Collections.Generic;

namespace GA_Modified
{
    interface IExchangeable
    {
        Chromosome getBest();
        Chromosome getWorst();
    }

    public class BaseAlgorithm: IExchangeable
    {
        protected List<Chromosome> parents;
        protected List<Chromosome> childrens = new List<Chromosome>();
        protected Random random = new Random();

        //Algorithm initial values
        protected int initialPopulationCount;
        protected int endIterationsCount;
        protected int xMin;
        protected int yMin;
        protected int xMax;
        protected int yMax;
        protected int precision;
        protected double mutationProbability;
        protected double crossingoverProbability;

        public BaseAlgorithm(int endIterationsCount, int precision, int populationsCount, int[] xBounds, int[] yBounds, double mutationProbability, double crossingoverProbability)
        {
            if (xBounds.Length != 2 || yBounds.Length != 2)
                throw new Exception("Wrong algorithm initial parameters");
            if (endIterationsCount <= 0)
                throw new Exception("Wrong endIterationsCount value");
            this.initialPopulationCount = populationsCount;
            this.endIterationsCount = endIterationsCount;
            this.parents = new List<Chromosome>(populationsCount);
            this.xMin = xBounds[0];
            this.xMax = xBounds[1];
            this.yMin = yBounds[0];
            this.yMax = yBounds[1];
            this.precision = precision;
            this.crossingoverProbability = crossingoverProbability;
            this.mutationProbability = mutationProbability;
        }

        public Chromosome getBest()
        {
            return null;
        }

        public Chromosome getWorst()
        {
            return null;
        }

        protected double fitnessFunction(Chromosome chromosome)
        {
            double xValue = chromosome.convertValue(chromosome.xGens);
            double x = this.xMin + (this.xMax - this.xMin) * (xValue / Math.Pow(2, this.precision) - 1);
            double yValue = chromosome.convertValue(chromosome.yGens);
            double y = this.yMin + (this.yMax - this.yMin) * (yValue / Math.Pow(2, this.precision) - 1);

            return (Math.Cos(x * x) + Math.Cos(y * y)) - (1 / Math.Pow(2, Math.Pow((5 * x) * y, 5)));
        }

        protected virtual void crossingover()
        {
            for (int i = 0; i < this.parents.Count; i++)
            {
                Chromosome children1;
                Chromosome children2;

                int firstParentIndex = random.Next(0, this.parents.Count);
                int secondParentIndex;

                do
                {
                    secondParentIndex = random.Next(0, this.parents.Count);
                } while (firstParentIndex == secondParentIndex);

                Chromosome parent1 = this.parents[firstParentIndex];
                Chromosome parent2 = this.parents[secondParentIndex];

                int crossPoint = this.random.Next(0, parent1.chromosomeLength);

                if (isProbable(this.crossingoverProbability))
                {
                    children1 = Chromosome.concat(parent1, parent2, crossPoint);
                    children2 = Chromosome.concat(parent2, parent1, crossPoint);

                    if (isProbable(0.5))
                    {
                        this.childrens.Add(children1);
                    }
                    else
                    {
                        this.childrens.Add(children2);
                    }
                }
            }
        }

        protected virtual void mutation()
        {
            //this.parents.ForEach(chromosome => chromosome.mutate());
            this.childrens.ForEach(chromosome =>
                {
                    if (isProbable(this.mutationProbability))
                    {
                        int randomValue = random.Next(0, chromosome.chromosomeLength);
                        chromosome.xGens[randomValue] = chromosome.xGens[randomValue] == 0 ? 1 : 0;
                        randomValue = random.Next(0, chromosome.chromosomeLength);
                        chromosome.yGens[randomValue] = chromosome.yGens[randomValue] == 0 ? 1 : 0;
                    }
                });
        }

        protected virtual void selection()
        {
            List<Chromosome> result = new List<Chromosome>();
            result.AddRange(this.parents);
            result.AddRange(this.childrens);

            result.ForEach(chromosome => chromosome.fitnessValue = this.fitnessFunction(chromosome));

            result.Sort((chromosome1, chromosome2) => (chromosome1.fitnessValue.CompareTo(chromosome2.fitnessValue)));

            result.RemoveRange(0, result.Count - this.initialPopulationCount);

            this.parents = result;
            this.childrens.Clear();
        }

        protected void initialization()
        {
            for (int i = 0; i < this.initialPopulationCount; i++)
            {
                Chromosome chromosome = new Chromosome(this.precision);
                this.parents.Add(chromosome);
            }
        }

        public Tuple<double[], double, int, double> start(Action<int> callback)
        {
            this.initialization();

            double averageFitnessValue = 0;
            int prevIterationsCount = 0;
            double epsilon = 0.05;

            int i = 0;
            do {
                i++;

                callback(i);

                this.crossingover();
                this.mutation();
                this.selection();

                double currentAverage= 0.0;
                this.parents.ForEach(chromosome => currentAverage += chromosome.fitnessValue);
                currentAverage /= this.initialPopulationCount;
                if (Math.Abs(averageFitnessValue - currentAverage) < epsilon)
                {
                    prevIterationsCount++;
                }
                else
                {
                    prevIterationsCount = 0;
                }
                averageFitnessValue = currentAverage;

            } while(prevIterationsCount < endIterationsCount);

            double xValue = this.parents[0].convertValue(this.parents[0].xGens);
            double x = this.xMin + (this.xMax - this.xMin) * (xValue / Math.Pow(2, this.precision) - 1);
            double yValue = this.parents[0].convertValue(this.parents[0].yGens);
            double y = this.yMin + (this.yMax - this.yMin) * (yValue / Math.Pow(2, this.precision) - 1);

            return new Tuple<double[], double, int, double>(new double[] { x, y }, this.parents[this.initialPopulationCount-1].fitnessValue, i, averageFitnessValue);
        }

        protected bool isProbable(double probability)
        {
            double value = (double)random.Next(0, 1000)/1000;
            return value <= probability;
        }
    }
}

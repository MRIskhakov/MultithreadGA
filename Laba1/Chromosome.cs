using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_Modified
{
    public class Chromosome
    {
        public List<int> xGens;
        public List<int> yGens;
        public int chromosomeLength;
        private Random rand = new Random();
        public double fitnessValue = 0;

        public Chromosome(int precision)
        {
            this.chromosomeLength = precision;
            this.xGens = new List<int>();
            this.yGens = new List<int>();
            this.initialize();
        }

        public Chromosome(int precision, List<int> xGens, List<int> yGens)
        {
            if (xGens.Count != precision && yGens.Count != precision)
                throw new Exception("Can't concat two chromosomes with different lengths of X and Y gens");
            this.chromosomeLength = precision;
            this.xGens = xGens;
            this.yGens = yGens;
        }

        public static Chromosome concat(Chromosome chromosome1, Chromosome chromosome2, int crossPoint)
        {
            if (chromosome1.chromosomeLength != chromosome2.chromosomeLength)
                throw new Exception("Can't concat two chromosomes with different lengths");
            List<int> xGens = chromosome1.xGens.GetRange(0, crossPoint + 1);
            xGens.AddRange(chromosome2.xGens.GetRange(crossPoint + 1, chromosome2.xGens.Count - crossPoint - 1));

            List<int> yGens = chromosome1.yGens.GetRange(0, crossPoint + 1);
            yGens.AddRange(chromosome2.yGens.GetRange(crossPoint + 1, chromosome2.yGens.Count - crossPoint - 1));

            return new Chromosome(chromosome1.chromosomeLength, xGens, yGens);
        }

        public static Chromosome concat(Chromosome chromosome1, Chromosome chromosome2, int firstCrossPoint, int secondCrossPoint)
        {
            if (chromosome1.chromosomeLength != chromosome2.chromosomeLength)
                throw new Exception("Can't concat two chromosomes with different lengths");

            List<int> xFirstGens = chromosome1.xGens.GetRange(0, firstCrossPoint + 1);
            List<int> xLastGens = chromosome1.xGens.GetRange(secondCrossPoint + 1, chromosome1.chromosomeLength-secondCrossPoint-1);
            List<int> xMiddleGens = chromosome2.xGens.GetRange(firstCrossPoint + 1, secondCrossPoint-firstCrossPoint);
            List<int> xGens = new List<int>();
            xGens.AddRange(xFirstGens);
            xGens.AddRange(xMiddleGens);
            xGens.AddRange(xLastGens);

            List<int> yFirstGens = chromosome1.xGens.GetRange(0, firstCrossPoint + 1);
            List<int> yLastGens = chromosome1.xGens.GetRange(secondCrossPoint + 1, chromosome1.chromosomeLength - secondCrossPoint - 1);
            List<int> yMiddleGens = chromosome2.yGens.GetRange(firstCrossPoint + 1, secondCrossPoint - firstCrossPoint);
            List<int> yGens = new List<int>();
            yGens.AddRange(yFirstGens);
            yGens.AddRange(yMiddleGens);
            yGens.AddRange(yLastGens);

            return new Chromosome(chromosome1.chromosomeLength, xGens, yGens);
        }

        public static Chromosome getCrossedChromosome(Chromosome chromosome1, Chromosome chromosome2,
    int crossPoint1, int crossPoint2, int crossPoint3, int crossPoint4)
        {
            List<int> xGens = chromosome1.xGens.GetRange(0, crossPoint1 + 1);
            xGens.AddRange(chromosome2.xGens.GetRange(crossPoint1 + 1, crossPoint2 - crossPoint1));
            xGens.AddRange(chromosome1.xGens.GetRange(crossPoint2 + 1, crossPoint3 - crossPoint2));
            xGens.AddRange(chromosome1.xGens.GetRange(crossPoint3 + 1, crossPoint4 - crossPoint3));
            xGens.AddRange(chromosome2.xGens.GetRange(crossPoint4 + 1, chromosome2.xGens.Count - crossPoint4 - 1));
           
            List<int> yGens = chromosome1.yGens.GetRange(0, crossPoint1 + 1);
            yGens.AddRange(chromosome2.yGens.GetRange(crossPoint1 + 1, crossPoint2 - crossPoint1));
            yGens.AddRange(chromosome1.yGens.GetRange(crossPoint2 + 1, crossPoint3 - crossPoint2));
            yGens.AddRange(chromosome1.yGens.GetRange(crossPoint3 + 1, crossPoint4 - crossPoint3));
            yGens.AddRange(chromosome2.yGens.GetRange(crossPoint4 + 1, chromosome2.yGens.Count - crossPoint4 - 1));

            return new Chromosome(chromosome1.chromosomeLength, xGens, yGens);
        }

        public double convertValue(List<int> values)
        {
            double resultValue = 0.0;
            for (int i = 0; i < this.chromosomeLength; i++)
            {
                resultValue += values[i] * Math.Pow(2, values.Count - i - 1);
            }
            return resultValue;
        }

        private void initialize()
        {
            for (int i = 0; i < this.chromosomeLength; i++)
            {
                this.xGens.Add(this.rand.Next(0, 2));
                this.yGens.Add(this.rand.Next(0, 2));
            }
        }

        public static int hemmingValue(Chromosome chromosome1, Chromosome chromosome2)
        {
            int distance = 0;
            for (int i = 0; i < chromosome1.chromosomeLength; i++)
            {
                distance += chromosome1.xGens[i] == chromosome2.xGens[i] ? 0 : 1;
                distance += chromosome1.yGens[i] == chromosome2.yGens[i] ? 0 : 1;
            }
            return distance;
        }
    }
}

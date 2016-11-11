using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GA_Modified;

namespace Laba1.GA_Models
{
    public delegate void SyncCallback<T>(WorkerItem<T> worker) where T: BaseAlgorithm;

    public class WorkerItem<T> where T : BaseAlgorithm
    {
        private Thread thread;
        private T      algorithm;
        private int    syncPeriod;
        private int    iterationsCount;

        Chromosome getBestChromosome()
        {
            return this.algorithm.getBest();
        }

        Chromosome getWorstChromosome()
        {
            return this.algorithm.getWorst();
        }

        public WorkerItem(int syncPeriod, int iterationsCount)
        {
            this.syncPeriod = syncPeriod;
            this.iterationsCount = iterationsCount;

            thread = new Thread(start);
            thread.Start();
        }

        public void start(object param)
        {
            SyncCallback<T> callback = (SyncCallback<T>)param;

            this.algorithm.start((iteration) =>
            {
                if (iteration % this.syncPeriod == 0)
                {
                    callback(this);
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GA_Modified;
using Laba1.GA_Models;

namespace Laba1.GA_Models
{
    public class BaseModel<T> where T: BaseAlgorithm
    {
        protected List<WorkerItem<T>> workerItems;
        protected Barrier _barrier;

        BaseModel(int itemsCount, int syncPeriod, int iterationsCount)
        {
            _barrier = new Barrier(itemsCount, (barrier) =>
            {
                this.exchange();
            });

            for (int i = 0; i < itemsCount; i++)
            {
                var workerItem = new WorkerItem<T>(syncPeriod, iterationsCount);
                workerItems.Add(workerItem);
                workerItem.start((worker) =>
                {
                    this._barrier.SignalAndWait();
                });
            }
        }

        protected virtual void exchange()
        {

        }
    }
}

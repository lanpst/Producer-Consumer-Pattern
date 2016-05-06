using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPOS.DesignPattern.ProducerConsumer
{
    public abstract class Consumer<T>
    {
        public abstract void Consume(T produce);

        internal void StartConsume(T product)
        {
            Consume(product);

            this.isBusy = false;
        }
        bool isBusy = false;
        internal bool IsBusy
        {
            get { return isBusy; }
        }

        internal void SetBusy()
        {
            this.isBusy = true;
        }
    }
}

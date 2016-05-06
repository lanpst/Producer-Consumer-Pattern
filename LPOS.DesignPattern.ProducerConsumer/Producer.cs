using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPOS.DesignPattern.ProducerConsumer
{
    public interface IProducer<T>
    {
        ProducerConsumerStation<T>.ProduceHandler Produce { get; }
        string Name { get; }
    }

    internal class Producer<T> : IProducer<T>
    {
        readonly string name;
        public string Name
        {
            get { return name; }
        }
        public Producer(string name)
        {
            this.name = name;
        }
        ProducerConsumerStation<T>.ProduceHandler produce;

        public ProducerConsumerStation<T>.ProduceHandler Produce
        {
            get { return produce; }
            internal set { produce = value; }
        }
    }
}

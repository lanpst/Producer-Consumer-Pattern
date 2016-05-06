using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPOS.DesignPattern.ProducerConsumer
{
    public class ProducerConsumerStation<T>
    {
        System.Collections.Concurrent.ConcurrentQueue<T> queue;

        public delegate void ProduceHandler(T product);

        List<IProducer<T>> producers;

        List<Consumer<T>> consumers;

        public ProducerConsumerStation()
        {
            producers = new List<IProducer<T>>();

            consumers = new List<Consumer<T>>();

            queue = new System.Collections.Concurrent.ConcurrentQueue<T>();
        }

        public void AddConsumer(Consumer<T> consumer)
        {
            consumers.Add(consumer);
        }

        public IProducer<T> NewProducer(string name)
        {
            var producer = new Producer<T>(name);

            producer.Produce = new ProduceHandler(Produce);

            return producer;
        }

        private void Produce(T product)
        {
            queue.Enqueue(product);

            TryConsume();
        }

        bool isTrying = false;
        private void TryConsume()
        {
            if (!isTrying)
            {
                isTrying = true;

                var actTryConsume = new Action(() =>
                {
                    while (queue.Count > 0)
                    {
                        var freeConsumers = consumers.Where(q => !q.IsBusy);

                        if (freeConsumers.Count() > 0)
                        {
                            T product;

                            if (queue.TryDequeue(out product))
                            {
                                var consumer = freeConsumers.First();

                                consumer.SetBusy();

                                var actConsume = new Action<T>(consumer.StartConsume);

                                actConsume.BeginInvoke(product, (ar) =>
                                {
                                    (ar.AsyncState as Action<T>).EndInvoke(ar);
                                }, actConsume);
                            }
                        }
                        System.Threading.Thread.Sleep(0);
                    }
                });
                actTryConsume.BeginInvoke((ar) =>
                {
                    (ar.AsyncState as Action).EndInvoke(ar);

                    isTrying = false;

                }, actTryConsume);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            producer.Produce = new ProduceHandler(this.Produce);

            return producer;
        }

        private void Produce(T product)
        {
            System.Console.WriteLine($"Producer with {product.ToString()}"
                + "\t" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            queue.Enqueue(product);

            TryMethod();
        }
        
        object lockObj = new object();

        private async Task TryMethod()
        {
            await Task.Run(() =>
            {
                if (System.Threading.Monitor.TryEnter(lockObj))
                {
                    System.Console.WriteLine($"lock on {queue.Count}");

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

                                consumer.StartConsume(product);
                            }
                        }
                        System.Threading.Thread.Sleep(0);
                    }

                    System.Console.WriteLine($"lock off {queue.Count}");
                    System.Threading.Monitor.Exit(lockObj);
                }
            });
            
        }
    }
}

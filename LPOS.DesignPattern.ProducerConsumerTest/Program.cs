using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPOS.DesignPattern.ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ProducerConsumerPatternTester tester = new ProducerConsumerPatternTester();

            tester.StartTest();

            System.Console.ReadKey();
        }
    }
    public class LocalConsumer : Consumer<int>
    {
        string name;
        public LocalConsumer(string name)
        {
            this.name = name;
        }

        public override void Consume(int produce)
        {
            System.Console.WriteLine($"Start Consumer {this.name.ToString()} with {produce}"
                    + "\t" + System.Threading.Thread.CurrentThread.ManagedThreadId);

            System.Threading.Thread.Sleep(1000);

            System.Console.WriteLine($"Finish Consumer {this.name.ToString()} with {produce}"
                    + "\t" + System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }

    public class ProducerConsumerPatternTester
    {
        ProducerConsumerStation<int> pcp;
        public void StartTest()
        {
            pcp = new ProducerConsumerStation<int>();
            pcp.AddConsumer(new LocalConsumer("consumer1"));
            pcp.AddConsumer(new LocalConsumer("consumer2"));
            pcp.AddConsumer(new LocalConsumer("consumer3"));

            ProduceAsync(pcp.NewProducer("producer1"), 1);
            
            ProduceAsync(pcp.NewProducer("producer2"), 1000);

        }

        private async Task ProduceAsync(IProducer<int> producer, int startValue)
        {
            Task.Run(() =>
            {
                for (int i = 1; i <= 50; i++)
                {
                    producer.Produce(i * startValue);

                    System.Threading.Thread.Sleep(100);
                }
            });
        }
    }
}

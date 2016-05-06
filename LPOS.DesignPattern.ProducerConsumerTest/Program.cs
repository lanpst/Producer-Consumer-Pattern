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

        public class ProducerConsumerPatternTester
        {
            ProducerConsumerStation<int> pcp;
            public void StartTest()
            {
                pcp = new ProducerConsumerStation<int>();
                pcp.AddConsumer(new LocalConsumer("consumer1"));
                pcp.AddConsumer(new LocalConsumer("consumer2"));
                pcp.AddConsumer(new LocalConsumer("consumer3"));
                pcp.AddConsumer(new LocalConsumer("consumer4"));
                var producer1 = pcp.NewProducer("producer1");
                var producer2 = pcp.NewProducer("producer2");

                var act = new Action<IProducer<int>, int>((IProducer<int> p, int q) =>
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        p.Produce(i * q);

                        System.Console.WriteLine($"Producer ({p.Name}) with {i * q}");

                        System.Threading.Thread.Sleep(100);
                    }
                });

                act.BeginInvoke(producer1, 1, null, null);

                act.BeginInvoke(producer2, 100, null, null);

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
                System.Threading.Thread.Sleep(2000);

                System.Console.WriteLine($"Consumer ({name}) with {produce.ToString()}");
            }
        }
    }
}

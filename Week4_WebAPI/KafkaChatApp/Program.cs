using System;
using System.Threading.Tasks;
using Confluent.Kafka;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Enter '1' for Producer, '2' for Consumer:");
        var choice = Console.ReadLine();

        var topic = "testchat";

        if (choice == "1")
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            while (true)
            {
                Console.Write("You: ");
                var message = Console.ReadLine();

                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
            }
        }
        else if (choice == "2")
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "chat-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);

            Console.WriteLine("Listening for messages...");
            while (true)
            {
                var result = consumer.Consume();
                Console.WriteLine($"Friend: {result.Message.Value}");
            }
        }
    }
}


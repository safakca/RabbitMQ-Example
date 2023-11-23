// See https://aka.ms/new-console-template for more information
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");

ConnectionFactory factory = new()
{
    Uri = new Uri("amqps://...")
};

using var connection = factory.CreateConnection();
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "rabbitmq-example", durable: false, exclusive: false, autoDelete: false, arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, e) =>
    {
        var body = e.Body.Span;
        var message = Encoding.UTF8.GetString(body);
        Customer customer = JsonConvert.DeserializeObject<Customer>(message);
        Console.WriteLine($"Adı: {customer.Name} Soyadı: {customer.Surname} [{customer.Message}]");
    };

    channel.BasicConsume(queue: "rabbitmq-example", autoAck: true, consumer: consumer);

    Console.WriteLine("Mesajınız başarılı bir şekilde ulasti ");
    Console.ReadLine();
}


public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Message { get; set; }
}

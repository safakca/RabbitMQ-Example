// See https://aka.ms/new-console-template for more information
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");

Customer customer = new()
{
    Id=1,
    Name="Merit",
    Surname="Cos",
    Message="Siparişiniz Yolda"
};


ConnectionFactory connectionFactory = new()
{
    Uri = new Uri("amqps://geilrjnf:CFHBVh0ApbH-4IfPGkSEB7-hUFVg1wQK@sparrow.rmq.cloudamqp.com/geilrjnf")
};

using IConnection connection = connectionFactory.CreateConnection();
using (IModel channel = connection.CreateModel())
{
    channel.QueueDeclare(queue:"rabbitmq-example", durable: false, exclusive:false, autoDelete: false, arguments: null);

    var data = JsonConvert.SerializeObject(customer);
    var byteData = Encoding.UTF8.GetBytes(data);
    channel.BasicPublish(exchange : string.Empty, routingKey : "rabbitmq-example", basicProperties : null, body: byteData);
}

Console.WriteLine("Mesaj basari ile yayinlandi");
Console.ReadKey();

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Message { get; set; }
}

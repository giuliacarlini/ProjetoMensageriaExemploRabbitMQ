using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

Console.WriteLine("[*] Aguardando mensagens.");

var consumidor = new EventingBasicConsumer(channel);

consumidor.Received += (model, ea) =>
{
    var corpo = ea.Body.ToArray();
    var mensage = Encoding.UTF8.GetString(corpo);

    Console.WriteLine($"[x] Recebido: {mensage}");
};

channel.BasicConsume(queue: "hello",
    autoAck: true,
    consumer: consumidor);

Console.WriteLine(" Aperte [enter] para sair.");
Console.ReadLine();
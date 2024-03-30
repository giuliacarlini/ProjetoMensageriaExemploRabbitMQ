using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

var mensagem = GetMessage(args);

var corpo = Encoding.UTF8.GetBytes(mensagem);

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: "hello",
    basicProperties: null,
    body: corpo);

Console.WriteLine($" [x] Enviado {mensagem}");


string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Mensagem vazia");
}
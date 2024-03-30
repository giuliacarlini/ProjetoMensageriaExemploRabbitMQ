using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue_2",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

var mensagem = GetMessage(args);

var corpo = Encoding.UTF8.GetBytes(mensagem);
var propriedades = channel.CreateBasicProperties();
propriedades.Persistent = true;  //persistindo mensagem mesmo que o servidor rabbit dê algum problema

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: "task_queue_2",
    basicProperties: propriedades, //persistindo mensagem mesmo que o servidor rabbit dê algum problema
    body: corpo);

Console.WriteLine($" [x] Enviado {mensagem}");


string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Mensagem vazia");
}
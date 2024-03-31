using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

var severity = (args.Length > 0) ? args[0] : "info";
var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World";

var corpo = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: "direct_logs",
    routingKey: severity,
    basicProperties: null,
    body: corpo);

Console.WriteLine($" [x] Enviado '{severity}': '{message}'");

string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Mensagem vazia");
}
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var exchange = "topic_logs";

channel.ExchangeDeclare(exchange, ExchangeType.Topic);

var routingKey = (args.Length > 0) ? args[0] : "anonimo.info";
var message = (args.Length > 1) ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World";

var corpo = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: exchange,
    routingKey: routingKey,
    basicProperties: null,
    body: corpo);

Console.WriteLine($" [x] Enviado '{routingKey}': '{message}'");

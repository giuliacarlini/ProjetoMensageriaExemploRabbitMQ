using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("topic_logs", ExchangeType.Topic);

var queueName = channel.QueueDeclare().QueueName;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                            Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

foreach (var bindingKey in args)
{
    channel.QueueBind(queue: queueName,
                      exchange: "topic_logs",
                      routingKey: bindingKey);
}

Console.WriteLine("[*] Aguardando mensagens.");

var consumidor = new EventingBasicConsumer(channel);

consumidor.Received += (model, ea) =>
{
    var corpo = ea.Body.ToArray();
    var mensagem = Encoding.UTF8.GetString(corpo);

    var routingKey = ea.RoutingKey;

    Console.WriteLine($"[x] Recebido: '{routingKey}':'{mensagem}'");

};

channel.BasicConsume(queue: queueName,
    autoAck: true,
    consumer: consumidor);

Console.WriteLine(" Aperte [enter] para sair.");
Console.ReadLine();
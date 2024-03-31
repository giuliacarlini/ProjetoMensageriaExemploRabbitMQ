using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("logs", ExchangeType.Fanout);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName,
                  exchange: "logs",
                  routingKey: string.Empty);


Console.WriteLine("[*] Aguardando mensagens.");

var consumidor = new EventingBasicConsumer(channel);

consumidor.Received += (model, ea) =>
{
    var corpo = ea.Body.ToArray();
    var mensagem = Encoding.UTF8.GetString(corpo);

    Console.WriteLine($"[x] Recebido: {mensagem}");

    int dots = mensagem.Split( '.' ).Length - 1;
    Thread.Sleep( dots * 1000);
    Console.WriteLine("[x] Concluído");
};

channel.BasicConsume(queue: queueName,
    autoAck: true,
    consumer: consumidor);

Console.WriteLine(" Aperte [enter] para sair.");
Console.ReadLine();
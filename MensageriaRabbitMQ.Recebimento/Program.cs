using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue_2",
                        durable: true, //garante a permanencia da mensagem na exchange caso o servidor rabbit quebre por algum motivo
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); //processar apenas uma mensagem por vez

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

    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //linha essencial para dizer que a mensagem foi entregue
                                                                    //com sucesso e para remoção da fila
};

channel.BasicConsume(queue: "task_queue_2",
    autoAck: false, //ack como falso significa que ao cair no recebimento, não vai dizer que a mensagem foi entregue com sucesso
    consumer: consumidor);

Console.WriteLine(" Aperte [enter] para sair.");
Console.ReadLine();
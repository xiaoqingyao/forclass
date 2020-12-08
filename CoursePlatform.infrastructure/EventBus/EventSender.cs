using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Notes.infrastructure.EventBus
{
    public class EventSender : ISender
    {

        private readonly IOptions<EventBusOptions> _opt;

        public const string NotesObjectType = "Notes_Event_Type";


        public void SendAsync<T>(T events) where T : INotesEvent
        {

            
            var factory = new ConnectionFactory() { HostName = this._opt.Value.HostName
                                                   , UserName = this._opt.Value.UserName
                                                    , Password = this._opt.Value.Password
                                                    , VirtualHost = this._opt.Value.VirtualHost};

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: this._opt.Value.Exchange, type: ExchangeType.Topic);


            var prop = channel.CreateBasicProperties();

            prop.AppId = this._opt.Value.AppId;
            prop.Headers.Add(NotesObjectType, events.GetType().AssemblyQualifiedName);
            prop.ContentType = "application/json";



            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(events));



            channel.BasicPublish(exchange: "logs",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);


        }

    }
}

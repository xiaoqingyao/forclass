using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Notes.infrastructure.EventBus
{
    public class EventBusOptions
    {
        public const string SectionName = "EventBus";


        public string Exchange { get; set; }


        public string AppId { get; set; }


        public string HostName { get; set; }


        public string UserName { get; set; }


        public string Password { get; set; }


        public int Port { get; set; }

        public string VirtualHost { get; set; }
    }



}

using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events
{
    public class EventSender : IEventSender
    {


        private readonly ICapPublisher _capBus;

        public EventSender(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        public Task SendAsync<T>(T data, string name = null) where T : ICoursePlatformEvent
        {

            if (String.IsNullOrEmpty(name))
            {

                name = typeof(T).Name;

            }


            return this._capBus.PublishAsync(name, data);
        }


        public async Task SendAsync<T>(IList<T> data, string name = null) where T : ICoursePlatformEvent
        {

            foreach (var item in data)
            {
                string tname = item.GetType().Name;
               await  this.SendAsync(item, tname);
            }
           
        }



        //public Task SendBatchAsync<T>(IList<T> data, string name) 
        //    where T : INotesEvent
        //{
        //    if (data == null || data.Count == 0)
        //    {
        //        return;
        //    }
        //    foreach (var item in data)
        //    {
        //        this.SendAsync(item, name);
        //    }
        //}

    }
}

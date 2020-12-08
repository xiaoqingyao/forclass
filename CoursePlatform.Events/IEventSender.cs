using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Events
{
    public interface IEventSender
    {
        Task SendAsync<T>(T data, string name = null) where T : ICoursePlatformEvent;


        Task SendAsync<T>(IList<T> data, string name = null) where T : ICoursePlatformEvent;


    }
}

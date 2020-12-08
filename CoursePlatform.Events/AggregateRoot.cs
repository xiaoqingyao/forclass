using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;

namespace CoursePlatform.Events
{
    public abstract class AggregateRoot
    {

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string ID { get; set; }

        [JsonIgnore]
        public IMapper Mapper { get; set; }


        [JsonIgnore]
        public IList<ICoursePlatformEvent> Events { get; set; }


        protected void AddEvent(ICoursePlatformEvent @event)
        {
            if (this.Events == null)
            {
                this.Events = new List<ICoursePlatformEvent>();
            }

            Events.Add(@event);
        }


        public void Apply(params IDomainSetter[] setters)
        {
            foreach (var item in setters)
            {
                item.Set(this);
            }
        }


    }
}

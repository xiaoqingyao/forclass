using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Events
{
    public interface IDomainSetter
    {

        void Set(AggregateRoot aggregate);

    }
}

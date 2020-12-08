using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;



namespace CoursePlatform.Infrastructure.Serializer
{
    public interface ISerializer
    {

        string Serial(object obj);


        T Deserial<T>(string source);

        Object Deserial(string source, Type type);

    }
}

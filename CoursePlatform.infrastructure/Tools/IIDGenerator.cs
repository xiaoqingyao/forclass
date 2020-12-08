using Flakey;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.infrastructure.Tools
{
    public interface IIDTools
    {
        string ID();
    }


    /// <summary>
    /// IdGenerator from github https://github.com/joshclark/Flakey
    /// </summary>
    public class FlakyId : IIDTools
    {



        public const string ConfigSectionName = "IDGenerator";



        public const string MathinIdName = "MachineId";


        public const string EpochTime = "EpochTime";

        //private int mationId;

        private IdGenerator ider;

        public FlakyId(int mationId, DateTime epochTime)
        {
            this.ider = new IdGenerator(mationId, epochTime);

           // this.ID = this.ider.CreateId();

        }



        public string ID()
        {
            return this.ider.CreateId().ToString();
        }
    }


}




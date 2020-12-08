using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoursePlatform.infrastructure
{
    public class StringHelper
    {

        private string _input;

        public StringHelper(string input)
        {
            this._input = input;
        }


        private string[] charAry;

        public String Get(int postion)
        {
            if (this.charAry == null || this.charAry.Length == 0)
            {
                return null;
            }
            if (this.charAry.Length -1 < postion)
            {
                return null;
            }
            return this.charAry[postion];
        }

        public T As<T>(int postion)
        {
            var str = this.Get(postion);
            if (String.IsNullOrEmpty(str))
            {
                return default;
            }
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(str); //(T)Convert.ChangeType(str, typeof(T));
        }


        public StringHelper Split(char c, StringSplitOptions opts = StringSplitOptions.RemoveEmptyEntries)
        {
            if (String.IsNullOrEmpty(_input))
            {
                return this;
            }
            this.charAry = this._input.Split(new char[] { c }, opts);
            return this;
        }

    }
}

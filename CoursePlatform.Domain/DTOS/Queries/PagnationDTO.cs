using System;
using System.Collections.Generic;
using System.Text;

namespace CoursePlatform.Domain.DTOS.Queries
{
    public class PagnationDTO
    {

        private int _pageIndex;

        public int PageIndex { 
        
            get
            {
                if (this._pageIndex <= 0)
                {
                    return 0;
                }
                return this._pageIndex - 1;
            }
            set
            {
                this._pageIndex = value;
            }
        }



        public int PageSize { get; set; }

    }
}

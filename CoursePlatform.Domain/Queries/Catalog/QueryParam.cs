using CoursePlatform.Asset.ApiDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Queries.Catalog
{
    public class QueryParam
    {
        public string CourseId { get; set; }

        public CatalogRequestParam Param { get; set; }
    }
}

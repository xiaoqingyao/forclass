using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS.Queries
{
    public class GetParam
    {

        [Required]
        public string Id { get; set; }
    }

    public class GetDsParam : GetParam
    {
        [Range(0, int.MaxValue)]
        public int CatalogId { get; set; }
    }
}

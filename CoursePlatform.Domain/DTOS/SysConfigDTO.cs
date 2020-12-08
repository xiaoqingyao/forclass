using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.DTOS
{

    public record  SysConfigDTO([Required]Guid Key, [Required]IList<TagAttrDTO> TagAttrs){}


    public record TagAttrDTO([Required]string Name, int Id);
}

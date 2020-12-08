using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoursePlatform.Domain.DTOS
{
    public class UserJoinDTO
    {
        [Required]
        public string CourseId { get; set; }
    }
}

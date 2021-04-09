using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SafeAdmin.Model
{
    public class Ethnicity
    {
        [Required]
        public int ID { get; set; }

        [MaxLength(20)]
        public string Value { get; set; }

    }
}

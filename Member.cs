using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SafeAdmin.Model
{
    public class Member
    {
        [Required]
        [JsonIgnore]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(64)]
        [JsonIgnore]
        public string Password { get; set; }

        [MaxLength(64)]
        [JsonIgnore]
        public string Salt { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(15)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(50)]
        public string Zip { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        public string Sex { get; set; }  //todo:  change to a type Sextype, maybe it's needed, becauses of radiobuttons 

        [JsonIgnore]
        public bool IsActive { get; set; }       
    }

    /*
    public class SexType
    {
        public string Male { get; set; }
        public string Female { get; set; }
        public string Other { get; set; }

    }
    */
}

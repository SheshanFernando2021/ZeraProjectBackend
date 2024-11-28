using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ZeraAPI.ZeraAPI.viewModels
{

    public class Register
    {
        [Key]
        public int RegisterId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        [DisplayName("Confirm Password")]
        public required string PasswordConfirmation { get; set; }
        public required string Address {get; set;}
    }


}
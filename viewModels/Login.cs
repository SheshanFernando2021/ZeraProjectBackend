using System;
using System.Collections.Generic;
using System.Linq;

namespace ZeraAPI.ZeraAPI.viewModels
{

    public class Login
    {
        public int LoginId {get; set;}
        public required string UserName {get; set;}
        public required string Password {get; set;}        
    }


}
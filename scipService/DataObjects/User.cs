using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scipService.DataObjects
{

    public class User : EntityData
    {
        public int UserID { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public int Date { get; set; }
        public List<uint> Sitter { get; set; }
    }
}
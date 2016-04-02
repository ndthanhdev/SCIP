using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCIPclient.Models
{
    public enum Gender
    {
        Other,
        Female,
        Male,
        Lesbian,
        Gay,
        Bisexual,
        Transgender
    }

    public class User
    {
        public string Id { get; set; }
        public uint UserID { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public Uri Avatar { get; set; }
        public int Date { get; set; }
        public List<uint> Sitter { get; set; }

    }
    
}

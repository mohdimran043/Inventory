using System;
using System.Collections.Generic;

namespace MOI.Patrol
{
    public partial class Aspnetusers
    {
        public Aspnetusers()
        {
            Aspnetuserclaims = new HashSet<Aspnetuserclaims>();
            Aspnetuserlogins = new HashSet<Aspnetuserlogins>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Normalizedusername { get; set; }
        public string Email { get; set; }
        public string Normalizedemail { get; set; }
        public bool Emailconfirmed { get; set; }
        public string Passwordhash { get; set; }
        public string Securitystamp { get; set; }
        public string Concurrencystamp { get; set; }
        public string Phonenumber { get; set; }
        public bool Phonenumberconfirmed { get; set; }
        public bool Twofactorenabled { get; set; }
        public DateTime? Lockoutend { get; set; }
        public bool Lockoutenabled { get; set; }
        public int Accessfailedcount { get; set; }
        public string Jobtitle { get; set; }
        public string Fullname { get; set; }
        public string Configuration { get; set; }
        public bool Isenabled { get; set; }
        public string Createdby { get; set; }
        public string Updatedby { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }

        public ICollection<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public ICollection<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
        public ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
    }
}

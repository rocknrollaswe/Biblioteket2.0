using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bibblan.Models;
using System.Configuration; 

namespace Bibblan.Services
{
    public static class DbInitialiser
    { 
        public static BiblioteketContext Db { get; set; }
    
        public static void InitialiseDB()
        {
            Db = new BiblioteketContext();
        }
    }
}

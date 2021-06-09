using System;
using System.Collections.Generic;
using System.Text;
using Bibblan.Services; 

namespace Bibblan.Services
{
    public static class LogOut
    {
        public static void LogOutUser() 
        {
            GlobalClass.currentUserID = null;
            GlobalClass.chosenBook = null;
            GlobalClass.chosenBookReport = null;
            GlobalClass.deletedObjects = null;
            GlobalClass.loanPermission = null;
            GlobalClass.userFirstName = null;
            GlobalClass.userPermission = null; 
        
        }

    }
}

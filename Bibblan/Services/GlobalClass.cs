using System;
using System.Collections.Generic;
using System.Text;
using Bibblan.Models;


namespace Bibblan.Services
{
    static class GlobalClass        //Globala variabler vi kommer använda i hela programmet. Skapa fler om det behövs, och sätt värde på de i MainWindow.cs filen efter login.
    {
#nullable enable //gör så att allt inom nullable går att nulla
        public static int? userPermission { get; set; }
        public static int? currentUserID { get; set; }
        public static string? userFirstName { get; set; }
        public static int? loanPermission { get; set; }
        public static Book? chosenBook { get; set; }
        public static UserReport? chosenBookReport { get; set; }
#nullable disable //slutet av nullable, allt efter kan inte nullas
    }
}

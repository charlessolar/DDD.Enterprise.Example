using System;

namespace Demo.Library.Security
{
    public class Manager : IManager
    {
        public Boolean Authorize(String Actor, String Context, String Action)
        {
            // Default manager - not a very good manager
            return true;
        }
    }
}
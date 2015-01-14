using System;

namespace Demo.Library.Security
{
    public interface IManager
    {
        Boolean Authorize(String Actor, String Context, String Action);
    }
}
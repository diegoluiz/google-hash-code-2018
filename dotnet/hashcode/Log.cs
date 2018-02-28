using System;

namespace hashcode
{
    public static class Log
    {
        public static void Write(string text, params string[] args)
        {
            Console.WriteLine(string.Format(text, args));
        }
    }
}

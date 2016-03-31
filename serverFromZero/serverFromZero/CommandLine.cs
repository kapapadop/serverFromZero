using System;

namespace serverFromZero {
    class CommandLine {
        private static readonly object _token = new object();

        public static void Write(string text) {
            lock(_token) {
                Console.WriteLine(text);
            }
        }       
    }
}
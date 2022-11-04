using System;
using Zork.Common;

namespace Zork.Cli
{
    internal class ConsoleInputService
    {
        public event EventHandler<string> InputReceived;

        public void ProccessInput()
        {
            string inputString = Console.ReadLine().Trim();
            InputReceived?.Invoke(this, inputString);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Installer
{
    public static class Interactor
    {
        private static List<string> _messageList = new List<string>();

        public static void Message(string message)
        {
            _messageList.Add(message);
            Console.WriteLine(message);
        }

        public static void MessageAndWaitForExit(string message)
        {
            Message(message);
            WaitForExit();
        }

        public static string GetNonEmpty(string message)
        {
            Message(message);
            string response = string.Empty;
            while (string.IsNullOrEmpty(response.Trim()))
            {
                ClearAndRewrite();
                response = Console.ReadLine();
            }
            return response;
        }

        private static void ClearAndRewrite()
        {
            Console.Clear();
            foreach (var message in _messageList)
                Console.WriteLine(message);
        }

        public static void WaitForExit()
        {
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}

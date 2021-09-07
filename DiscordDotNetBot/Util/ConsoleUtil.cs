using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordDotNetBot.Util
{
    public static class ConsoleUtil
    {
        private static List<string> _messageList = new List<string>();

        public static string ReadStringNonEmpty(string message)
        {
            _messageList.Add(message);
            Console.WriteLine(message);
            bool hadResponse = false;
            string response;
            do
            {
                if (hadResponse)
                    ClearAndRewrite();

                response = Console.ReadLine();
                hadResponse = true;
            } while (string.IsNullOrEmpty(response?.Trim()));
            _messageList.Add(response);
            return response;
        }

        public static string ReadSecretNonEmpty(string message)
        {
            _messageList.Add(message);
            Console.WriteLine(message);
            bool hadResponse = false;
            StringBuilder response;
            do
            {
                if (hadResponse)
                    ClearAndRewrite();

                response = new StringBuilder();
                ConsoleKeyInfo current;
                while((current = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (current.Key == ConsoleKey.Backspace)
                    {
                        if (response.Length > 0)
                        {
                            response.Remove(response.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        response.Append(current.KeyChar);
                        Console.Write("*");
                    }
                }
                hadResponse = true;
            } while (string.IsNullOrEmpty(response.ToString().Trim()));
            _messageList.Add(new string('*', response.Length));
            Console.Write("\n");
            return response.ToString();
        }

        public static bool ReadBoolYesNo(string message)
        {
            _messageList.Add(message);
            Console.WriteLine(message);
            bool hadResponse = false;
            bool? response = null;
            string input;
            do
            {
                if (hadResponse)
                    ClearAndRewrite();

                input = Console.ReadLine();
                if (input.ToUpper() == "Y") response = true;
                else if (input.ToUpper() == "N") response = false;
                hadResponse = true;
            } while (response == null);
            _messageList.Add(input);
            return response.Value;
        }

        public static void Write(string message)
        {
            Console.WriteLine(message);
            _messageList.Add(message);
        }

        public static void ClearAndEmptyMessageList()
        {
            Console.Clear();
            _messageList = new List<string>();
        }

        private static void ClearAndRewrite()
        {
            Console.Clear();
            foreach (var message in _messageList)
                Console.WriteLine(message);
        }
    }
}

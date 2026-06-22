using System;
using System.Threading;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Handles all console UI rendering — colours, dividers, and typing effects.
    /// </summary>
    public static class UIHelper
    {
        private const string Divider = "══════════════════════════════════════════════";

        /// <summary>
        /// Prints the cybersecurity ASCII art logo to the console.
        /// </summary>
        public static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Divider);
            Console.WriteLine(@"
   ██████╗██╗   ██╗██████╗ ███████╗██████╗ 
  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗
  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝
  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗
  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║
   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝

        🔒  CYBERSECURITY AWARENESS BOT  🔒
    [ Protecting you in the digital world ]
");
            Console.WriteLine(Divider);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a section divider line.
        /// </summary>
        public static void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(Divider);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a bot message with a cyan label and optional typing delay.
        /// </summary>
        /// <param name="message">The message text to display.</param>
        /// <param name="useDelay">Whether to add a typing-effect delay.</param>
        public static void PrintBotMessage(string message, bool useDelay = true)
        {
            if (useDelay)
                Thread.Sleep(400);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[CyberBot] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prompts the user for input and returns the trimmed response.
        /// </summary>
        /// <param name="prompt">The prompt label to display.</param>
        /// <returns>The trimmed string the user typed.</returns>
        public static string GetUserInput(string prompt = "You")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[{prompt}] > ");
            Console.ForegroundColor = ConsoleColor.White;
            string input = Console.ReadLine() ?? string.Empty;
            Console.ResetColor();
            return input.Trim();
        }

        /// <summary>
        /// Prints a header title in a highlighted colour.
        /// </summary>
        /// <param name="title">The header text to display.</param>
        public static void PrintHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n  *** {title} ***\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Clears the console and re-prints the logo.
        /// </summary>
        public static void ClearAndResetScreen()
        {
            Console.Clear();
            PrintLogo();
        }
    }
}

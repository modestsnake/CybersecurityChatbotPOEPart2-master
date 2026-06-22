using System;
using System.Media;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Core chatbot controller — manages conversation flow, user name, and greeting.
    /// </summary>
    public class ChatBot
    {
        private string _userName = string.Empty;
        private readonly ResponseEngine _responseEngine;
        private bool _isRunning;

        /// <summary>
        /// Initialises the ChatBot with a new ResponseEngine instance.
        /// </summary>
        public ChatBot()
        {
            _responseEngine = new ResponseEngine();
            _isRunning = true;
        }

        /// <summary>
        /// Starts the chatbot — plays the greeting, shows the logo, and begins the chat loop.
        /// </summary>
        public void Start()
        {
            UIHelper.PrintLogo();
            PlayVoiceGreeting();
            AskForName();
            UIHelper.PrintDivider();
            PrintWelcomeMessage();
            UIHelper.PrintDivider();
            RunChatLoop();
        }

        /// <summary>
        /// Attempts to play the WAV voice greeting file if it exists.
        /// </summary>
        private void PlayVoiceGreeting()
        {
            string wavPath = "greeting.wav";
            if (System.IO.File.Exists(wavPath))
            {
                try
                {
                    using var player = new SoundPlayer(wavPath);
                    player.PlaySync();
                }
                catch (Exception ex)
                {
                    UIHelper.PrintBotMessage($"(Could not play greeting audio: {ex.Message})", useDelay: false);
                }
            }
            else
            {
                // Greeting audio not found — continue silently
                UIHelper.PrintBotMessage("Welcome! (Place 'greeting.wav' in the exe folder to enable voice greeting.)", useDelay: false);
            }
        }

        /// <summary>
        /// Prompts the user to enter their name and stores it for personalised responses.
        /// </summary>
        private void AskForName()
        {
            UIHelper.PrintBotMessage("Hello! Before we begin, what's your name?", useDelay: false);

            string name;
            do
            {
                name = UIHelper.GetUserInput();
                if (string.IsNullOrWhiteSpace(name))
                    UIHelper.PrintBotMessage("I didn't catch that — please enter your name.");
            } while (string.IsNullOrWhiteSpace(name));

            _userName = name;
        }

        /// <summary>
        /// Displays a personalised welcome message and lists available topics.
        /// </summary>
        private void PrintWelcomeMessage()
        {
            UIHelper.PrintBotMessage($"Great to meet you, {_userName}! I'm CyberBot, your cybersecurity assistant.");
            UIHelper.PrintBotMessage("You can ask me about any of these topics:");

            var topics = _responseEngine.GetSupportedTopics();
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var topic in topics)
                Console.WriteLine($"   • {topic}");
            Console.ResetColor();

            UIHelper.PrintBotMessage("Type 'exit' or 'quit' at any time to leave.");
        }

        /// <summary>
        /// Runs the main conversation loop until the user exits.
        /// </summary>
        private void RunChatLoop()
        {
            while (_isRunning)
            {
                UIHelper.PrintDivider();
                string input = UIHelper.GetUserInput(_userName);

                if (string.IsNullOrWhiteSpace(input))
                {
                    UIHelper.PrintBotMessage("Please type something so I can help you.");
                    continue;
                }

                if (IsExitCommand(input))
                {
                    HandleExit();
                    break;
                }

                string? response = _responseEngine.GetResponse(input);

                if (response != null)
                {
                    UIHelper.PrintBotMessage(response);
                }
                else
                {
                    UIHelper.PrintBotMessage($"I didn't quite understand that, {_userName}. Could you rephrase? Try asking about 'password', 'phishing', or 'safe browsing'.");
                }
            }
        }

        /// <summary>
        /// Checks whether the user's input is an exit or quit command.
        /// </summary>
        /// <param name="input">The user's input string.</param>
        /// <returns>True if the input signals an intent to exit.</returns>
        private bool IsExitCommand(string input)
        {
            string lower = input.ToLower();
            return lower.Contains("exit") || lower.Contains("quit") || lower.Contains("bye");
        }

        /// <summary>
        /// Handles the farewell message before shutting down.
        /// </summary>
        private void HandleExit()
        {
            UIHelper.PrintDivider();
            UIHelper.PrintBotMessage($"Stay safe online, {_userName}! Remember: think before you click. Goodbye! 👋");
            UIHelper.PrintDivider();
            _isRunning = false;
        }
    }
}

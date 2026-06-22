using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbot.GUI
{
    /// <summary>
    /// GUI-compatible ChatBot with database integration and Part 3 features
    /// Localised for South African users
    /// </summary>
    public class ChatBot
    {
        // Event that fires when bot generates a response
        public event Action<string> ResponseGenerated;

        // Database helper
        private DatabaseHelper dbHelper;

        // User information
        public string UserName { get; private set; }
        public bool IsNameSet { get; private set; }

        // Conversation state
        private string conversationState;
        private string tempTaskTitle;
        private string tempTaskDescription;

        // Response collections
        private Dictionary<string, List<string>> responses;
        private Random random;
        private Dictionary<string, string> userMemory;

        // Quiz state
        private List<QuizQuestion> quizQuestions;
        private int currentQuizIndex;
        private int quizScore;
        private bool quizActive;

        public ChatBot()
        {
            dbHelper = new DatabaseHelper();
            dbHelper.TestConnection();

            random = new Random();
            responses = new Dictionary<string, List<string>>();
            userMemory = new Dictionary<string, string>();
            quizQuestions = new List<QuizQuestion>();

            IsNameSet = false;
            UserName = "Friend";
            conversationState = "AWAITING_NAME";

            InitializeResponses();
        }

        private void InitializeResponses()
        {
            responses["password"] = new List<string>
            {
                "🔐 Use strong passwords with at least 12 characters mixing letters, numbers, and symbols.",
                "🔐 Never reuse passwords across different accounts. Consider using a password manager!",
                "🔐 Change your passwords regularly, especially for your banking apps like Capitec, FNB, or Standard Bank.",
                "🔐 Avoid using personal details like your ID number or surname in passwords."
            };
            responses["phishing"] = new List<string>
            {
                "🎣 Be cautious of SMSes or emails pretending to be from SARS, your bank, or SASSA asking for personal info.",
                "🎣 Never click suspicious links. Scammers often pose as eThekwini, City of Joburg, or municipal services.",
                "🎣 Legitimate companies like Vodacom, MTN, or your bank will never ask for your PIN or password via SMS.",
                "🎣 Watch out for fake 'You've won an airtime voucher' messages – these are common scams in SA."
            };
            responses["privacy"] = new List<string>
            {
                "🔒 Review your privacy settings on WhatsApp, Facebook, and other social media regularly.",
                "🔒 Be careful sharing your location or personal info, especially in community WhatsApp groups.",
                "🔒 Use a VPN when connecting to public Wi-Fi at malls like Sandton City or Gateway.",
                "🔒 Under POPIA (Protection of Personal Information Act), you have rights over your personal data."
            };
            responses["malware"] = new List<string>
            {
                "🦠 Keep your antivirus software updated and run regular scans.",
                "🦠 Only download apps from the official Google Play Store or Apple App Store.",
                "🦠 Be careful with email attachments, even from people you know.",
                "🦠 Avoid downloading pirated software or movies – they often contain malware."
            };
            responses["2fa"] = new List<string>
            {
                "🔐 Two-factor authentication (2FA) adds an extra security layer beyond passwords.",
                "🔐 Enable 2FA on your banking apps, Gmail, and social media accounts.",
                "🔐 Many SA banks offer app-based 2FA which is safer than SMS OTPs.",
                "🔐 2FA makes it much harder for scammers to access your accounts even if they get your password."
            };
        }

        /// <summary>
        /// Returns the initial greeting message
        /// </summary>
        public string GetGreeting()
        {
            return "Howzit! Welcome to the Cybersecurity Awareness Bot. 🔒\n\n" +
                   "I'm here to help you stay safe online here in South Africa.\n\n" +
                   "Before we begin, what's your name?";
        }

        /// <summary>
        /// Main input processing method - called by the GUI
        /// </summary>
        public void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                RaiseResponse("I didn't catch that. Could you please say something?");
                return;
            }

            // Handle name capture first
            if (!IsNameSet)
            {
                UserName = input.Trim();
                IsNameSet = true;
                conversationState = "NORMAL";
                dbHelper.LogActivity("Chat Started", $"User: {UserName}");
                RaiseResponse($"Lekker to meet you, {UserName}! 😊\n\n" +
                             "I'm here to help you stay safe online. Type 'help' to see what I can do!");
                return;
            }

            // Handle quiz mode
            if (quizActive)
            {
                ProcessQuizAnswer(input);
                return;
            }

            // Handle multi-step states (like adding tasks)
            if (conversationState == "TASK_TITLE")
            {
                tempTaskTitle = input.Trim();
                conversationState = "TASK_DESCRIPTION";
                RaiseResponse("Sharp! Now provide a description (or type 'skip'):");
                return;
            }

            if (conversationState == "TASK_DESCRIPTION")
            {
                tempTaskDescription = input.ToLower() == "skip" ? "" : input.Trim();
                conversationState = "TASK_REMINDER";
                RaiseResponse("Would you like to set a reminder? Enter a date (yyyy-MM-dd) or type 'skip':");
                return;
            }

            if (conversationState == "TASK_REMINDER")
            {
                DateTime? reminderDate = null;
                if (input.ToLower() != "skip" && DateTime.TryParse(input.Trim(), out DateTime parsed))
                {
                    reminderDate = parsed;
                }

                bool success = dbHelper.AddTask(tempTaskTitle, tempTaskDescription, reminderDate);
                conversationState = "NORMAL";

                if (success)
                {
                    string msg = $"✓ Task added: '{tempTaskTitle}'";
                    if (reminderDate.HasValue)
                        msg += $"\n⏰ Reminder set for: {reminderDate.Value:yyyy-MM-dd}";
                    RaiseResponse(msg);
                }
                else
                {
                    RaiseResponse("✗ Eish! Failed to add task. Please try again.");
                }
                return;
            }

            if (conversationState == "AWAITING_TASK_ID_COMPLETE")
            {
                conversationState = "NORMAL";
                if (int.TryParse(input.Trim(), out int taskId))
                {
                    bool success = dbHelper.CompleteTask(taskId);
                    RaiseResponse(success ? $"✓ Sharp! Task #{taskId} marked as complete!" : $"✗ Could not find task #{taskId}");
                }
                else
                {
                    RaiseResponse("Invalid task ID. Please enter a number.");
                }
                return;
            }

            if (conversationState == "AWAITING_TASK_ID_DELETE")
            {
                conversationState = "NORMAL";
                if (int.TryParse(input.Trim(), out int taskId))
                {
                    bool success = dbHelper.DeleteTask(taskId);
                    RaiseResponse(success ? $"✓ Task #{taskId} deleted!" : $"✗ Could not find task #{taskId}");
                }
                else
                {
                    RaiseResponse("Invalid task ID. Please enter a number.");
                }
                return;
            }

            // Normal conversation - detect intent
            string intent = DetectIntent(input);
            dbHelper.LogActivity("User Input", input);
            HandleIntent(intent, input);
        }

        /// <summary>
        /// NLP keyword detection
        /// </summary>
        private string DetectIntent(string userInput)
        {
            string input = userInput.ToLower().Trim();

            if (input == "help" || input.Contains("what can you do") || input.Contains("commands"))
                return "HELP";

            if (input == "hello" || input == "hi" || input == "hey" || input == "howzit" || input == "molo" || input == "sawubona")
                return "GREETING";

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("concerned") || input.Contains("stressed"))
                return "WORRIED";

            if (input.Contains("add task") || input.Contains("create task") ||
                input.Contains("new task") || input.Contains("remind me") ||
                input.Contains("set reminder") || input.Contains("add a task"))
                return "ADD_TASK";

            if (input.Contains("show task") || input.Contains("view task") ||
                input.Contains("my task") || input.Contains("list task"))
                return "VIEW_TASKS";

            if (input.Contains("delete task") || input.Contains("remove task"))
                return "DELETE_TASK";

            if (input.Contains("complete task") || input.Contains("finish task") ||
                input.Contains("mark complete"))
                return "COMPLETE_TASK";

            if (input.Contains("quiz") || input.Contains("game") ||
                input.Contains("test me") || input.Contains("play"))
                return "START_QUIZ";

            if (input.Contains("activity log") || input.Contains("show log") ||
                input.Contains("what have you done") || input.Contains("history"))
                return "SHOW_LOG";

            if (input.Contains("tell me more") || input.Contains("explain more") ||
                input.Contains("more info"))
                return "MORE_INFO";

            if (input.Contains("password"))
                return "PASSWORD";

            if (input.Contains("phishing") || input.Contains("scam"))
                return "PHISHING";

            if (input.Contains("privacy") || input.Contains("private") || input.Contains("popia"))
                return "PRIVACY";

            if (input.Contains("malware") || input.Contains("virus"))
                return "MALWARE";

            if (input.Contains("2fa") || input.Contains("two factor") ||
                input.Contains("two-factor") || input.Contains("authentication") || input.Contains("otp"))
                return "2FA";

            return "UNKNOWN";
        }

        /// <summary>
        /// Handle detected intent
        /// </summary>
        private void HandleIntent(string intent, string userInput)
        {
            switch (intent)
            {
                case "ADD_TASK":
                    conversationState = "TASK_TITLE";
                    RaiseResponse("Lekker! Let's add a cybersecurity task. 📋\n\nWhat is the task title?");
                    break;

                case "VIEW_TASKS":
                    RaiseResponse(GetTasksList());
                    break;

                case "COMPLETE_TASK":
                    conversationState = "AWAITING_TASK_ID_COMPLETE";
                    RaiseResponse(GetTasksList() + "\n\nEnter the task ID to mark as complete:");
                    break;

                case "DELETE_TASK":
                    conversationState = "AWAITING_TASK_ID_DELETE";
                    RaiseResponse(GetTasksList() + "\n\nEnter the task ID to delete:");
                    break;

                case "START_QUIZ":
                    StartQuiz();
                    break;

                case "SHOW_LOG":
                    RaiseResponse(GetActivityLog());
                    break;

                case "PASSWORD":
                    userMemory["last_topic"] = "password";
                    dbHelper.LogActivity("Topic Discussed", "Password Security");
                    RaiseResponse(GetRandomResponse("password"));
                    break;

                case "PHISHING":
                    userMemory["last_topic"] = "phishing";
                    dbHelper.LogActivity("Topic Discussed", "Phishing");
                    RaiseResponse(GetRandomResponse("phishing"));
                    break;

                case "PRIVACY":
                    userMemory["last_topic"] = "privacy";
                    dbHelper.LogActivity("Topic Discussed", "Privacy");
                    RaiseResponse(GetRandomResponse("privacy"));
                    break;

                case "MALWARE":
                    userMemory["last_topic"] = "malware";
                    dbHelper.LogActivity("Topic Discussed", "Malware");
                    RaiseResponse(GetRandomResponse("malware"));
                    break;

                case "2FA":
                    userMemory["last_topic"] = "2fa";
                    dbHelper.LogActivity("Topic Discussed", "2FA");
                    RaiseResponse(GetRandomResponse("2fa"));
                    break;

                case "MORE_INFO":
                    RaiseResponse(HandleMoreInfo());
                    break;

                case "GREETING":
                    RaiseResponse($"Howzit {UserName}! How can I help you stay safe online today?");
                    break;

                case "HELP":
                    RaiseResponse(GetHelpMessage());
                    break;

                case "WORRIED":
                    RaiseResponse(HandleSentiment());
                    break;

                default:
                    RaiseResponse("Sorry, I'm not sure I understand. Try:\n• 'add task' or 'view tasks'\n• 'start quiz'\n• 'show log'\n• Ask about passwords, phishing, privacy\n• Type 'help' for more!");
                    break;
            }
        }

        #region Task Methods

        private string GetTasksList()
        {
            List<TaskItem> tasks = dbHelper.GetAllTasks();

            if (tasks.Count == 0)
                return "📋 You have no tasks yet. Say 'add task' to create one!";

            string result = "📋 Your Cybersecurity Tasks:\n";
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "✓" : "○";
                string reminder = task.ReminderDate.HasValue ? $" [Due: {task.ReminderDate.Value:yyyy-MM-dd}]" : "";
                result += $"\n{status} #{task.TaskId}: {task.Title}{reminder}";
                if (!string.IsNullOrWhiteSpace(task.Description))
                    result += $"\n   {task.Description}";
            }
            return result;
        }

        #endregion

        #region Quiz Methods

        private void StartQuiz()
        {
            quizQuestions = dbHelper.GetQuizQuestions();

            if (quizQuestions.Count == 0)
            {
                RaiseResponse("Eish! No quiz questions available. Please check the database.");
                return;
            }

            quizActive = true;
            currentQuizIndex = 0;
            quizScore = 0;

            dbHelper.LogActivity("Quiz Started", $"User: {UserName}");
            RaiseResponse($"🎮 CYBERSECURITY QUIZ STARTED!\n\nAnswer {quizQuestions.Count} questions. Type A, B, C, or D.\n\n" + GetCurrentQuestion());
        }

        private string GetCurrentQuestion()
        {
            var q = quizQuestions[currentQuizIndex];
            return $"━━━ Question {currentQuizIndex + 1}/{quizQuestions.Count} ━━━\n\n" +
                   $"{q.QuestionText}\n\n" +
                   $"A) {q.OptionA}\n" +
                   $"B) {q.OptionB}\n" +
                   $"C) {q.OptionC}\n" +
                   $"D) {q.OptionD}";
        }

        private void ProcessQuizAnswer(string input)
        {
            string answer = input.Trim().ToUpper();

            if (answer.Length == 0 || !"ABCD".Contains(answer[0]))
            {
                RaiseResponse("Please answer with A, B, C, or D.");
                return;
            }

            var question = quizQuestions[currentQuizIndex];
            string feedback;

            if (question.IsCorrect(answer[0]))
            {
                quizScore++;
                feedback = "✓ Correct! Sharp sharp!\n\n";
            }
            else
            {
                feedback = $"✗ Eish, not quite. The correct answer is {question.CorrectAnswer}.\n\n";
            }

            feedback += $"💡 {question.Explanation}";

            currentQuizIndex++;

            if (currentQuizIndex < quizQuestions.Count)
            {
                RaiseResponse(feedback + "\n\n" + GetCurrentQuestion());
            }
            else
            {
                quizActive = false;
                double percentage = (double)quizScore / quizQuestions.Count * 100;

                string result = $"{feedback}\n\n🏆 QUIZ COMPLETED!\n\n";
                result += $"Your Score: {quizScore}/{quizQuestions.Count} ({percentage:F1}%)\n\n";

                if (percentage >= 90)
                    result += "🌟 Excellent! You're a cybersecurity pro, my friend!";
                else if (percentage >= 70)
                    result += "👍 Lekker job! Keep learning to stay safe online!";
                else if (percentage >= 50)
                    result += "📚 Not bad, but there's room for improvement!";
                else
                    result += "📖 Keep practicing! Staying safe online is important!";

                dbHelper.LogActivity("Quiz Completed", $"Score: {quizScore}/{quizQuestions.Count}");
                RaiseResponse(result);
            }
        }

        #endregion

        #region Activity Log

        private string GetActivityLog()
        {
            List<ActivityLogItem> logs = dbHelper.GetActivityLog(10);

            if (logs.Count == 0)
                return "📊 No activity recorded yet.";

            string result = "📊 RECENT ACTIVITY LOG:\n";
            int count = 1;
            foreach (var log in logs)
            {
                result += $"\n{count}. [{log.Timestamp:HH:mm:ss}] {log.ActionType}";
                result += $"\n   └─ {log.Description}";
                count++;
            }
            result += $"\n\n(Showing last {logs.Count} activities)";
            return result;
        }

        #endregion

        #region Helper Methods

        private string GetRandomResponse(string category)
        {
            if (responses.ContainsKey(category) && responses[category].Count > 0)
            {
                int index = random.Next(responses[category].Count);
                return responses[category][index];
            }
            return "I don't have information on that topic right now.";
        }

        private string HandleMoreInfo()
        {
            if (userMemory.ContainsKey("last_topic"))
            {
                string topic = userMemory["last_topic"];
                return $"Here's more about {topic}:\n\n" + GetRandomResponse(topic);
            }
            return "What would you like to know more about? Try passwords, phishing, privacy, or malware.";
        }

        private string HandleSentiment()
        {
            return $"I understand you're concerned, {UserName}. 💙\n\n" +
                   "Cybercrime is a big problem here in SA, but don't stress – I'm here to help!\n\n" +
                   "Would you like to:\n" +
                   "• Learn about strong passwords?\n" +
                   "• Understand phishing scams (like fake SARS SMSes)?\n" +
                   "• Take a quick quiz?\n\n" +
                   "Just let me know!";
        }

        private string GetHelpMessage()
        {
            return "🤖 Here's what I can help you with:\n\n" +
                   "📋 TASK MANAGEMENT:\n" +
                   "   • 'add task' - Create a task\n" +
                   "   • 'view tasks' - See all tasks\n" +
                   "   • 'complete task' - Mark as done\n" +
                   "   • 'delete task' - Remove a task\n\n" +
                   "🎮 QUIZ:\n" +
                   "   • 'start quiz' - Test your knowledge\n\n" +
                   "📊 ACTIVITY:\n" +
                   "   • 'show log' - View activity history\n\n" +
                   "🔐 TOPICS:\n" +
                   "   • passwords, phishing, privacy, malware, 2FA\n" +
                   "   • 'tell me more' for additional info";
        }

        private void RaiseResponse(string message)
        {
            ResponseGenerated?.Invoke(message);
        }

        #endregion
    }
}
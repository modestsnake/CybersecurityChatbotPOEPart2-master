using System;
using System.Collections.Generic;

namespace CybersecurityChatbot.GUI
{
    public class ResponseEngine
    {
        private readonly Dictionary<string, string> _responses;
        private readonly Dictionary<string, List<string>> _randomResponses;
        private readonly Random _random;

        public string LastTopic { get; private set; } = string.Empty;

        public ResponseEngine()
        {
            _random = new Random();

            _responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you",     "Running at full capacity and ready to help keep you cyber-safe!" },
                { "purpose",         "I'm your Cybersecurity Awareness Bot! Ask me about passwords, phishing, safe browsing, malware, 2FA, scams, or privacy." },
                { "what can you do", "I can give you tips on:\n• Passwords\n• Phishing\n• Safe browsing\n• Malware\n• 2FA\n• Scams\n• Privacy" },
                { "safe browsing",   "🌐 Safe Browsing:\n• Look for HTTPS before entering personal info.\n• Keep your browser updated.\n• Use an ad blocker.\n• Avoid downloading software from unknown sites." },
                { "malware",         "🦠 Malware Protection:\n• Keep your OS and antivirus updated.\n• Never open attachments from unknown senders.\n• Scan USB drives before use.\n• Back up your data regularly." },
                { "scam",            "⚠️ Scam Awareness:\n• If it sounds too good to be true, it probably is.\n• Legitimate companies never ask for gift card payments.\n• Verify phone numbers before calling back." },
                { "privacy",         "🛡️ Privacy Tips:\n• Review app permissions regularly.\n• Use a VPN on public Wi-Fi.\n• Audit which apps access your social accounts." },
                { "2fa",             "🔐 Two-Factor Authentication:\n• Adds a second login step beyond your password.\n• Use an authenticator app over SMS codes.\n• Enable it on email, banking, and social media." },
                { "two-factor",      "🔐 Two-Factor Authentication:\n• Authenticator apps are more secure than SMS codes.\n• Enable it everywhere that supports it." }
            };

            _randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "password", new List<string>
                    {
                        "🔑 Use at least 12 characters mixing uppercase, lowercase, numbers, and symbols. Never reuse passwords.",
                        "🔑 Consider a password manager like Bitwarden or 1Password — they generate and store strong passwords for you.",
                        "🔑 A passphrase like 'BlueSky!Running#42' is both strong and memorable. Avoid dictionary words alone."
                    }
                },
                {
                    "phishing", new List<string>
                    {
                        "🎣 Always verify the sender's email address — scammers use addresses that look almost right.",
                        "🎣 Hover over links before clicking to see the real destination URL. If in doubt, go directly to the website.",
                        "🎣 Urgent language and spelling errors are classic phishing red flags. Pause and verify before acting."
                    }
                }
            };
        }

        public string? GetResponse(string input)
        {
            string lower = input.ToLower();
            foreach (var entry in _randomResponses)
            {
                if (lower.Contains(entry.Key.ToLower()))
                {
                    LastTopic = entry.Key;
                    return entry.Value[_random.Next(entry.Value.Count)];
                }
            }
            foreach (var entry in _responses)
            {
                if (lower.Contains(entry.Key.ToLower()))
                {
                    LastTopic = entry.Key;
                    return entry.Value;
                }
            }
            return null;
        }

        public string GetFollowUp()
        {
            if (string.IsNullOrEmpty(LastTopic))
                return "What topic would you like to explore? Try passwords, phishing, or safe browsing.";
            if (_randomResponses.TryGetValue(LastTopic, out var list))
                return $"Here's another tip on {LastTopic}:\n{list[_random.Next(list.Count)]}";
            if (_responses.TryGetValue(LastTopic, out var response))
                return $"To add to what I said about {LastTopic}:\n{response}";
            return $"I've shared what I know about {LastTopic}. Try another topic!";
        }

        public List<string> GetSupportedTopics()
        {
            var topics = new List<string>(_responses.Keys);
            foreach (var key in _randomResponses.Keys)
                if (!topics.Contains(key)) topics.Add(key);
            return topics;
        }
    }
}
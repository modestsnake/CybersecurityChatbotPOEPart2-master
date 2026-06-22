using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Provides cybersecurity keyword matching and response generation.
    /// </summary>
    public class ResponseEngine
    {
        private readonly Dictionary<string, string> _responses;
        private readonly Random _random;

        /// <summary>
        /// Initialises the response engine with keyword-to-response mappings.
        /// </summary>
        public ResponseEngine()
        {
            _random = new Random();

            _responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "how are you",
                    "I'm running at full capacity and ready to help keep you cyber-safe! How about you?"
                },
                {
                    "purpose",
                    "I'm your personal Cybersecurity Awareness Bot! I can teach you about passwords, phishing, safe browsing, malware, and more. Just ask!"
                },
                {
                    "what can you do",
                    "Great question! I can give you tips on:\n  • Passwords\n  • Phishing scams\n  • Safe browsing\n  • Malware\n  • Two-factor authentication\n  ...and much more. Just type a topic!"
                },
                {
                    "password",
                    "🔑 Password Tip:\n  • Use at least 12 characters.\n  • Mix uppercase, lowercase, numbers, and symbols.\n  • Never reuse passwords across sites.\n  • Consider using a password manager like Bitwarden or 1Password."
                },
                {
                    "phishing",
                    "🎣 Phishing Alert:\n  • Always check the sender's email address carefully.\n  • Never click links in unexpected emails — go directly to the website.\n  • Look for spelling mistakes and urgent language — these are red flags.\n  • When in doubt, delete the email."
                },
                {
                    "safe browsing",
                    "🌐 Safe Browsing Tips:\n  • Look for HTTPS and the padlock icon before entering any personal info.\n  • Keep your browser and extensions up to date.\n  • Use an ad blocker to reduce exposure to malicious ads.\n  • Avoid downloading software from untrusted sources."
                },
                {
                    "malware",
                    "🦠 Malware Protection:\n  • Keep your operating system and antivirus software updated.\n  • Never open attachments from unknown senders.\n  • Scan USB drives before use.\n  • Back up your data regularly."
                },
                {
                    "2fa",
                    "🔐 Two-Factor Authentication (2FA):\n  • 2FA adds a second layer of security beyond your password.\n  • Use an authenticator app (Google Authenticator, Authy) rather than SMS.\n  • Enable 2FA on all important accounts — email, banking, social media."
                },
                {
                    "scam",
                    "⚠️ Scam Awareness:\n  • If an offer sounds too good to be true, it probably is.\n  • Legitimate companies will never ask for payment via gift cards.\n  • Verify phone numbers and email addresses before responding.\n  • Report scams to your national cybercrime authority."
                },
                {
                    "privacy",
                    "🛡️ Privacy Tips:\n  • Review app permissions — does your torch app really need your contacts?\n  • Use a VPN on public Wi-Fi.\n  • Regularly audit which apps have access to your social media accounts.\n  • Read privacy policies before signing up to services."
                }
            };
        }

        /// <summary>
        /// Attempts to find a matching response for the given user input.
        /// Returns null if no keyword is matched.
        /// </summary>
        /// <param name="input">The raw user input string.</param>
        /// <returns>A response string, or null if unrecognised.</returns>
        public string? GetResponse(string input)
        {
            string lower = input.ToLower();

            foreach (var entry in _responses)
            {
                if (lower.Contains(entry.Key.ToLower()))
                    return entry.Value;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all supported keywords the user can ask about.
        /// </summary>
        /// <returns>A list of keyword strings.</returns>
        public List<string> GetSupportedTopics()
        {
            return new List<string>(_responses.Keys);
        }
    }
}

using System;
using System.Collections.Generic;

namespace CybersecurityChatbot.GUI
{
    public class MemoryStore
    {
        private readonly Dictionary<string, string> _userMemory;

        public MemoryStore()
        {
            _userMemory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void Set(string key, string value) => _userMemory[key] = value;

        public string? Get(string key) =>
            _userMemory.TryGetValue(key, out string? value) ? value : null;

        public bool Has(string key) => _userMemory.ContainsKey(key);

        public string? TryExtractInterest(string input)
        {
            string lower = input.ToLower();
            string[] triggers = { "interested in", "care about", "want to learn about", "curious about" };
            foreach (string trigger in triggers)
            {
                int idx = lower.IndexOf(trigger);
                if (idx >= 0)
                {
                    string topic = input.Substring(idx + trigger.Length).Trim().TrimEnd('.', '!', '?');
                    if (!string.IsNullOrWhiteSpace(topic))
                    {
                        Set("interest", topic);
                        return topic;
                    }
                }
            }
            return null;
        }
    }
}
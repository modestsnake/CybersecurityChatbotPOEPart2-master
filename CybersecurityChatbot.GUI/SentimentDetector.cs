using System;
using System.Collections.Generic;

namespace CybersecurityChatbot.GUI
{
    public class SentimentDetector
    {
        public enum Sentiment { Positive, Negative, Neutral }

        private readonly Dictionary<string, Sentiment> _sentimentMap;

        public SentimentDetector()
        {
            _sentimentMap = new Dictionary<string, Sentiment>(StringComparer.OrdinalIgnoreCase)
            {
                { "worried",    Sentiment.Negative },
                { "scared",     Sentiment.Negative },
                { "frustrated", Sentiment.Negative },
                { "confused",   Sentiment.Negative },
                { "anxious",    Sentiment.Negative },
                { "stressed",   Sentiment.Negative },
                { "angry",      Sentiment.Negative },
                { "upset",      Sentiment.Negative },
                { "happy",      Sentiment.Positive },
                { "curious",    Sentiment.Positive },
                { "excited",    Sentiment.Positive },
                { "great",      Sentiment.Positive },
                { "glad",       Sentiment.Positive },
                { "good",       Sentiment.Positive }
            };
        }

        public Sentiment DetectSentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var entry in _sentimentMap)
                if (lower.Contains(entry.Key)) return entry.Value;
            return Sentiment.Neutral;
        }

        public string? GetSentimentResponse(Sentiment sentiment)
        {
            return sentiment switch
            {
                Sentiment.Negative => "I understand — cybersecurity can feel overwhelming. You're doing the right thing by learning. Here's something helpful:",
                Sentiment.Positive => "That's great to hear! Your enthusiasm for staying safe online is the right attitude. Here's a tip:",
                _ => null
            };
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace ChatBotPartTwo
{
    public class SentimentResponse
    {
        // Dictionary storing sentiments and responses
        private readonly Dictionary<string, List<string>> sentiments;

        public SentimentResponse()
        {
            sentiments = LoadSentiments();
        }


        // ================= LOAD SENTIMENTS =================

        private Dictionary<string, List<string>> LoadSentiments()
        {
            return new Dictionary<string, List<string>>
            {
                {
                    "worried",
                    new List<string>
                    {
                        "It's understandable to feel worried about cybersecurity threats.",
                        "Online scams can be stressful, but staying informed helps a lot.",
                        "Don't worry, learning safe online habits already puts you ahead."
                    }
                },

                {
                    "scared",
                    new List<string>
                    {
                        "Cyber threats can seem scary at first, but awareness is powerful.",
                        "Taking small security steps can greatly improve your online safety.",
                        "You're not alone, many people feel concerned about online threats."
                    }
                },

                {
                    "confused",
                    new List<string>
                    {
                        "Cybersecurity can feel confusing initially, but you'll learn step by step.",
                        "It's okay to ask questions, cybersecurity has many concepts.",
                        "Learning gradually is the best approach to understanding cybersecurity."
                    }
                },

                {
                    "frustrated",
                    new List<string>
                    {
                        "Technology problems can be frustrating sometimes.",
                        "Cybersecurity takes practice, so don't give up.",
                        "You're doing well by continuing to learn despite the challenges."
                    }
                },

                {
                    "curious",
                    new List<string>
                    {
                        "Curiosity is excellent for learning cybersecurity.",
                        "Exploring cybersecurity topics helps improve your digital safety.",
                        "The more curious you are, the more prepared you'll become online."
                    }
                },

                {
                    "excited",
                    new List<string>
                    {
                        "That's great! Cybersecurity is an exciting field to explore.",
                        "Your enthusiasm will help you learn cybersecurity faster.",
                        "Being excited about online safety is a great mindset to have."
                    }
                }
            };
        }


        // ================= DETECT SENTIMENT =================

        public string DetectSentiment(string userInput)
        {
            userInput = userInput.ToLower();

            foreach (var sentiment in sentiments)
            {
                if (userInput.Contains(sentiment.Key))
                {
                    List<string> responses = sentiment.Value;

                    RandomResponse random =
                        new RandomResponse();

                    return responses[
                        new System.Random()
                        .Next(responses.Count)];
                }
            }

            return null;
        }
    }
}
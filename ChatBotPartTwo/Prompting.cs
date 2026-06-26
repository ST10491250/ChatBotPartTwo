using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotPartTwo
{
    // Handles chatbot interaction logic
    public class Prompting
    {
        private readonly Storing store;
        private readonly RandomResponse randomResponse;

        private readonly MemoryRecall memoryRecall;

        private readonly SentimentResponse sentimentResponse;

        private readonly List<string> memoryTriggers =
    new List<string>
{
    "i am interested in",
    "my interests are",
    "i like",
    "i enjoy learning about",
    "i want to learn about"
};

        public Prompting()
        {
            store = new Storing();

            randomResponse = new RandomResponse();

            memoryRecall = new MemoryRecall();

            sentimentResponse = new SentimentResponse();
        }


        // ================= GET BOT RESPONSE =================

        public string GetBotResponse(string userInput)
        {
            // Clean input
            userInput = userInput.Trim().ToLower();

            // ================= 1. HANDLE USER NAME FIRST =================
            // This ensures the chatbot starts with a name before anything else
            if (string.IsNullOrEmpty(store.UserName))
            {
                if (store.CheckUserName(userInput))
                {
                    store.UserName = userInput;

                    return $"Nice to meet you {store.UserName}! " +
                           "How can I help you with cybersecurity today?";
                }
                else
                {
                    return "Please enter a valid name (letters only, no numbers or symbols).";
                }
            }

            // ================= 2. VALIDATE QUESTION =================
            // Prevent empty or invalid inputs after name is set
            if (!store.VerifyUserQuestion(userInput))
            {
                return "Please enter a valid cybersecurity question.";
            }

            // ================= 3. EXIT HANDLING =================
            // Allow user to exit chatbot gracefully
            if (store.ExitBot(userInput))
            {
                return $"Goodbye {store.UserName}, stay safe online!";
            }

            // ================= 4. SENTIMENT DETECTION =================
            // Detect emotional state and respond empathetically
            string sentimentReply =
                sentimentResponse.DetectSentiment(userInput);

            if (!string.IsNullOrEmpty(sentimentReply))
            {
                return sentimentReply;
            }

            // ================= MEMORY RECALL =================

            if (IsMemoryRecallRequest(userInput))
            {
                string interest =
                    memoryRecall.GetUserInterest(store.UserName);

                if (!string.IsNullOrEmpty(interest))
                {
                    return $"I remember you're interested in {interest}.";
                }
            }

            // ================= 5. RANDOM RESPONSE CHECK =================
            // If input matches a cybersecurity keyword, return a random response
            string randomReply =
                randomResponse.GetRandomResponse(userInput);

            if (!string.IsNullOrEmpty(randomReply))
            {
                // Save topic for memory (simple learning behaviour)
                memoryRecall.SaveMemory(store.UserName, userInput);

                return randomReply;
            }

          

            string detectedInterest =
                DetectInterest(userInput);

            if (!string.IsNullOrEmpty(detectedInterest))
            {
                memoryRecall.SaveMemory(
                    store.UserName,
                    detectedInterest);

                return $"Great! I'll remember that you're interested in {detectedInterest}.";
            }

            // ================= 7. KEYWORD MATCHING (FALLBACK SYSTEM) =================
            //dictionary-based response system from Storing.cs
            List<string> responses = new List<string>();

            string[] words = userInput.Split(' ');

            List<string> filteredWords = new List<string>();

            foreach (string word in words)
            {
                if (!store.WordsToIgnore.Contains(word))
                {
                    filteredWords.Add(word);
                }
            }

            string cleanedInput = string.Join(" ", filteredWords);

            foreach (var entry in store.BotResponses)
            {
                string[] keywords = entry.Key.Split('|');

                foreach (string keyword in keywords)
                {
                    if (cleanedInput.Contains(keyword.Trim()))
                    {
                        responses.Add(entry.Value);
                        break;
                    }
                }
            }

            if (responses.Count > 0)
            {
                return string.Join("\n", responses);
            }

            // ================= 8. DEFAULT ERROR HANDLING =================
            // Final fallback if nothing matches
            return "I'm not sure I understand. Can you rephrase your cybersecurity question?";
        }

        // ================= SET USER NAME 

        public bool SetUserName(string name)
        {
            if (store.CheckUserName(name))
            {
                store.UserName = name;
                return true;
            }

            return false;
        }

        private string DetectInterest(string userInput)
        {
            foreach (string trigger in memoryTriggers)
            {
                if (userInput.Contains(trigger))
                {
                    // Remove trigger phrase
                    string topic = userInput.Replace(trigger, "").Trim();

                    return topic;
                }
            }

            return null;
        }

        private bool IsMemoryRecallRequest(string userInput)
        {
            List<string> recallTriggers = new List<string>
    {
        "what do you remember",
        "remember me",
        "my interests",
        "what are my interests",
        "hello",
        "hi"
    };

            return recallTriggers.Any(trigger =>
                userInput.Contains(trigger));
        }

    }
}
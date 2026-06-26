using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChatBotPartTwo
{
    public class MemoryRecall
    {
        private readonly string filePath;

        public MemoryRecall()
        {
            string folder = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);

            filePath = Path.Combine(folder, "UserMemory.txt");

            CreateFileIfNeeded();
        }


        // ================= CREATE FILE =================

        private void CreateFileIfNeeded()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }


        // ================= SAVE MEMORY =================

        public void SaveMemory(string username, string topic)
        {
            List<string> storedMemory =
                File.ReadAllLines(filePath).ToList();

            string newEntry = $"{username}|{topic}";

            // Prevent duplicates
            if (!storedMemory.Contains(newEntry))
            {
                storedMemory.Add(newEntry);

                File.WriteAllLines(filePath, storedMemory);
            }
        }


        // ================= GET USER INTEREST =================

        public string GetUserInterest(string username)
        {
            List<string> storedMemory =
                File.ReadAllLines(filePath).ToList();

            foreach (string line in storedMemory)
            {
                string[] parts = line.Split('|');

                if (parts[0].Equals(username,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return parts[1];
                }
            }

            return null;
        }


        // ================= DISPLAY MEMORY =================

        public List<string> GetAllMemory()
        {
            return File.ReadAllLines(filePath).ToList();
        }
    }
}
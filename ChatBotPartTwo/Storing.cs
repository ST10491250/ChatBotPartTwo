using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChatBotPartTwo
{
    // Handles chatbot data, validation, and responses
    public class Storing
    {
        // ================= PROPERTIES =================

        public Dictionary<string, string> BotResponses { get; private set; }

        public List<string> WordsToIgnore { get; private set; }

        public string UserName { get; set; } = string.Empty;

        public string UserQuestion { get; set; } = string.Empty;


        // ================= CONSTRUCTOR =================

        public Storing()
        {
            BotResponses = new Dictionary<string, string>();
            WordsToIgnore = new List<string>();

            StoreWordsToIgnore();
            StoreBotResponses();
        }


        // ================= STORE STOP WORDS =================

        private void StoreWordsToIgnore()
        {
            WordsToIgnore.AddRange(new List<string>
            {
                "tell",
                "me",
                "about",
                "what",
                "you",
                "is",
                "please",
                "define",
                "how",
                "why",
                "do",
                "i",
                "with",
                "give",
                "provide",
                "tips",
                "tip",
                "and",
                "am"
            });
        }


        // ================= STORE BOT RESPONSES =================

        private void StoreBotResponses()
        {
            BotResponses.Add("hello|hi|hey",
                $"Hello {UserName}!\nI am happy to assist you today.\nFeel free to ask me anything about cybersecurity and online safety.");

            BotResponses.Add("thanks|thank you",
                $"You are welcome {UserName}!\nI am always here to help you.\nNever hesitate to ask questions when you need assistance.");

            BotResponses.Add("bye|goodbye",
                $"Goodbye {UserName}!\nStay safe while browsing online.\nRemember to protect your personal information.");

            BotResponses.Add("password",
                "A strong password helps protect your accounts from hackers.\nAlways use numbers, symbols, and uppercase letters.\nNever share your password with anyone.");

            BotResponses.Add("phishing",
                "Phishing is a scam used to steal personal information.\nAttackers often send fake emails or messages.\nAlways check links before clicking them.");

            BotResponses.Add("malware",
                "Malware is harmful software designed to damage computers.\nIt can steal data or slow down devices.\nAlways install trusted antivirus software.");

            BotResponses.Add("virus",
                "A computer virus spreads from one device to another.\nIt can damage files and programs.\nKeeping your antivirus updated helps protect your system.");

            BotResponses.Add("ransomware",
                "Ransomware locks your files or device.\nAttackers demand money to restore access.\nBacking up your data can help prevent major loss.");

            BotResponses.Add("hacker",
                "A hacker is someone skilled in computers and networks.\nSome hackers help improve security.\nOthers try to steal data or cause damage.");

            BotResponses.Add("firewall",
                "A firewall helps block unauthorized access.\nIt monitors network traffic entering and leaving your system.\nFirewalls improve computer and network security.");

            BotResponses.Add("vpn",
                "A VPN protects your online privacy.\nIt encrypts your internet connection.\nVPNs are useful when using public WiFi networks.");

            BotResponses.Add("wifi",
                "Public WiFi networks may not always be safe.\nHackers can sometimes steal information on open networks.\nAvoid accessing sensitive accounts on public WiFi.");

            BotResponses.Add("2fa",
                "Two-factor authentication adds extra security to accounts.\nIt requires another verification step after entering a password.\nThis helps prevent unauthorized access.");

            BotResponses.Add("encryption",
                "Encryption protects information by converting it into secret code.\nOnly authorized users can read encrypted data.\nEncryption is important for online privacy and security.");

            BotResponses.Add("cybersecurity",
                "Cybersecurity protects computers, networks, and information.\nIt helps prevent cyber attacks and data theft.\nGood cybersecurity practices keep users safe online.");

            BotResponses.Add("ddos",
                "A DDoS attack floods a server with too much traffic.\nThis causes websites or services to become unavailable.\nOrganizations use security tools to reduce these attacks.");

            BotResponses.Add("scam",
                "Online scams are designed to trick users.\nScammers may ask for money or personal information.\nAlways verify suspicious emails, messages, or websites.");

            BotResponses.Add("spam",
                "Spam messages are unwanted emails or advertisements.\nSome spam messages may contain harmful links.\nAvoid opening suspicious attachments from unknown senders.");

            BotResponses.Add("backup",
                "Backing up data protects important information.\nIt helps recover files after cyber attacks or accidents.\nAlways keep copies of important files in safe locations.");

            BotResponses.Add("update",
                "Software updates improve security and performance.\nUpdates fix vulnerabilities that hackers may exploit.\nAlways keep your applications and devices updated.");

            BotResponses.Add("antivirus",
                "Antivirus software detects harmful programs.\nIt helps remove malware and viruses from devices.\nRegular scans improve system protection.");

            BotResponses.Add("social",
                "Social engineering tricks people into revealing information.\nAttackers manipulate users through trust or fear.\nAlways verify requests for sensitive information.");

            BotResponses.Add("network",
                "A network connects devices so they can communicate.\nNetworks allow sharing of information and resources.\nSecure networks help protect sensitive data.");

            BotResponses.Add("cloud",
                "Cloud computing stores files and services online.\nIt allows users to access data from different devices.\nCloud security is important for protecting information.");

            BotResponses.Add("breach",
                "A data breach happens when information is exposed or stolen.\nBreaches can affect businesses and individuals.\nStrong security practices reduce the risk of breaches.");

            BotResponses.Add("identity",
                "Identity theft happens when someone steals personal information.\nCriminals may use stolen identities for fraud.\nProtecting passwords and documents helps prevent identity theft.");

            BotResponses.Add("botnet",
                "A botnet is a group of infected devices controlled by hackers.\nBotnets are often used for cyber attacks.\nKeeping devices secure helps prevent infections.");

            BotResponses.Add("trojan",
                "A Trojan is malware disguised as useful software.\nUsers may install it without knowing it is harmful.\nAlways download software from trusted sources.");

            BotResponses.Add("spyware",
                "Spyware secretly monitors user activity.\nIt can collect passwords and personal information.\nAntivirus software can help detect spyware.");

            BotResponses.Add("worm",
                "A computer worm spreads automatically across networks.\nIt does not need user interaction to spread.\nWorms can slow down systems and networks.");

            BotResponses.Add("cookies",
                "Cookies are small files saved by websites.\nThey remember user settings and browsing activity.\nUsers can manage cookies in browser settings.");

            BotResponses.Add("cybercrime",
                "Cybercrime involves illegal activities using computers or the internet.\nExamples include hacking, scams, and identity theft.\nCybersecurity helps reduce cybercrime risks.");

            BotResponses.Add("sql",
                "SQL injection attacks target databases.\nAttackers insert harmful commands into applications.\nSecure coding helps prevent SQL injection attacks.");

            BotResponses.Add("xss",
                "Cross-site scripting is a web security vulnerability.\nAttackers inject harmful scripts into websites.\nDevelopers use validation to reduce XSS attacks.");

            BotResponses.Add("ethical",
                "Ethical hackers test systems legally.\nThey help organizations find weaknesses before criminals do.\nEthical hacking improves cybersecurity protection.");

            BotResponses.Add("security",
                "Security protects systems, devices, and information.\nGood security reduces risks from cyber attacks.\nEveryone should practice safe online behavior.");
        }


        // ================= EXIT CHECK =================

        public bool ExitBot(string message)
        {
            List<string> exitWords = new List<string>
            {
                "bye",
                "goodbye",
                "stop",
                "exit"
            };

            return exitWords.Any(word =>
                message.ToLower().Contains(word));
        }


        // ================= NAME VALIDATION =================

        public bool CheckUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            if (userName.Contains("  "))
                return false;

            if (Regex.IsMatch(userName, @"[0-9!@#$%^&*(),.?\:{ }|<>]"))
                return false;

            return true;
        }


        // ================= QUESTION VALIDATION =================

        public bool VerifyUserQuestion(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return false;

            if (question.Contains("  "))
                return false;

            return true;
        }
    }
}
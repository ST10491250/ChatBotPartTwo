using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotPartTwo
{
    public class RandomResponse
    {
        private readonly Random random;

        private readonly Dictionary<string, List<string>> responses;

        public RandomResponse()
        {
            random = new Random();

            responses = LoadResponses();
        }


        // ================= LOAD RESPONSES =================

        private Dictionary<string, List<string>> LoadResponses()
        {
            return new Dictionary<string, List<string>>
    {
        {
            "password|passwords|login|credentials|passcode|account password",
            new List<string>
            {
                "Use strong passwords with symbols and numbers.",
                "Avoid reusing passwords on multiple websites.",
                "Enable two-factor authentication for extra safety."
            }
        },

        {
            "privacy|private|data privacy|online privacy|personal data|privacy settings",
            new List<string>
            {
                "Review your privacy settings regularly.",
                "Avoid sharing sensitive information publicly.",
                "Use secure websites when entering personal data."
            }
        },

        {
            "scam|scams|fraud|fake message|fake email|phishing|money scam|job scam",
            new List<string>
            {
                "Be careful of suspicious emails and links.",
                "Scammers often pretend to be trusted companies.",
                "Never share OTPs or banking details online."
            }
        },

        {
            "vpn|virtual private network|secure connection|hide ip|internet privacy",
            new List<string>
            {
                "VPNs help secure your internet connection.",
                "Use trusted VPN providers only.",
                "VPNs are useful when using public Wi-Fi."
            }
        },

        {
            "malware|virus|trojan|worm|spyware|ransomware",
            new List<string>
            {
                "Avoid downloading files from unknown sources.",
                "Keep your antivirus updated at all times.",
                "Do not open suspicious attachments or links."
            }
        },

        {
            "hacking|hack|hacker|unauthorized access|bruteforce|brute force",
            new List<string>
            {
                "Use strong and unique passwords.",
                "Enable two-factor authentication.",
                "Never share your login details.",
                "Keep your software updated regularly."
            }
        },

        {
            "firewall|network firewall|security wall|traffic filter",
            new List<string>
            {
                "Firewalls help block unwanted traffic.",
                "Always keep your firewall enabled.",
                "Check firewall logs for suspicious activity."
            }
        },

        {
            "social engineering|social|manipulation|impersonation|trick",
            new List<string>
            {
                "Never trust urgent requests for information.",
                "Always verify who is contacting you.",
                "Do not share personal details with strangers.",
                "Attackers use trust to manipulate victims."
            }
        },

        {
            "wifi|public wifi|free wifi|hotspot|open network",
            new List<string>
            {
                "Avoid using public Wi-Fi for banking.",
                "Use a VPN on public networks.",
                "Turn off auto-connect for Wi-Fi.",
                "Forget unknown networks after use."
            }
        }
    };
        }
        // ================= GET RANDOM RESPONSE =================

        public string GetRandomResponse(string userInput)
        {
            userInput = userInput.ToLower();

            foreach (var entry in responses)
            {
                // split triggers inside the key
                string[] triggers = entry.Key.Split('|');

                foreach (string trigger in triggers)
                {
                    if (userInput.Contains(trigger))
                    {
                        List<string> possibleResponses = entry.Value;

                        int randomIndex = random.Next(possibleResponses.Count);
                        return possibleResponses[randomIndex];
                    }
                }
            }

            return null;
        }


        // ================= GET ALL KEYWORDS =================

        public List<string> GetKeywords()
        {
            return responses.Keys.ToList();
        }
    }
}
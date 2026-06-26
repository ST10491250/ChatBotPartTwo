using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBotPartTwo
{
    public class QuizQuestion
    {

        //getters and setters
        public string Question { get; set; }
        public string CorrectChoice { get; set; }

        //get and set on the list
        public List<string> Choices { get; set; }

    }
}

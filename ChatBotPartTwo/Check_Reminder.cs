using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatBotPartTwo
{
    public class Check_Reminder
    {


        private List<string> descriptions = new List<string>();
        private List<string> Date = new List<string>();
        public string validateInput(string UserInput)
        {
            //check if empty or not.
            string storing = string.Empty;
            if (!string.IsNullOrEmpty(UserInput))
            {
                //return found.
                storing = "found";
            }
            else
            {
                //then return error message.
                storing = "Please add a task";
            }

            return storing;
        }


        //method to get number 

        public string Get_Days(string Day)
        {
            string returnDays = string.Empty;
            //get the numbers from the users
            string getDay = Regex.Replace(Day, @"[^\d]", "");

            //check if the day is 0             
            if (getDay == "0")
            {
                returnDays = "today";
            }
            else
            {
                returnDays = getDay;
            }
            return returnDays;
        }//end


        //method to store today's date


        public string today_date(string descript, string date)
        {
            //validate
            if (date == "today")
            {
                //get the date

                DateTime today_date = DateTime.Now.Date;
                string formate_date = today_date.ToString("yyyy-MM-dd");

                //ADD ALL
                descriptions.Add(descript);
                Date.Add(formate_date);

                return "Nice I wil remind you in " + date;
            }
            else
            {
                return "error";
            }
            // then add both description and date

        }

        //creating a method that gets the remind days

        public string get_RemindDate(string descript, string date)
        {

            DateTime currentDate = DateTime.Now.Date;

            //THEN FORMAT
            string format_date = currentDate.ToString("yyyy-MM-dd");

            //get the day in the format

            string find_day = format_date.Substring(8, 2);

            //get date from 2 to 8
            string final_date = format_date.Substring(0, 8);

            int total_days = int.Parse(find_day) + int.Parse(date);

            string store_date = final_date + total_days;
            return store_date;
        }
    } //

}
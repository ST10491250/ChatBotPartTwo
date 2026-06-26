using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatBotPartTwo
{
    public partial class MainWindow : Window
    {
        private MediaPlayer Audio_greet;
   
        dbConnect db = new dbConnect();
        private readonly Prompting chatbot;

        private bool waitingForReminder = false;
        private int pendingTaskId = 0;


        //declaring all global variables and instances for mini game
        //list [generic]
        private List<QuizQuestion> quizData;

        Check_Reminder Reminder = new Check_Reminder();
        string hold_userTask = string.Empty;

        //declared a generic collection that will store the tasks
        private List<string> taskList = new List<string>();
        List<string> activityLog = new List<string>();
        //variables
        private int questionIndex = 0;
        private int currentScore = 0;

        //buttons
        private Button selectedChoice = null;
        private Button correctChoiceButton = null;

        public MainWindow()
        {
            InitializeComponent();

            chatbot = new Prompting();
            //calling a method that creates the table
            db.CreateTable();   
            // Optional welcome message
            ChatList.Items.Add("Bot: Welcome to the Cyber Security Awareness Bot!");
            ChatList.Items.Add("Bot: Please enter your name first.");
            greet();
            LoadQuizData();
            ShowQuiz();
        }


        // HANDLE USER INPUT 

        private void HandleUserInput(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text.Trim();

            // Prevent empty messages
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                MessageBox.Show(
                    "Please enter a message.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Add user message to chat
            ChatList.Items.Add($"You: {userMessage}");


            // Get bot response
            string botResponse = chatbot.GetBotResponse(userMessage);

            // Add bot response
            ChatList.Items.Add($"Bot: {botResponse}");

            ChatList.ScrollIntoView(
             ChatList.Items[ChatList.Items.Count - 1]);

            // Exit logic
            if (botResponse.Contains("Goodbye"))
            {
                Application.Current.Shutdown();
            }

            // Clear textbox
            UserInput.Clear();
        }


        // ================= NAVIGATE HOME =================

        private void Navigate_home(object sender, RoutedEventArgs e)
        {
            home_page.Visibility = Visibility.Visible;

            chats_page.Visibility = Visibility.Collapsed;

            exit_page.Visibility = Visibility.Collapsed;

            quiz_page.Visibility = Visibility.Collapsed;
            addTask_page.Visibility = Visibility.Collapsed;

            activitylog_page.Visibility = Visibility.Collapsed;
        }


        // ================= NAVIGATE CHATS =================

        private void navigate_chats(object sender, RoutedEventArgs e)
        {
            chats_page.Visibility = Visibility.Visible;

            home_page.Visibility = Visibility.Collapsed;

            exit_page.Visibility = Visibility.Collapsed;
            quiz_page.Visibility = Visibility.Collapsed;
            addTask_page.Visibility = Visibility.Collapsed;

            activitylog_page.Visibility = Visibility.Collapsed;
        }

        private void navigate_quiz(object sender, RoutedEventArgs e)
        {
            AddToActivityLog("Quiz started");
            quiz_page.Visibility = Visibility.Visible;
            addTask_page.Visibility = Visibility.Collapsed;
            
            activitylog_page.Visibility = Visibility.Collapsed;

            chats_page.Visibility = Visibility.Collapsed;
            home_page.Visibility = Visibility.Collapsed;

            exit_page.Visibility = Visibility.Collapsed;
        }
        private void navigate_activityLog(object sender, RoutedEventArgs e)
        {
            quiz_page.Visibility = Visibility.Collapsed;
            addTask_page.Visibility = Visibility.Collapsed;

            activitylog_page.Visibility = Visibility.Visible;
            chats_page.Visibility = Visibility.Collapsed;

            home_page.Visibility = Visibility.Collapsed;

            exit_page.Visibility = Visibility.Collapsed;
        }
        private void Navigate_task(object sender, RoutedEventArgs e)
        {
            quiz_page.Visibility = Visibility.Collapsed;
            addTask_page.Visibility = Visibility.Visible;

            activitylog_page.Visibility = Visibility.Collapsed;
            chats_page.Visibility = Visibility.Visible;

            home_page.Visibility = Visibility.Collapsed;

            exit_page.Visibility = Visibility.Collapsed;
        }

        // ================= EXIT APP =================

        private void Exit_app(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit Application",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }





        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string input = user_question.Text.Trim();

            // --- If we're waiting for a reminder after adding a task ---
            if (waitingForReminder)
            {
                // The user is replying to "Would you like a reminder?"
                string lower = input.ToLower();

                // Check if they say "yes" (or contain "remind")
                if (lower.Contains("yes") || lower.Contains("remind"))
                {
                    // Try to extract number of days
                    string hold_day = Reminder.Get_Days(input);
                    DateTime? reminderDate = null;

                    if (hold_day == "today")
                        reminderDate = DateTime.Today;
                    else if (int.TryParse(hold_day, out int days))
                        reminderDate = DateTime.Today.AddDays(days);
                    else
                    {
                        show_chats.Items.Add("Bot: I couldn't understand the days. Say 'remind me in X days' or 'remind me today'.");
                        user_question.Clear();
                        return;
                    }

                    // Update the specific task
                    if (reminderDate.HasValue)
                    {
                        db.UpdateReminder(pendingTaskId, reminderDate.Value);
                        show_chats.Items.Add($"Bot: Reminder set for task ID {pendingTaskId} on {reminderDate.Value.ToShortDateString()}.");
                        AddToActivityLog($"Reminder set for task {pendingTaskId}.");
                        RefreshTaskDisplay();
                    }
                }
                else if (lower.Contains("no") || lower.Contains("cancel"))
                {
                    show_chats.Items.Add("Bot: No reminder set.");
                    AddToActivityLog($"No reminder for task {pendingTaskId}.");
                }
                else
                {
                    show_chats.Items.Add("Bot: Please answer 'yes' or 'no'.");
                    user_question.Clear();
                    return;
                }

                // Reset the waiting state
                waitingForReminder = false;
                pendingTaskId = 0;
                user_question.Clear();
                return;
            }

            // --- Normal command processing ---

            // Validation (optional – skip if not needed)
            if (Reminder.validateInput(input) != "found")
            {
                MessageBox.Show(Reminder.validateInput(input));
                return;
            }

            // Show tasks / activity log (simplified)
            if (input.Contains("show my tasks") || input.Contains("list reminders"))
            {
                RefreshTaskDisplay();
                show_chats.Items.Add("Bot: Tasks listed above.");
                user_question.Clear();
                return;
            }

            if (input.Contains("what have you done") || input.Contains("show activity log"))
            {
                // Clear and populate the activity list
                activityList.Items.Clear();

                if (activityLog.Count == 0)
                {
                    activityList.Items.Add("No activity recorded yet.");
                }
                else
                {
                    // Showing last 5 entries 
                    var recentActivities = activityLog.TakeLast(5).Reverse();
                    foreach (var activity in recentActivities)
                    {
                        activityList.Items.Add(activity);
                    }
                }

                // Navigate to the activity log page
                quiz_page.Visibility = Visibility.Collapsed;
                addTask_page.Visibility = Visibility.Collapsed;
                activitylog_page.Visibility = Visibility.Visible;
                chats_page.Visibility = Visibility.Collapsed;
                home_page.Visibility = Visibility.Collapsed;
                exit_page.Visibility = Visibility.Collapsed;

                user_question.Clear();
                return;
            }

            // ADD TASK 
            if (input.Contains("add task"))
            {
                string description = input.Replace("add task", "").Trim();
                if (string.IsNullOrEmpty(description))
                {
                    show_chats.Items.Add("Bot: Please provide a description.");
                    user_question.Clear();
                    return;
                }

                // Adding  to database 
                db.AddTask(description, description, null);
                pendingTaskId = db.GetLastTaskID();

                RefreshTaskDisplay();
                AddToActivityLog($"Task added: {description}");

                // Ask for reminder
                show_chats.Items.Add($"Bot: Task '{description}' added successfully.");
                show_chats.Items.Add("Bot: Would you like to set a reminder? (Say 'yes' or 'no')");

                // Set the waiting flag
                waitingForReminder = true;
                user_question.Clear();
                return;
            }

            
            if (input.Contains("remind"))
            {
                
                var tasks = db.GetTasks().Where(t => t.Status != "Completed").ToList();
                if (tasks.Count == 0)
                {
                    show_chats.Items.Add("Bot: No pending tasks.");
                    user_question.Clear();
                    return;
                }

                string hold_day = Reminder.Get_Days(input);
                DateTime? date = null;
                if (hold_day == "today") date = DateTime.Today;
                else if (int.TryParse(hold_day, out int d)) date = DateTime.Today.AddDays(d);
                else
                {
                    show_chats.Items.Add("Bot: I couldn't understand the days.");
                    user_question.Clear();
                    return;
                }

                foreach (var t in tasks) db.UpdateReminder(t.Id, date.Value);
                RefreshTaskDisplay();
                show_chats.Items.Add($"Bot: Reminder set for all pending tasks on {date.Value.ToShortDateString()}.");
                user_question.Clear();
                return;
            }

            
            show_chats.Items.Add("Bot: I don't understand. Try 'add task ...' or 'show my tasks'.");
            user_question.Clear();
        }


        // when the task is double clicked on the list view
        private void show_chats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (show_chats.SelectedItem == null) return;

            string selectedEntry = show_chats.SelectedItem.ToString();
           
            int start = selectedEntry.IndexOf('[') + 1;
            int end = selectedEntry.IndexOf(']');
            if (start <= 0 || end <= start) return;

            string idStr = selectedEntry.Substring(start, end - start);
            if (!int.TryParse(idStr, out int taskId)) return;

            // Checking the status if contains "Completed" (or "Done") then delete, else mark complete
            if (selectedEntry.Contains("Completed"))
            {
                db.DeleteTask(taskId);
                AddToActivityLog($"Task {taskId} deleted.");
            }
            else
            {
                db.MarkCompleted(taskId);
                AddToActivityLog($"Task {taskId} marked as completed.");
            }

            RefreshTaskDisplay();
        }


        //method that handles adding activity logs
        private void AddToActivityLog(string entry)
        {
            string logEntry = $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {entry}";

            activityLog.Add(logEntry);

            if (activityLog.Count > 50)
            {
                activityLog.RemoveAt(0);
            }
        }




        //method to show the quiz on the buttons
        private void ShowQuiz()
        {

            //check if the user is not done playing
            if (questionIndex >= quizData.Count)
            {
                AddToActivityLog($"Quiz completed. Score: {currentScore}/10");
                //show complete message
                MessageBox.Show("You already completed the game with a score of: " + currentScore + "/10");
                //then reset the game
                currentScore = 0;
                currentScore = 0;
                questionIndex = 0;
                ShowQuiz();
                //stop the execution
                return;

            }

            //get the current index quiz
            correctChoiceButton = null;
            selectedChoice = null;

            //then get all the questions values
            var currentQuiz = quizData[questionIndex];

            //display the questions to the user
            DisplayedQuestion.Text = currentQuiz.Question;

            // Combine all answers (including the correct one) and shuffle
            var allChoices = new List<string>(currentQuiz.Choices)
            {
                currentQuiz.CorrectChoice
            };

            var shuffledChoices = allChoices.OrderBy(_ => Guid.NewGuid()).ToList();

            // Assign shuffled answers to buttons
            FirstChoiceButton.Content = shuffledChoices[0];
            SecondChoiceButton.Content = shuffledChoices[1];
            ThirdChoiceButton.Content = shuffledChoices[2];
            FourthChoiceButton.Content = shuffledChoices[3];

            // Identify which button has the correct answer
            if ((string)FirstChoiceButton.Content == currentQuiz.CorrectChoice) correctChoiceButton = FirstChoiceButton;
            else if ((string)SecondChoiceButton.Content == currentQuiz.CorrectChoice) correctChoiceButton = SecondChoiceButton;
            else if ((string)ThirdChoiceButton.Content == currentQuiz.CorrectChoice) correctChoiceButton = ThirdChoiceButton;
            else if ((string)FourthChoiceButton.Content == currentQuiz.CorrectChoice) correctChoiceButton = FourthChoiceButton;

            clearStyle();

        }//end of Show quiz method

        //method to reset the buttons
        private void clearStyle()
        {

            //use foreach to reset
            foreach (Button choice in new[] { FirstChoiceButton, SecondChoiceButton, ThirdChoiceButton, FourthChoiceButton })
            {

                choice.Background = Brushes.LightGray;

            }

        }//end of the clear style method

        //method to load the quiz data
        private void LoadQuizData()
        {

            //store info
            quizData = new List<QuizQuestion>
            {
                //1st question
                new QuizQuestion
                {

                    Question = "What does the term malware refer to?",
                    CorrectChoice = "Malicious software designed to harm a system",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "A type of hardware","An encryption algorithm","A secure firewall"
                    }

                },// end of question
                
                //2nd question
                new QuizQuestion
                {

                    Question = "What is phishing?",
                    CorrectChoice = "Tricking people into revealing personal information via fake messages",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "Encrypting emails for protection","Testing network speeds","Blocking spam emails with firewalls"
                    }

                },// end of question
                
                //3rd question
                new QuizQuestion
                {

                    Question = " What is the main purpose of a firewall?",
                    CorrectChoice = " To block unauthorized access to a network",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "To boost internet speed","To store passwords","To perform automatic software updates"
                    }

                },// end of question
                                
                //4th question
                new QuizQuestion
                {

                    Question = "What does HTTPS stand for?",
                    CorrectChoice = "Hypertext Transfer Protocol Secure",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "High Transfer Protocol Server","Hyperlink Transfer Protection Software","Host Transfer Packet Service"
                    }

                },// end of question
                
                //5th question
                new QuizQuestion
                {

                    Question = "What is a strong password?\r\n",
                    CorrectChoice = "A long, complex password with letters, numbers, and symbols ",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "A password with only your name","The word \"password123\""," Your date of birth"
                    }

                },// end of question
                
                //6th question
                new QuizQuestion
                {

                    Question = "What is two-factor authentication (2FA)?",
                    CorrectChoice = "A security process requiring two types of verification",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "A software update","A type of virus scan","A backup system"
                    }

                },// end of question
                
                //7th question
                new QuizQuestion
                {

                    Question = "What is a DDoS attack?",
                    CorrectChoice = "An attempt to overwhelm a server with traffic",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "A way to safely transfer data","A virus removal tool","A password recovery method"
                    }

                },// end of question
                
                //8th question
                new QuizQuestion
                {

                    Question = "What is the role of encryption in cybersecurity?",
                    CorrectChoice = "To convert data into unreadable form for protection",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "To make files smaller","To delete old data","To improve network speed"
                    }

                },// end of question
                
                //9th question
                new QuizQuestion
                {

                    Question = "Which of the following is an example of social engineering?",
                    CorrectChoice = "Pretending to be IT support to get login details",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "Scanning for open ports","Updating antivirus software","Installing a VPN"
                    }

                },// end of question
                
                //10th question
                new QuizQuestion
                {

                    Question = "What does VPN stand for?",
                    CorrectChoice = "Virtual Private Network",
                    Choices = new List<string>
                    {
                        //incorrect choices
                        "Very Powerful Node","Virus Protection Network","Verified Protocol Network"
                    }

                },// end of question

            };

        }//end of the method load quiz data or info


        private void HandleAnswerSelection(object sender, RoutedEventArgs e)
        {

            //use the object name to get the selection button
            selectedChoice = sender as Button;

            string chosen = selectedChoice.Content.ToString();

            //then check with the correct anwser on the current quiz
            string correct = quizData[questionIndex].CorrectChoice;

            //then check if correct or not by if statement
            if (chosen == correct)
            {
                //then set the button background color
                selectedChoice.Background = Brushes.Green;

                //assigning to hold
                correctChoiceButton = selectedChoice;
            }
            else
            {
                //if incorrect
                selectedChoice.Background = Brushes.DarkRed;
                correctChoiceButton = selectedChoice;
            }
        }//end of handle answer selection event handler

        //event handler for the next button
        private void HandleNextQuestion(object sender, RoutedEventArgs e)
        {
            //check if the user selected one of the choices
            if (selectedChoice == null)
            {
                //then show error message
                MessageBox.Show("choose one of the four choices");
            }
            else
            {
                //then add points, and only if correct
                string chosen = selectedChoice.Content.ToString();
                string correct = quizData[questionIndex].CorrectChoice;

                //check if correct
                if (chosen == correct)
                {
                    //then add point
                    currentScore++;

                    //then show the score
                    DisplayScore.Text = "Score : " + currentScore;

                    //then move to the next index question
                    questionIndex++;

                    //then show the question again for the next one
                    ShowQuiz();
                }
                else
                {
                    //move to the next question
                    questionIndex++;
                    ShowQuiz();
                }

            }

        }//end of handle next question event handler

        private void ViewActivityLog_Click(object sender, RoutedEventArgs e)
        {
            activityList.Items.Clear();

            if (activityLog.Count == 0)
            {
                activityList.Items.Add("No activity recorded yet.");
                return;
            }

            // Show only last 5 actions
            var recentActivities = activityLog
                                    .TakeLast(5)
                                    .Reverse();

            foreach (var activity in recentActivities)
            {
                activityList.Items.Add(activity);
            }
        }

        public void greet()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BotRecording.wav");

            if (System.IO.File.Exists(filePath))
            {
                Audio_greet = new MediaPlayer();
                Audio_greet.Open(new Uri(filePath, UriKind.Absolute));
                Audio_greet.MediaEnded += Audio_greet_MediaEnded;
                Audio_greet.MediaFailed += (s, e) =>
                {
                    // If playback fails, skip the audio and show the page
                    ShowChatPage();
                };
                Audio_greet.Play();
            }
            else
            {
                // File not found – skip audio and show page immediately
                ShowChatPage();
            }
        }

        private void ShowChatPage()
        {
            home_page.Visibility = Visibility.Hidden;
            chats_page.Visibility = Visibility.Visible;
        }

        private void Audio_greet_MediaEnded(object sender, EventArgs e)
        {
            Audio_greet.MediaEnded -= Audio_greet_MediaEnded;
            ShowChatPage();
        }



        private void RefreshTaskDisplay()
        {
            show_chats.Items.Clear();
            var tasks = db.GetTasks();
            foreach (var t in tasks)
            {
                string reminder = t.ReminderDate?.ToShortDateString() ?? "No reminder";
                string status = t.Status ?? "Pending";
                show_chats.Items.Add($"[{t.Id}] {t.Title} - {t.Description} | Reminder: {reminder} | {status}");
            }
        }
    }



}
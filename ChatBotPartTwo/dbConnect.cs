using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ChatBotPartTwo
{
    public class dbConnect
    {
        private readonly string connection =
            @"Server=(localdb)\CyberSecurityBOT;Database=ChatBotTasks;Trusted_Connection=True;";


        // CREATING THE TABLE

        public void CreateTable()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = @"
                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME='Tasks'
                )
                BEGIN
                    CREATE TABLE Tasks
                    (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(100),
                        Description NVARCHAR(MAX),
                        ReminderDate DATETIME NULL,
                        Status NVARCHAR(20) DEFAULT 'Pending'
                    )
                END";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        }

        
        // INSERTING INTO THE  TASK TABLE
   
        public void AddTask(string title,
                            string description,
                            DateTime? reminder)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = @"INSERT INTO Tasks
                                (Title,Description,ReminderDate,Status)
                                VALUES
                                (@title,@description,@reminder,'Pending')";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", description);

                if (reminder == null)
                    cmd.Parameters.AddWithValue("@reminder", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@reminder", reminder);

                cmd.ExecuteNonQuery();
            }
        }

     
        // READ ALL TASKS
        
        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = "SELECT * FROM Tasks";

                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TaskItem task = new TaskItem();

                    task.Id = Convert.ToInt32(reader["Id"]);
                    task.Title = reader["Title"].ToString();
                    task.Description = reader["Description"].ToString();

                    if (reader["ReminderDate"] == DBNull.Value)
                        task.ReminderDate = null;
                    else
                        task.ReminderDate = Convert.ToDateTime(reader["ReminderDate"]);

                    task.Status = reader["Status"].ToString();

                    tasks.Add(task);
                }
            }

            return tasks;
        }

  
        // DELETE TASK
       
        public void DeleteTask(int id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = "DELETE FROM Tasks WHERE Id=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateReminder(int id, DateTime reminder)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query =
                "UPDATE Tasks SET ReminderDate=@date WHERE Id=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@date", reminder);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }


        public int GetLastTaskID()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query = "SELECT MAX(Id) FROM Tasks";

                SqlCommand cmd = new SqlCommand(query, con);

                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                    return Convert.ToInt32(result);

                return 0;
            }
        }

  
        // MARK COMPLETE
       
        public void MarkCompleted(int id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();

                string query =
                    "UPDATE Tasks SET Status='Completed' WHERE Id=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
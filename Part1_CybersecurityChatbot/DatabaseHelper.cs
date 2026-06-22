using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=chatbot_db;User=chatbot_user;Password=ChatBot123!;Port=3306;";

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("✓ Database connected successfully!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Database Error: {ex.Message}");
                return false;
            }
        }

        public bool AddTask(string title, string description, DateTime? reminderDate)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @desc, @reminder)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@desc", description ?? "");
                    cmd.Parameters.AddWithValue("@reminder", reminderDate.HasValue ? (object)reminderDate.Value : DBNull.Value);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        LogActivity("Task Added", $"Added task: '{title}'");
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding task: {ex.Message}");
                return false;
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT task_id, title, description, reminder_date, is_completed FROM tasks ORDER BY created_at DESC";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskItem
                            {
                                TaskId = reader.GetInt32("task_id"),
                                Title = reader.GetString("title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                                ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminder_date")) ? (DateTime?)null : reader.GetDateTime("reminder_date"),
                                IsCompleted = reader.GetBoolean("is_completed")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting tasks: {ex.Message}");
            }
            return tasks;
        }

        public bool CompleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE tasks SET is_completed = TRUE WHERE task_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        LogActivity("Task Completed", $"Completed task #{taskId}");
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error completing task: {ex.Message}");
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM tasks WHERE task_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        LogActivity("Task Deleted", $"Deleted task #{taskId}");
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
                return false;
            }
        }

        public void LogActivity(string actionType, string description)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO activity_log (action_type, description) VALUES (@type, @desc)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@type", actionType);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging activity: {ex.Message}");
            }
        }

        public List<ActivityLogItem> GetActivityLog(int limit = 10)
        {
            List<ActivityLogItem> logs = new List<ActivityLogItem>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT action_type, description, timestamp FROM activity_log ORDER BY timestamp DESC LIMIT {limit}";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(new ActivityLogItem
                            {
                                ActionType = reader.GetString("action_type"),
                                Description = reader.GetString("description"),
                                Timestamp = reader.GetDateTime("timestamp")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting activity log: {ex.Message}");
            }
            return logs;
        }

        public List<QuizQuestion> GetQuizQuestions()
        {
            List<QuizQuestion> questions = new List<QuizQuestion>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM quiz_questions ORDER BY RAND()";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new QuizQuestion
                            {
                                QuestionId = reader.GetInt32("question_id"),
                                QuestionText = reader.GetString("question_text"),
                                OptionA = reader.GetString("option_a"),
                                OptionB = reader.GetString("option_b"),
                                OptionC = reader.GetString("option_c"),
                                OptionD = reader.GetString("option_d"),
                                CorrectAnswer = reader.GetString("correct_answer")[0],
                                Explanation = reader.GetString("explanation")
                            });
                        }
                    }
                }
                LogActivity("Quiz Started", $"Loaded {questions.Count} questions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting quiz questions: {ex.Message}");
            }
            return questions;
        }
    }

    public class TaskItem
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ActivityLogItem
    {
        public string ActionType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class QuizQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public char CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public bool IsCorrect(char userAnswer)
        {
            return char.ToUpper(userAnswer) == char.ToUpper(CorrectAnswer);
        }
    }
}
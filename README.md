# 🔒 Cybersecurity Awareness Chatbot

A South African-focused cybersecurity awareness chatbot built with C# (.NET) and WPF, featuring task management, an interactive quiz, NLP-based responses, and MySQL database integration.


## 📋 Features

- **Task Assistant**: Add, view, complete, and delete cybersecurity tasks (stored in MySQL)
- **Quiz Game**: Test your cybersecurity knowledge with 12+ questions
- **NLP Simulation**: Understands different ways of phrasing requests
- **Activity Log**: Tracks all chatbot actions
- **South African Context**: References SARS, SASSA, local banks, and POPIA
- **Voice Greeting**: Plays an audio welcome message
- **GUI Interface**: User-friendly Windows application


## 🛠️ Prerequisites

Before running this application, you need to install:

1. **.NET SDK 10.0** (or compatible version)
   - Download: https://dotnet.microsoft.com/download

2. **MySQL Server** (8.0 or later)
   - Download: https://dev.mysql.com/downloads/installer/

3. **MySQL Workbench** (for database setup)
   - Usually included with MySQL installer

4. **Visual Studio 2022/2026** (optional, for editing code)
   - Download: https://visualstudio.microsoft.com/


## 🗄️ Database Setup

### Step 1: Open MySQL Workbench

Connect to your local MySQL instance.

### Step 2: Create Database and User

Run the following SQL commands:

sql
-- Create user
CREATE USER 'chatbot_user'@'localhost' IDENTIFIED BY 'ChatBot123!';

-- Create database
CREATE DATABASE IF NOT EXISTS chatbot_db;

-- Grant permissions
GRANT ALL PRIVILEGES ON chatbot_db.* TO 'chatbot_user'@'localhost';
FLUSH PRIVILEGES;

### Create Tables

USE chatbot_db;

-- Tasks table
CREATE TABLE IF NOT EXISTS tasks (
    task_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    reminder_date DATE,
    is_completed BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Activity log table
CREATE TABLE IF NOT EXISTS activity_log (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    action_type VARCHAR(100),
    description TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Quiz questions table
CREATE TABLE IF NOT EXISTS quiz_questions (
    question_id INT AUTO_INCREMENT PRIMARY KEY,
    question_text TEXT NOT NULL,
    option_a VARCHAR(255),
    option_b VARCHAR(255),
    option_c VARCHAR(255),
    option_d VARCHAR(255),
    correct_answer CHAR(1),
    explanation TEXT
);

-- Insert sample quiz questions
INSERT INTO quiz_questions (question_text, option_a, option_b, option_c, option_d, correct_answer, explanation) VALUES
('What should you do if you receive an email asking for your password?', 'Reply with your password', 'Delete the email', 'Report the email as phishing', 'Ignore it', 'C', 'Reporting phishing emails helps prevent scams and protects others.'),
('What makes a strong password?', 'Your name and birthday', 'At least 8 characters with letters, numbers, and symbols', 'Password123', 'The same password for all accounts', 'B', 'Strong passwords should be complex and unique for each account.'),
('What is phishing?', 'A type of fishing sport', 'A scam to steal personal information', 'A computer virus', 'A software update', 'B', 'Phishing is a fraudulent attempt to obtain sensitive information.'),
('Should you use public Wi-Fi for online banking?', 'Yes, it is always safe', 'No, it is not secure', 'Only on weekends', 'Only with VPN', 'D', 'Public Wi-Fi is risky. Always use a VPN for sensitive activities.'),
('What is two-factor authentication (2FA)?', 'Using two passwords', 'An extra layer of security requiring two forms of verification', 'Two antivirus programs', 'Two email accounts', 'B', '2FA adds an extra layer of security beyond just a password.'),
('Is it safe to click on links in emails from unknown senders?', 'Yes, always', 'No, never', 'Only if it looks official', 'Sometimes', 'B', 'Never click links from unknown senders as they could be phishing attempts.'),
('What should you do before downloading an app?', 'Download immediately', 'Check reviews and permissions', 'Ask a friend', 'Nothing', 'B', 'Always verify the app legitimacy and check permissions.'),
('How often should you update your passwords?', 'Never', 'Every 3-6 months', 'Every 10 years', 'Only when hacked', 'B', 'Regular password updates help maintain security.'),
('What is malware?', 'A type of hardware', 'Malicious software designed to harm your computer', 'A browser extension', 'An email service', 'B', 'Malware is software intentionally designed to cause damage.'),
('Should you share your personal information on social media?', 'Yes, everything', 'No, be very careful', 'Only your address', 'Only your phone number', 'B', 'Limit personal information to protect your privacy and security.'),
('What is ransomware?', 'Free software', 'Malware that locks your files and demands payment', 'A type of antivirus', 'A gaming software', 'B', 'Ransomware encrypts your files and demands payment.'),
('Is it safe to use the same password for multiple accounts?', 'Yes, it is convenient', 'No, each account should have a unique password', 'Only for social media', 'Only for shopping sites', 'B', 'Using unique passwords prevents a breach from compromising all accounts.');

### Configure Database Connection if needed

File:  CybersecurityChatbot.GUI/DatabaseHelper.cs
private string connectionString = "Server=localhost;Database=chatbot_db;User=chatbot_user;Password=ChatBot123!;Port=3306;";

Change Password=ChatBot123! to your actual password.

### Run the app

# Copy your path and paste on command prompt

C:\Users\morel>cd C:\Users\morel\Downloads\CybersecurityChatbotPOEPart2-master

C:\Users\morel\Downloads\CybersecurityChatbotPOEPart2-master>dotnet build
Restore complete (2,5s)
  CybersecurityChatbot net10.0-windows succeeded (0,6s) → Part1_CybersecurityChatbot\bin\Debug\net10.0-windows\CybersecurityChatbot.dll
  CybersecurityChatbot.GUI net10.0-windows succeeded (1,1s) → CybersecurityChatbot.GUI\bin\Debug\net10.0-windows\CybersecurityChatbot.GUI.dll

Build succeeded in 4,9s

C:\Users\morel\Downloads\CybersecurityChatbotPOEPart2-master>cd CybersecurityChatbot.GUI

C:\Users\morel\Downloads\CybersecurityChatbotPOEPart2-master\CybersecurityChatbot.GUI>dotnet run

### Using The Application

Once the GUI opens:

Enter your name when prompted
Chat with the bot using the text input box
Access Task Assistant to add/view cybersecurity tasks
Start Quiz to test your cybersecurity knowledge
View Activity Log to see recent actions

### Error: "Database connection failed"
Solution:

Verify MySQL server is running
Check your username and password in DatabaseHelper.cs
Ensure the database chatbot_db exists
Confirm the user chatbot_user has proper permissions

### Error: "Table doesn't exist"
Solution:

Run the SQL CREATE TABLE scripts in MySQL Workbench
Ensure you're using the correct database: USE chatbot_db;

### Error: "It was not possible to find any compatible framework version"
Solution:

Install .NET 6.0 SDK or later from https://dotnet.microsoft.com/download

### Error: "Build failed with errors"
Solution:

Ensure all .cs files are in the correct project folder
Run dotnet restore before building
Check that DatabaseHelper.cs exists in the GUI project

















References
Freeman, E. and Robson, E. (2021) Head First Design Patterns: Building Extensible and Maintainable Object-Oriented Software. 2nd edn. Sebastopol: O'Reilly Media
Microsoft (2024) SoundPlayer Class — System.Media. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer [Accessed 20 Jun 2026].
Microsoft (2024) Windows Presentation Foundation (WPF) for .NET. Available at: https://learn.microsoft.com/en-us/dotnet/desktop/wpf [Accessed 20 Jun 2026].
National Cyber Security Centre (NCSC) (2023) Phishing Attacks: Defending Your Organisation. Available at: https://www.ncsc.gov.uk/guidance/phishing [Accessed 20 Jun 2026].
Nielsen, J. (1994) Usability Engineering. San Francisco: Morgan Kaufmann 
Picard, R.W. (1997) Affective Computing. Cambridge: MIT Press.
Russell, S. and Norvig, P. (2021) Artificial Intelligence: A Modern Approach. 4th edn. Hoboken: Pearson.
SANS Institute (2023) Security Awareness Training: Building a Security Culture. Available at: https://www.sans.org/security-awareness-training [Accessed 21 Jun 2026].
Weizenbaum, J. (1966) 'ELIZA — A Computer Program for the Study of Natural Language Communication Between Man and Machine', *Communications of the ACM*, 9(1), pp. 36–45.
adegeo (2026). Styles and templates - WPF. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview [Accessed 21 Jun 2026].
jwmsft (2024). ResourceDictionary and XAML resource references - Windows apps. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/xaml-resource-dictionary [Accessed 14 May 2026].
‌Picard, R. (n.d.). Affective Computing. [online] Available at: https://arl.human.cornell.edu/linked%20docs/Picard%20Affective%20Computing.pdf [Accessed 21 Jun 2026].
Sans.org. (2025). SANS 2023 Security Awareness Report: Managing Human Risk | SANS Institute. [online] Available at: https://www.sans.org/blog/sans-2023-security-awareness-report-managing-human-risk [Accessed 14 May 2026].

‌ NCSC (2023). NCSC Annual Review 2023. [online] www.ncsc.gov.uk. Available at: https://www.ncsc.gov.uk/collection/annual-review-2023 [Accessed 22 Jun 2026].

‌

‌

‌


CybersecurityChatbot – PROG6221 POE

GitHub Repository:
https://github.com/modestsnake/CybersecurityChatbotPOEPart2.git

This Cybersecurity Awareness Chatbot was built C# .NET 10 and is developed across three parts as part of PROG6221 module. The application educates users on cybersecurity topics which include phishing, password safety, malware, two-factor authentication, safe browsing and privacy

The Requirements: 
•	Windows 10 or newer
•	(.NET 10 SDK) https://dotnet.microsoft.com/en-us/download/dotnet/10.0 (Microsoft, 2026)
•	Visual Studio 2022 or newer with .NET desktop development workload

How to Clone the Repository
•	User must open Command Prompt and run git clone https://github.com/modestsnake/CybersecurityChatbotPOEPart2.git
•	Then cd CybersecurityChatbotPOEPart2

How to Run Part 2 - WPF GUI APP
1.	Open “CybersecurityChatbot.slnx” in Visual Studio
2.	Right-click “CybersecurityChatbot.GUI” in Solution Explorer
3.	Then click “Set as Startup Project”
4.	Press “F5” to build and run
Or
1.	Open command prompt and type dotnet run --project CybersecurityChatbot.GUI

Part 2 – The WPF GUI Application Features
Part 2 reveals a complete visual interface made with WPF. Among the updates of new interactive elements shaped for smoother navigation
Dark Theme Chat Interface
The application uses a dark green and black colour scheme with chat bubbles to display conversation history. WPF styles and resource dictionaries were used to maintain consistent visual design across all UI elements (Microsoft, 2024).

Event-Driven Architecture
The ChatBot class raises a ResponseGenerated event that the MainWindow subscribes to. This desouples the UI from the logic layer, following the observer design pattern (Freeman and Robson, 2021)

Sentiment Detection
A SentimentDetector class analyses user input for emotional keywords such as “worried”, “confused”, excited”, and “curious”. The chatbot responds empathetically before providing cybersecurity advice – a technique aligned with affective computing principles (Picard, 1997)

Memory Store
A MemoryStore class persists the user's name and stated interests across the conversation session using an in-memory dictionary. This enables context-aware responses, such as recalling a user's interest in phishing when they ask for a tip (Russell and Norvig, 2021).

Randomised Responses
The ResponseEngine stores multiple responses per topic (passwords, phishing, social engineering) and selects one at random using System.Random. This prevents repetitive answers and improves perceived intelligence (Weizenbaum, 1966).

Audio Greeting
On startup, the application plays a greeting.wav file using System.Media.SoundPlayer. The WAV file is generated using the Windows System.Speech.Synthesis.SpeechSynthesizer class (Microsoft, 2024).

Enter Key Support
Users can send messages by pressing Enter in addition to clicking the Send button, improving usability and accessibility (Nielsen, 1994).

Expanded Topic Coverage
Part 2 adds responses for VPN usage, encryption, firewall protection, and social engineering — broadening the educational scope of the chatbot (SANS Institute, 2023; NCSC, 2023).





















References
Freeman, E. and Robson, E. (2021) Head First Design Patterns: Building Extensible and Maintainable Object-Oriented Software. 2nd edn. Sebastopol: O'Reilly Media
Microsoft (2024) SoundPlayer Class — System.Media. Available at: https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer [Accessed 12 May 2026].
Microsoft (2024) Windows Presentation Foundation (WPF) for .NET. Available at: https://learn.microsoft.com/en-us/dotnet/desktop/wpf [Accessed 12 May 2026].
National Cyber Security Centre (NCSC) (2023) Phishing Attacks: Defending Your Organisation. Available at: https://www.ncsc.gov.uk/guidance/phishing [Accessed 13 May 2026].
Nielsen, J. (1994) Usability Engineering. San Francisco: Morgan Kaufmann 
Picard, R.W. (1997) Affective Computing. Cambridge: MIT Press.
Russell, S. and Norvig, P. (2021) Artificial Intelligence: A Modern Approach. 4th edn. Hoboken: Pearson.
SANS Institute (2023) Security Awareness Training: Building a Security Culture. Available at: https://www.sans.org/security-awareness-training [Accessed 14 May 2026].
Weizenbaum, J. (1966) 'ELIZA — A Computer Program for the Study of Natural Language Communication Between Man and Machine', *Communications of the ACM*, 9(1), pp. 36–45.
adegeo (2026). Styles and templates - WPF. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/styles-templates-overview [Accessed 14 May 2026].
jwmsft (2024). ResourceDictionary and XAML resource references - Windows apps. [online] Microsoft.com. Available at: https://learn.microsoft.com/en-us/windows/apps/develop/platform/xaml/xaml-resource-dictionary [Accessed 14 May 2026].
‌Picard, R. (n.d.). Affective Computing. [online] Available at: https://arl.human.cornell.edu/linked%20docs/Picard%20Affective%20Computing.pdf [Accessed 14 May 2026].
Sans.org. (2025). SANS 2023 Security Awareness Report: Managing Human Risk | SANS Institute. [online] Available at: https://www.sans.org/blog/sans-2023-security-awareness-report-managing-human-risk [Accessed 14 May 2026].

‌ NCSC (2023). NCSC Annual Review 2023. [online] www.ncsc.gov.uk. Available at: https://www.ncsc.gov.uk/collection/annual-review-2023 [Accessed 14 May 2026].

‌

‌

‌


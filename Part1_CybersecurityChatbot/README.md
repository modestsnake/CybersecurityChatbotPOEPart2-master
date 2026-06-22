# CybersecurityChatbot — Part 1

A C# .NET 8 Console Application that acts as a cybersecurity awareness chatbot.

## Features

- ASCII art cybersecurity logo on launch
- Voice greeting via `greeting.wav` (place the WAV file next to the `.exe`)
- Personalised experience — asks for your name and uses it throughout
- Keyword-based responses covering: passwords, phishing, safe browsing, malware, 2FA, scams, privacy
- Coloured console output (green for bot, yellow for user prompts, cyan for headers)
- Typing-effect delays for a realistic chat feel
- Input validation — handles empty input and unrecognised queries gracefully

## Project Structure

```
CybersecurityChatbot/
├── Program.cs          ← Entry point
├── ChatBot.cs          ← Conversation flow controller
├── ResponseEngine.cs   ← Keyword → response dictionary
├── UIHelper.cs         ← Console colours, dividers, input helpers
├── greeting.wav        ← (You supply this) Voice greeting audio
└── .github/
    └── workflows/
        └── dotnet.yml  ← GitHub Actions CI pipeline
```

## How to Run

1. Open `CybersecurityChatbot.csproj` in Visual Studio 2022
2. Press **F5** to build and run
3. (Optional) Place a `greeting.wav` file in the output folder (`bin/Debug/net8.0/`) for voice greeting

## Supported Chat Topics

| You type…       | Bot responds with…            |
|-----------------|-------------------------------|
| how are you     | Friendly status reply         |
| purpose / what can you do | Bot's capabilities |
| password        | Password safety tips          |
| phishing        | Phishing awareness tips       |
| safe browsing   | Safe browsing tips            |
| malware         | Malware prevention tips       |
| 2fa             | Two-factor authentication tips |
| scam            | Scam avoidance tips           |
| privacy         | Privacy protection tips       |
| exit / quit / bye | Farewell message + close    |

## GitHub Actions CI

The workflow at `.github/workflows/dotnet.yml` automatically builds the project on every push to `main`.

After your first push, go to **Actions** tab in GitHub to see the CI badge — screenshot it for your README as required.

## Creating a Voice Greeting WAV

You can record one for free:
- Windows: open **Voice Recorder** app → record → save as WAV
- Online: [ttsfree.com](https://ttsfree.com) → download as MP3 → convert to WAV with [cloudconvert.com](https://cloudconvert.com)
- Suggested text: *"Welcome to the Cybersecurity Awareness Chatbot. Stay safe online!"*

Rename the file to `greeting.wav` and copy it into your project folder, then set **Copy to Output Directory → Copy Always** in Visual Studio's file properties.

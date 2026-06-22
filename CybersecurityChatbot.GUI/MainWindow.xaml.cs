using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot.GUI
{
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot = new ChatBot();
            _chatBot.ResponseGenerated += OnBotResponse;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayGreeting();
            AddBotBubble(_chatBot.GetGreeting());
        }

        private void PlayGreeting()
        {
            string wavPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
            if (System.IO.File.Exists(wavPath))
            {
                try { new SoundPlayer(wavPath).Play(); } catch { }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMessage();
        }

        private void SendMessage()
        {
            string input = InputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;
            AddUserBubble(input);
            InputTextBox.Clear();
            _chatBot.ProcessInput(input);
        }

        private void OnBotResponse(string message)
        {
            Dispatcher.Invoke(() => AddBotBubble(message));
        }

        private void AddBotBubble(string text)
        {
            var label = new TextBlock
            {
                Text = "🤖 CyberBot",
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 200, 100)),
                Margin = new Thickness(2, 0, 0, 2),
                FontWeight = FontWeights.Bold
            };
            var message = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Segoe UI")
            };
            var bubble = new Border
            {
                Style = (Style)FindResource("BotBubble"),
                Child = new StackPanel { Children = { label, message } }
            };
            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToEnd();
        }

        private void AddUserBubble(string text)
        {
            string displayName = _chatBot.IsNameSet ? _chatBot.UserName : "You";
            var label = new TextBlock
            {
                Text = $"👤 {displayName}",
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0)),
                Margin = new Thickness(2, 0, 0, 2),
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            var message = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                FontFamily = new FontFamily("Segoe UI"),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            var bubble = new Border
            {
                Style = (Style)FindResource("UserBubble"),
                Child = new StackPanel { Children = { label, message } }
            };
            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
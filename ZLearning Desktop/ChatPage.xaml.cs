﻿using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ZLearning_Desktop
{
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage()
        {
            InitializeComponent();
        }
        public TelegramBotClient bot = new TelegramBotClient("1556766412:AAG245ESxF1ESl3d8vyHZ3hhzEgx2ukKXuI");
        List<long> idList = new List<long>();
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            messageTxt.Focus();
            bot.OnMessage += OnMessage;
            bot.StartReceiving();

        }
        private void OnMessage(object sender, MessageEventArgs e)
        {
            long id = e.Message.Chat.Id;
            string msg = e.Message.Text;
            string name = e.Message.Chat.FirstName + " " + e.Message.Chat.LastName;
            this.Dispatcher.Invoke(() =>
            {
                var textMessageFriend = new TextMessageFriend();
                textMessageFriend.Text = msg;
                textMessageFriend.Username = name;
                chatStackPanel.Children.Add(textMessageFriend);
                if(!idList.Contains(id)) idList.Add(id);
                memberCountTxt.Text = idList.Count.ToString() + " member";
            });
           
        }
      
        private async void sendMessageOrSound_Click(object sender, RoutedEventArgs e)
        {
            if (messageTxt.Text.Length != 0)
            {
                var textMessage = new TextMessage();
                textMessage.Text = messageTxt.Text;
                chatStackPanel.Children.Add(textMessage);
                //extra
                chatScroll.ScrollToEnd();
                messageTxt.Clear();
                messageTxt.Focus();
                //bot
                foreach (var id in idList)
                {
                    await bot.SendTextMessageAsync(id, textMessage.Text);
                }

            }
        }

        private void messageTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(messageTxt.Text.Length == 0)
            {
                messageOrSoundImg.Kind = PackIconKind.Microphone;
                //var color = (Color)ColorConverter.ConvertFromString("#6C7883");
                //sendMessageOrSound.Foreground = new SolidColorBrush(color);
            }
            else
            {
                messageOrSoundImg.Kind = PackIconKind.Send;
                //var color = (Color)ColorConverter.ConvertFromString("#5288C1");
                //sendMessageOrSound.Foreground = new SolidColorBrush(color);
            }
        }

        private void messageTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                sendMessageOrSound_Click(sender, null);
            }
            
        }

        private void openFileImage_Click(object sender, RoutedEventArgs e)
        {
            var od = new OpenFileDialog()
            {
                Filter=".jpg|*.jpg|.png|*.png|.bmp|*.bmp|.ico|*.ico|.gif|*.gif",
                Multiselect = true
            };
            if(od.ShowDialog() == true)
            {
                foreach(var filename in od.FileNames)
                {
                    var textMessage = new TextMessage();
                    textMessage.Text = messageTxt.Text;
                    textMessage.Url = filename;
                    if (messageTxt.Text == "") textMessage.Text = "Rasm";
                    chatStackPanel.Children.Add(textMessage);
                    chatScroll.ScrollToEnd();
                }               
                //extra
                messageTxt.Clear();
                messageTxt.Focus();
            }
        }
    }
}

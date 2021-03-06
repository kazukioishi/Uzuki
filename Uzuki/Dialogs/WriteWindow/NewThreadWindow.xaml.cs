﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Uzuki._2ch.Write;
using Uzuki.Dialogs.MainWindow;

namespace Uzuki.Dialogs.WriteWindow
{
    /// <summary>
    /// WriteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NewThreadWindow : MetroWindow
    {
        public CookieContainer cc;
        public String URL;

        public NewThreadWindow()
        {
            InitializeComponent();
            //ウィンドウ位置を使いやすい位置に
            this.Top = SingletonManager.MainWindowSingleton.Top;
            this.Left = SingletonManager.MainWindowSingleton.Left;
        }

        //書き込みボタン
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "書き込み中...";
            Thread thread = new Thread(WriteThread);
            thread.Start();
        }

        void WriteThread()
        {
            try
            {
                Writer writer = new Writer(URL);
                writer.CookieContainer = cc;
                //writer.CookieContainer.Add(new Cookie("READJS", "off") { Domain = new Uri(writer.PostURL).Host});
                //writer.CookieContainer.Add(new Cookie("MAIL", "") { Domain = new Uri(writer.PostURL).Host });
                //writer.CookieContainer.Add(new Cookie("NAME", "") { Domain = new Uri(writer.PostURL).Host });
                String Name = "", Mail = "", Message = "", Subject = "";
                Dispatcher.Invoke(new Action(() =>
                {
                    Name = NameTextBox.Text;
                    Mail = MailTextBox.Text;
                    Message = MessageTextBox.Text;
                    Subject = SubjectTextBox.Text;
                }));
                WriteResponse wr = writer.CreateThread(Name,Mail,Message,Subject);
                wr.GetResult();
                if (wr.Result == WriteResponse.WriteResult.True || wr.Result == WriteResponse.WriteResult.False)
                {
                    //成功時
                    Dispatcher.Invoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }
                else
                {
                    //たぶん何かのエラーだ
                    Dispatcher.Invoke(new Action(() =>
                    {
                        StatusLabel.Content = wr.Result;
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    StatusLabel.Content = ex.ToString();
                }));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            // Display OpenFileDialog by calling ShowDialog method 
            if (dlg.ShowDialog() == true)
            {
                StatusLabel.Content = "Uploading image...";
                new Thread(delegate()
                {
                    try
                    {
                        String url = ImageAPI.imgur.imgur.UploadImage(dlg.FileName);
                        Dispatcher.Invoke(new Action(() => {
                            MessageTextBox.Text += "\r\n" + url;
                            StatusLabel.Content = "画像を投稿しました";
                        }));
                    }
                    catch(Exception ex)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            StatusLabel.Content = ex.Message;
                        }));
                    }
                }).Start();
            }
        }
    }
}

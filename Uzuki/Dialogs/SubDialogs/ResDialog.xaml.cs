﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Uzuki._2ch.Objects;
using Uzuki.Dialogs.MainWindow;

namespace Uzuki.Dialogs.SubDialogs
{
    /// <summary>
    /// ResDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class ResDialog : MetroWindow
    {
        public ThreadMesg Resdata;
        public ResDialog()
        {
            InitializeComponent();
            this.DataContext = Resdata;
            //ウィンドウ位置を使いやすい位置に
            this.Top = SingletonManager.MainWindowSingleton.Top;
            this.Left = SingletonManager.MainWindowSingleton.Left;
        }
        public class ResClass : Uzuki._2ch.Objects.ThreadMesg
        {
            public int Count { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteWindow.WriteWindow writewindow = new WriteWindow.WriteWindow();
            Uri ur = new Uri(SingletonManager.MainWindowSingleton.SelectedThread.BoardURL);
            writewindow.MessageTextBox.Text = ">>" + Resdata.Count;
            writewindow.write2 = new _2ch.Write.Write2chThreadV2(ur.LocalPath, SingletonManager.MainWindowSingleton.SelectedThread.UnixTime.ToString(), ur.Host, SingletonManager.MainWindowSingleton.SetMannage.Cookie);
            writewindow.Show();
        }
    }
}

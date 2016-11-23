﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using MahApps.Metro.Controls;
using MahApps.Metro;



namespace Server
{
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            Globals.proc.EnableRaisingEvents = true;
            Globals.proc.Exited += Process_Exited;
        }

        //the OK button
        public async void retrieveInput_Click(object sender, RoutedEventArgs e)
        {
            int linenb;
            string confpath = Globals.path.FullName + "GA\\config\\server.P2M.conf";
            string targetpath = Globals.path.FullName + "GA\\config\\P2M.conf";

            if (Globals.proc.StartInfo.FileName == Globals.GApath && Globals.proc.HasExited == false)
                Globals.proc.Kill();
            if ((linenb = conf_verif(confpath)) == -1)
                return;
            if (!String.IsNullOrEmpty(this.inputText.Text))
                Writer("find-window-name = " + this.inputText.Text, targetpath, confpath, linenb);
            else
                Writer("#find-window-name = ", targetpath, confpath, linenb);
            Globals.proc.StartInfo.FileName = Globals.GApath;
            Globals.proc.StartInfo.Arguments = targetpath;
            Globals.proc.StartInfo.UseShellExecute = false;
            Globals.proc.StartInfo.CreateNoWindow = true;
            Globals.proc.Start();
            await Task.Delay(1000);
            if (Globals.proc.HasExited == true)
                MessageBox.Show("Window not found");
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new Action(() => ProgBar.Visibility = Visibility.Visible));
                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new Action(() => killInput.Visibility = Visibility.Visible));
            }
        }

        //kill child process, so the stream too
        public void killInput_Click(object sender, RoutedEventArgs e)
        {
            if (Globals.proc.StartInfo.FileName == Globals.GApath && Globals.proc.HasExited == false)
                Globals.proc.Kill();
        }

        //Day 'n' Night to choose between light or dark theme
        public void DnN_Click(object sender, RoutedEventArgs e)
        {
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetInverseAppTheme(theme.Item1));
        }

        //if the stream is off, the progress bar and the button stop disappear
        private void Process_Exited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new Action(() => ProgBar.Visibility = Visibility.Hidden));
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new Action(() => killInput.Visibility = Visibility.Hidden));
        }

        //write the new config file
        private static void Writer(string editline, string target, string source, int nbline)
        {
            string[] arrLine = File.ReadAllLines(source);
            arrLine[nbline - 1] = editline;
            File.WriteAllLines(target, arrLine);
        }

        //need a config file example
        private static int conf_verif(string path)
        {
            StreamReader txt;
            string line = "\0";
            int i = 0;

            if (File.Exists(path))
            {
                txt = new StreamReader(path);
                while (line != null && !line.Contains("find-window-name = "))
                {
                    ++i;
                    line = txt.ReadLine();
                    if (line != null && line.Contains("find-window-name = "))
                        return (i);
                }
                MessageBox.Show("Config file Error");
            }
            else
                MessageBox.Show("Could not find the config file");
            return (-1);
        }
    }

    public static class Globals
    {
        public static DirectoryInfo path = new DirectoryInfo("../../../../");
        public static Process proc = new Process();
        public static string GApath = path.FullName + "GA\\ga-server-periodic.exe";
    }
}

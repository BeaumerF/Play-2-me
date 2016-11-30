using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace Client
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
            var args = "config\\client.abs.conf rtsp://" + inputText.Text + ":8554/desktop";

            if ((Globals.proc.StartInfo.FileName == Globals.GApath) && (Globals.proc.HasExited == false))
                Globals.proc.Kill();
            var processInfo = new ProcessStartInfo(Globals.path.FullName + "\\GA\\" + Globals.GApath, args);
            processInfo.WorkingDirectory = Globals.path.FullName + "\\GA";
            processInfo.FileName = Globals.path.FullName + "\\GA\\" + Globals.GApath;
            processInfo.Arguments = args;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;
            Globals.proc.StartInfo = processInfo;
            Globals.proc.Start();
            await Task.Delay(1000);
            if (Globals.proc.HasExited)
                MessageBox.Show("Stream not found");
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => ProgBar.Visibility = Visibility.Visible));
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() => killInput.Visibility = Visibility.Visible));
            }
        }

        //kill child process, so the stream too
        public void killInput_Click(object sender, RoutedEventArgs e)
        {
            if ((Globals.proc.StartInfo.FileName == Globals.GApath) && (Globals.proc.HasExited == false))
                Globals.proc.Kill();
        }

        //Day 'n' Night to choose between light or dark theme
        public void DnN_Click(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetInverseAppTheme(theme.Item1));
        }

        //if the stream is off, the progress bar and the button stop disappear
        private void Process_Exited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => ProgBar.Visibility = Visibility.Hidden));
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => killInput.Visibility = Visibility.Hidden));
        }
    }

    public static class Globals
    {
        public static DirectoryInfo path = new DirectoryInfo("../../../../");
        public static Process proc = new Process();
        public static string GApath = "ga-client.exe";
    }
}
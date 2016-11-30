using System.Windows;

namespace Client
{
    public partial class App : Application
    {
        //need to kill the child if the parent is killed
        private void AppExit(object sender, ExitEventArgs e)
        {
            if ((Globals.proc.StartInfo.FileName == Globals.GApath)
                && (Globals.proc.HasExited == false))
                Globals.proc.Kill();
        }
    }
}
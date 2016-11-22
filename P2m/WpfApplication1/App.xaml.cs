using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    public partial class App : Application
    {
        //need to kill the child if the parent is killed
        private void AppExit(object sender, ExitEventArgs e)
        {
            if (Globals.proc.StartInfo.FileName == Globals.GApath
                && Globals.proc.HasExited == false)
                Globals.proc.Kill();
        }
    }    
}

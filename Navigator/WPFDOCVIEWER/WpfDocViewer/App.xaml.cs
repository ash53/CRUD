using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WpfDocViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Mark Lane set in the app xaml startup to catch any unhandled exceptions in the code during runtime.
        //As each is found add special exception handling as needed.
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
           
            Dispatcher.UnhandledException +=Dispatcher_UnhandledException;
           
        }


        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("WpfDocViewer An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

    }
}

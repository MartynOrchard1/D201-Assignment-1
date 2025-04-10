using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace movielibrary;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App() // LOOKS FOR THE ISSUE! -- Found the issue, issue was the path to the main window.xaml was incorrect.
    {
        // Handle unhandled exceptions in the application
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            MessageBox.Show(args.ExceptionObject.ToString(), "Unhandled Exception");
        };

        DispatcherUnhandledException += (sender, args) =>
        {
            MessageBox.Show(args.Exception.Message, "Dispatcher Unhandled Exception");
            args.Handled = true; // Prevents the app from crashing immediately
        };
    }
}


﻿using System;
using System.ServiceProcess;

namespace Rti.AdministrationService
{
    class LaunchService
	{  
		//
		// The main entry point for the application.
		//
		static void Main(string[] args)
		{
			// Run as console if there are command line arguments
			if (args.Length > 0)
			{               
			    foreach (var arg in args)
			    {
			        if (arg == "-x")
			        {
                        Console.WriteLine("[{0}]", Constants.RtiDllBuild);
			            Environment.Exit(0);
			        }
			    }

                // Visual Studio hosts the service for you, just wait
			    var service = new AdministrationService();
                service.Start();
				Console.WriteLine("Administration Service running. Press Enter to Exit...");
                Console.ReadLine();
                service.Stop();
			}
			else
			{
			    var servicesToRun = new ServiceBase[]
			    {
			        new AdministrationService(),
			    };

                ServiceBase.Run(servicesToRun);
			}
		}
	}
}
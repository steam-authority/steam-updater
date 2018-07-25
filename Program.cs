﻿using System;
using System.Threading;
using SteamUpdater.Consumers;

namespace SteamUpdater
{
    internal static class ChangeFetcher
    {
        private static void Main(string[] args)
        {
            Console.Title = "Steam Updater";

            // Rollbar
            Log.setupRollbar();

            // Wait for Rabbit
            while (true)
            {
                try
                {
                    var x = AbstractConsumer.getConnection();
                    var connection = x.Item1;
                    connection.Close();
                    connection.Dispose();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Waiting for Rabbit.. " + ex.Message);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            // Poll for new changes
            Steam.startSteam(false);

            // Consumers
            AbstractConsumer.startConsumers();

            // On quit
            Console.CancelKeyPress += delegate
            {
                Steam.quitOnDisconnect = true;
                Steam.steamUser.LogOff();
                Thread.Sleep(Timeout.Infinite);
            };

            // Block thread
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
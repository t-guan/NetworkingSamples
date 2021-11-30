using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace SyncSocketsConsole
{
    /// <summary>
    /// Represents the client side application.
    /// </summary>
    class Client
    {
        private Socket clientSocket;
        int addmode = 0;

        public void StartSendLoop()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                clientSocket.Connect(IPAddress.Loopback, Program.Port);

                while (true)
                {
                    Console.Write("Enter text to send: ");
                    string input = Console.ReadLine();

                    if (input == "exit")
                    {
                        clientSocket.Close();
                        Environment.Exit(0);
                    }


                    clientSocket.Send(Encoding.ASCII.GetBytes(input));
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}

using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Amazoom;

namespace MultiClient
{
    class Program
    {
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 100;
        public static int addmode = 0;
        public static int showmode = 0;

        static void Main()
        {
            Console.Title = "Client";
            ConnectToServer();
            RequestLoop();
            Exit();
        }

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException) 
                {
                    Console.Clear();
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }

        private static void RequestLoop()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

            while (true)
            {
                SendRequest();
                ReceiveResponse();
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        private static void SendRequest()
        {
            Console.Write("Send a request: ");
            string request = Console.ReadLine();
            string[] textsplit = Regex.Split(request, @",");
            SendString(request);

            if (request.ToLower() == "exit")
            {
                Exit();
            }
            else if (textsplit[0].ToLower() == "add")
            {
                addmode = 1;
            }
            else if (textsplit[0].ToLower() == "show")
            {
                showmode = 1;
            }
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            if (text == "StartNew")
            {
                Process.Start("Multi Client.exe");
                Console.WriteLine("New Client was Initialized");
            }
            else if (addmode == 1)
            {
                Inventory.Inv.Add(text);
                addmode = 0;
                int[,] dockloc = { { 0, 0, 1 } };
                Warehouse.initialize_warehouse(1, 5, 5, dockloc);
                Console.WriteLine("Item was added");
            }
            else if (showmode == 1)
            {
                try
                {
                    text = Inventory.Inv[Convert.ToInt32(text)];
                    Console.WriteLine(text);
                }
                catch (SystemException)
                {
                    Console.WriteLine("Invalid Index");
                }
                showmode = 0;
            }
            
        }
    }
}

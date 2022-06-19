using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ShgitObjects
{
    public static class Client
    {
        static byte[] ip = new byte[] { 127, 0, 0, 1 };
        static IPAddress ipAddr = new IPAddress(ip);
        static IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
        static Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public static string user = "";
        static string password = "";
        static byte[] buffer = new byte[1024];
        public static void ConnectToDatabase()
        {
            try
            {
                sender.Connect(localEndPoint);
                string log = "";
                Console.WriteLine("s for signup, l for login");
                while (log!="s" && log!="l")
                {
                    log = Console.ReadLine().ToLower();
                }

                string loginMessage = "";
                while(loginMessage!="logged in") //loop active until user logged in
                {
                    Console.WriteLine("user:");
                    user = Console.ReadLine();
                    Console.WriteLine("\npass:");
                    password = Console.ReadLine();
                    byte[] loginData = System.Text.Encoding.UTF8.GetBytes(user + " " + password + " " + log);
                    sender.Send(loginData);

                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
                    loginMessage = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    Console.WriteLine(loginMessage);
                }
            }

            // Manage of Socket's Exceptions
            catch (ArgumentNullException ane)
            {

                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            
        }

        public static void Quit()
        {
            sender.Send(Encoding.UTF8.GetBytes("quit"));
            sender.Close();
        }

        public static void UploadGraph (string absolutePath, string relativePath, string graph)
        {
            byte[] data = new byte[1024];
            byte[] command = Encoding.UTF8.GetBytes("upload " + graph + " " + user);
            byte[] storagePath = Encoding.UTF8.GetBytes(relativePath);
            byte[] fileSize = Encoding.UTF8.GetBytes(new FileInfo(absolutePath).Length.ToString());

            command.CopyTo(data, 0);
            sender.Send(data);
            Array.Clear(data, 0, data.Length);

            storagePath.CopyTo(data, 0);
            sender.Send(data);
            Array.Clear(data, 0, data.Length);

            fileSize.CopyTo(data, 0);
            sender.Send(data);
            Array.Clear(data, 0, data.Length);

            sender.SendFile(absolutePath);
        }

        public static string Info(string command, string location)
        {
            return "benis";
        }

        public static void DownloadGraph(string graph, string graphName)
        {
            sender.Send(Encoding.UTF8.GetBytes($"download {graphName} {user}"));
            string data = "";
            
            //while ()
            //{
            //    int dataLength = sender.Receive(buffer);
            //    data = Encoding.UTF8.GetString(buffer, 0, dataLength);
            //}
        }
    }
}

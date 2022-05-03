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
        public static void ConnectToDatabase()
        {
            try
            {

                sender.Connect(localEndPoint);
                string log = "";
                while (log!="s" && log!="l")
                {
                    log = Console.ReadLine().ToLower();
                }

                string loginMessage = "";
                while(loginMessage!="logged in")
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

        public static void Commit (string file, string nodePath)
        {
            sender.Send(System.Text.Encoding.UTF8.GetBytes("upload"));
            sender.Send(System.Text.Encoding.UTF8.GetBytes(file));
            sender.SendFile(nodePath);
        }
    }
}

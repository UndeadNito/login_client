using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;

namespace login_client
{
    class Network
    {
        static int port = 2020;
        static IPAddress address = Dns.GetHostEntry(IPAddress.Parse("127.0.0.1")).AddressList.Last();

        static TcpClient client = new TcpClient();
        static NetworkStream clientStream = null;
        

        public static bool Connect()
        {
            try
            {
                client.Connect(address, port);
                clientStream = client.GetStream();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error {Connection}]: " + e);
                return false;
            }
        }

        public static bool IsConnected()
        {
            return client.Connected;
        }

        public static bool Disconnect()
        {
            try
            {
                client.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error {Disconnection}]: " + e);
                return false;
            }
        }

        public static bool Send(byte code, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            data = data.Prepend(code).ToArray();

            try
            {
                clientStream.Write(data.ToArray(), 0, data.Length);
                Console.WriteLine("[Send]: " + message);
                return true;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("Server offline");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error {Sending}]: " + e);
                return false;
            }
        }

        public static string Receive(int length = 64)
        {
            byte[] data = new byte[length];

            try
            {
                clientStream.Read(data, 0, length);
                return Encoding.ASCII.GetString(data, 0, length);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error {Receiving}]: " + e);
                return "";
            }
        }


    }
}

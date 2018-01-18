using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client_Server_Practice_In_One_Class.TheCommunication
{
    public class Communication:ViewModelBase
    {
        Socket serverSocket, clientSocket;
        byte[] buffer = new byte[512];
        public BitmapImage Avatar { get; set; }

        List<ClientHandler> Client = new List<ClientHandler>();

        string endMessage = "quit";

        public Action<string> UpdateGui { get; set; }

        public Communication(int port, Action<string> updateGui, bool isServer, BitmapImage avatar)
        {
            this.UpdateGui = updateGui;

            if (isServer)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, port));
                serverSocket.Listen(5);
                Task.Factory.StartNew(Accept);
            }
            else
            {
                this.Avatar = avatar;
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Loopback, port));
                clientSocket = client.Client;
                Task.Factory.StartNew(Receive);
            }
        }

        private void Accept()
        {
            //clientSocket = serverSocket.Accept();
            //Task.Factory.StartNew(Receive);
            while (true)
            {
                Client.Add(new ClientHandler(serverSocket.Accept(), NewMessageReceived));
            }
        }

        private void NewMessageReceived(string message, Socket clientSocket)
        {
            foreach (var client in Client)
            {
                if (!client.ClienSocket.Equals(clientSocket))
                {
                    client.Send(message);
                }
            }

            UpdateGui(message);
        }

        private void Receive()
        {
            string message = "";
            while (!message.Equals(endMessage))
            {
                int length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                UpdateGui(message);
            }
        }

        public void Send(string message)
        {
            byte[] messageInBytes = Encoding.UTF8.GetBytes(Avatar + message);
            string messageAsString =  Encoding.UTF8.GetString(messageInBytes);
            clientSocket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}

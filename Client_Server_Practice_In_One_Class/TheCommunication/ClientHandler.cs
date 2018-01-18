using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server_Practice_In_One_Class.TheCommunication
{
    public class ClientHandler
    {

        public Socket ClienSocket { get; set; }
        byte[] buffer = new byte[512];
        public Action<string, Socket> UpdateGui { get; set; }

        public ClientHandler(Socket clientSocket, Action<string, Socket> updateGui)
        {
            this.UpdateGui = updateGui;
            this.ClienSocket = clientSocket;
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = "";

            while (true)
            {
                int length = ClienSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);
                UpdateGui(message, ClienSocket);
            }
        }

        public void Send(string message)
        {
            ClienSocket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
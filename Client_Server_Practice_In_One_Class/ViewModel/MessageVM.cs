using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client_Server_Practice_In_One_Class.ViewModel
{
    public class MessageVM
    {

        public BitmapImage  Avatar { get; set; }
        public string Message { get; set; }
        public MessageVM(string avatar, string message)
        {
            Avatar = new BitmapImage(new Uri(avatar, UriKind.Relative));
            Message = message;
        }
    }
}

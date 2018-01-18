using Client_Server_Practice_In_One_Class.TheCommunication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Client_Server_Practice_In_One_Class.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        public RelayCommand ServerBtn { get; set; }
        public RelayCommand ClientBtn { get; set; }
        public RelayCommand SendBtn { get; set; }
        public string Message { get; set; }
        public ObservableCollection<MessageVM> MessageList { get; set; }

        public ObservableCollection<BitmapImage> ImageList { get; set; }

        public BitmapImage SelectedImage { get; set; }
        public BitmapImage Avatar { get; set; }

        bool isServer = false;

        private const int port = 10100;

        Communication com;

        public MainViewModel()
        {
            ServerBtn = new RelayCommand(()=> 
            {
                isServer = true;
                com = new Communication(port, NewMessageReceived, isServer, null);
            });
            ClientBtn = new RelayCommand(() => 
            {
                isServer = false;
                com = new Communication(port, NewMessageReceived, isServer, SelectedImage);
            });
            SendBtn = new RelayCommand(() => 
            {
                com.Send(com.Avatar.UriSource.ToString() + ":" + Message);
            });

            MessageList = new ObservableCollection<MessageVM>();

            ImageList = new ObservableCollection<BitmapImage>();

            GenerateImageList();
        }

        private void NewMessageReceived(string message)
        {
            App.Current.Dispatcher.Invoke(() => 
            {
                string[] receivedMsg = message.Split(':');

                string uri = receivedMsg[0];
                string msg2 = receivedMsg[1];

                MessageList.Add(new MessageVM(uri, msg2));
            });
        }

        private void GenerateImageList()
        {
            ImageList.Add(new BitmapImage(new Uri("../Images/Avatar1.jpg", UriKind.Relative)));
            ImageList.Add(new BitmapImage(new Uri("../Images/Avatar2.png", UriKind.Relative)));
            ImageList.Add(new BitmapImage(new Uri("../Images/Avatar3.png", UriKind.Relative)));
            ImageList.Add(new BitmapImage(new Uri("../Images/Avatar4.png", UriKind.Relative)));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using UbuntuServerManager.Data;
using UbuntuServerManager.Models;

namespace UbuntuServerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext _context = new DataContext();
        private List<Server> _servers = new List<Server>();
        public ObservableCollection<Button> Buttons = new ObservableCollection<Button>();

        public MainWindow()
        {
            InitializeComponent();
            StackPanel.DataContext = Buttons;
            GetServers();
        }

        // Initialize Page
        private void GetServers()
        {
            Buttons = new ObservableCollection<Button>();
            _servers = _context.Servers.ToList();
            //_serverList.Children.Add()
            foreach (var serv in _servers)
            {
                var btn = GenerateServerButton(serv);
                Buttons.Add(btn);
                StackPanel.Children.Add(btn);
            }
        }

        private static Button GenerateServerButton(Server server)
        {
            Button btn = new Button()
            {
                Content = server.Id+" | "+server.NickName,
                Margin = new Thickness(5, 10, 5, 25),
                MinWidth = 30,
                CommandParameter = server.Id,
                
            };

            return btn;
        }
        // Page Interface
        private async Task NewServerAsync(string ipAddr, string networkName, string nickname, string pubKey)
        {
            Server serv = new Server()
            {
                IPAddress = ipAddr,
                NickName = nickname,
                NetworkName = networkName,
                Key = pubKey
            };
            await _context.Servers.AddAsync(serv);
            await _context.SaveChangesAsync();
        }


        // connect to server
        public void ConnectToNewServer()
        {

        }

        // Add Server Button Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddServer window = new AddServer();
            var result = window.ShowDialog();
            if (result == true)
            {
                GetServers();
            }
        }
    }
}

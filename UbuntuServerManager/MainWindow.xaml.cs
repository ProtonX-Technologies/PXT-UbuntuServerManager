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
using UbuntuServerManager.Handler;
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
        public AppManager AppManager { get; set; }

        public MainWindow()
        {
            AppManager=new AppManager();
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

        private Button GenerateServerButton(Server server)
        {
            Button btn = new Button()
            {
                Content = server.Id+" | "+server.NickName,
                Margin = new Thickness(5, 10, 5, 25),
                MinWidth = 30,
                CommandParameter = server.Id,
            };
            btn.Click += (sender, e) =>
            {
                ConnectToNewServer(server.Id);
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
        public void ConnectToNewServer(int id)
        {
            var serv = _servers.First(m => m.Id == id);
            
            // AppManager.Ssh = new SshHandler("", "", serv);
            AppManager.Ssh.InitSshConnection();

            SrvNameLbl.Content = serv.NetworkName;
            SrvIpLbl.Content = serv.IPAddress;
        }

        // Send Console Command
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string command = ConsoleEntry.Text;
            ConsoleBox.Text += $"|{AppManager.Ssh.Server.NickName}> {command} \n";

            var result = AppManager.Ssh.SendCommand(command);
            ConsoleBox.Text += result + "\n";

            ConsoleEntry.Text = "";
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

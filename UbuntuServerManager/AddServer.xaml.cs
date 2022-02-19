using System;
using System.Collections.Generic;
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
using UbuntuServerManager.Data;
using UbuntuServerManager.Models;

namespace UbuntuServerManager
{
    /// <summary>
    /// Interaction logic for AddServer.xaml
    /// </summary>
    public partial class AddServer : Window
    {

        private readonly DataContext _context;
        public AddServer()
        {
            _context = new DataContext();
            InitializeComponent();
        }


        private Server _newServer;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _newServer = new Server() {
                IPAddress = IpAddressEntry.Text,
                NickName = NicknameEntry.Text
            };
            if (PubKeyEntry.Text.Length > 0)
            {
                _newServer.Key = PubKeyEntry.Text;
            }

            _context.Servers.Add(_newServer);
            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}

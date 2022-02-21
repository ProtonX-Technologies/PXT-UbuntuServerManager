using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using UbuntuServerManager.Models;
using SshNet;
using UbuntuServerManager.Data;

namespace UbuntuServerManager.Handler
{
    public class SshHandler
    {
        public SshHandler(string username, string password, Server server)
        {
            Server = server;
            Username = username;
            Password = password;
        }

        public Server Server { get; set; }
        public bool ConnectionStatus { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public SshClient Client { get; set; }

    }

    public static class SshExtensions
    {
        public static void InitSshConnection(this SshHandler ssh)
        {
            ssh.Client = new SshClient(ssh.Server.IPAddress, ssh.Username, ssh.Password);
            ssh.Client.Connect();

            if (string.IsNullOrEmpty(ssh.Server.NetworkName))
            {
                ssh.Server.GetServerHostName(ssh);
            }
        }
        public static async Task InitSshConnectionAsync(this SshHandler ssh)
        {
            ssh.Client = new SshClient(ssh.Server.IPAddress, ssh.Username, ssh.Password);
            ssh.Client.Connect();

            if (string.IsNullOrEmpty(ssh.Server.NetworkName))
            {
                await ssh.Server.GetServerHostNameAsync(ssh);
            }
        }

        public static string SendCommand(this SshHandler ssh, string command)
        {
            var cmd = ssh.Client.RunCommand(command);
            
            return cmd.Result;
        }

        public static string GetServerHostName(this Server server, SshHandler ssh)
        {
            var context = new DataContext();
            var srv = context.Servers.FirstOrDefault(m => m.Id == server.Id);
            var result = ssh.SendCommand("hostnamectl");
            if (srv.NetworkName == null || srv.NetworkName != result)
            {
                srv.NetworkName = result;
                context.SaveChanges();
            }
            return result;
        }
        public static async Task<string> GetServerHostNameAsync(this Server server, SshHandler ssh)
        {
            var context = new DataContext();
            var srv = await context.Servers.FirstOrDefaultAsync(m => m.Id == server.Id);
            var result = ssh.SendCommand("hostnamectl");
            if (srv.NetworkName == null || srv.NetworkName != result)
            {
                srv.NetworkName = result;
                await context.SaveChangesAsync();
            }
            return result;
        }
    }
}

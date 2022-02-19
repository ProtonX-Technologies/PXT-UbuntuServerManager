using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbuntuServerManager.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string IPAddress { get; set; }
#nullable enable
        public string? NetworkName { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Key { get; set; }
#nullable disable
    }
}

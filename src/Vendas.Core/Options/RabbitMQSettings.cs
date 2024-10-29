using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Core.Options
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string CompraCriadaQueue { get; set; } = "CompraCriadaQueue";
        public string CompraAlteradaQueue { get; set; } = "CompraAlteradaQueue";
        public string CompraCanceladaQueue { get; set; } = "CompraCanceladaQueue";
    }

}

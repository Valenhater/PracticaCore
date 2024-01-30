using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaCore.Models
{
    public class ResumenClientePedido
    {
        public List<string> CodigoPedido { get; set; }
        public string CodigoCliente { get; set; }   
        public ResumenClientePedido()
        {
            this.CodigoPedido = new List<string>();
            this.CodigoCliente = string.Empty;
        }
    }
}

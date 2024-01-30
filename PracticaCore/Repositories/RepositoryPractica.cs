using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using PracticaCore.Models;

namespace PracticaCore.Repositories
{
   
    public class RepositoryPractica
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;
        public RepositoryPractica() 
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCORE;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }
        public List<string> GetEmpresas()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PRACTICA";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["Empresa"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }
        public ResumenClientePedido GetResumenClientePedido(string nombreEmpresa)
        {
            ResumenClientePedido resumen = new ResumenClientePedido();
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT CodigoCliente FROM CLIENTES WHERE Empresa = @NombreEmpresa";
            this.com.Parameters.Clear();
            this.com.Parameters.AddWithValue("@NombreEmpresa", nombreEmpresa);
            this.cn.Open();
            string codigoCliente = this.com.ExecuteScalar().ToString();
            this.cn.Close();
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT CodigoPedido FROM PEDIDOS WHERE CodigoCliente = @CodigoCliente";
            this.com.Parameters.Clear();
            this.com.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read())
            {
                resumen.CodigoPedido.Add(this.reader["CodigoPedido"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return resumen;
        }
        public Pedido GetDetallePedido(string codigoPedido)
        {
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = "SELECT * FROM PEDIDOS WHERE CodigoPedido = @CodigoPedido";
            this.com.Parameters.Clear();
            SqlParameter pamPedido = new SqlParameter("@CodigoPedido", codigoPedido);    
            this.com.Parameters.Add(pamPedido);
            this.cn.Open();
            SqlDataReader reader = this.com.ExecuteReader();
            Pedido detallePedido = null;
            if (reader.Read())
            {
                detallePedido = new Pedido
                {
                    CodigoPedido = reader["CodigoPedido"].ToString(),
                    FechaEntrega = (DateTime)reader["FechaEntrega"],
                    FormaEnvio = reader["FormaEnvio"].ToString(),
                    Importe = int.Parse(reader["Importe"].ToString())
                };
            }
            reader.Close();
            this.cn.Close();
            return detallePedido;
        }
        public int InsertarNuevoPedido(string codigoCliente, DateTime fechaEntrega, string formaEnvio, int importe)
        {
            string sql = "INSERT INTO PEDIDOS VALUES(@CodigoCliente, @FechaEntrega, @FormaEnvio, @Importe)";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.com.Parameters.Clear();
            SqlParameter pamCodCliente = new SqlParameter("@CodigoCliente", codigoCliente);
            this.com.Parameters.Add(pamCodCliente);
            SqlParameter pamFechaEntrega = new SqlParameter("@FechaEntrega", fechaEntrega);
            this.com.Parameters.Add(pamFechaEntrega);
            SqlParameter pamEnvio = new SqlParameter("@FormaEnvio", formaEnvio);
            this.com.Parameters.Add(pamEnvio);
            SqlParameter pamImporte = new SqlParameter("@Importe", importe);
            this.com.Parameters.Add(pamImporte);      
            this.cn.Open();
            int results = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return results;

        }
        public int BorrarPedido(string codigopedido)
        {           
            SqlParameter pamCodigo = new SqlParameter("@CodigoPedido", codigopedido);
            this.com.Parameters.Add(pamCodigo);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_ELIMINAR_PEDIDO";
            this.cn.Open();
            int deleted = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return deleted;
        }
    }
}

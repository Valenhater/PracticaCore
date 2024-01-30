using PracticaCore.Models;
using PracticaCore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaCore
{
    #region
    /*
         create procedure SP_PRACTICA 
    as 
	    select * from CLIENTES 
    GO 
    CREATE PROCEDURE SP_INSERTAR_PEDIDO
    @CodigoCliente NVARCHAR(50),
    @FechaEntrega DATETIME,
    @FormaEnvio NVARCHAR(50),
    @Importe INT
    AS
    INSERT INTO PEDIDOS (CodigoCliente, FechaEntrega, FormaEnvio, Importe)
    VALUES (@CodigoCliente, @FechaEntrega, @FormaEnvio, @Importe)
    GO

    CREATE PROCEDURE SP_ELIMINAR_PEDIDO
    @CodigoPedido NVARCHAR(50)
AS
    DELETE FROM PEDIDOS
    WHERE CodigoPedido = @CodigoPedido
GO
     */
    #endregion
    public partial class FormPractica : Form
    {
        RepositoryPractica repo;
        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryPractica();
            this.LoadClientes();
        }
        private void LoadClientes()
        {
            List<string> empresas = this.repo.GetEmpresas();
            foreach (string data in empresas)
            {
                this.cmbclientes.Items.Add(data);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmpresa = cmbclientes.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedEmpresa))
            {
                ResumenClientePedido resumen = repo.GetResumenClientePedido(selectedEmpresa);
                this.lstpedidos.Items.Clear();

                if (resumen.CodigoPedido != null && resumen.CodigoPedido.Count > 0)
                {
                    foreach (string cod in resumen.CodigoPedido)
                    {
                        this.lstpedidos.Items.Add(cod);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron pedidos para el cliente seleccionado.");
                }
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = lstpedidos.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < lstpedidos.Items.Count)
            {
                string selectedCodigoPedido = lstpedidos.Items[selectedIndex].ToString();
                Pedido detallePedido = repo.GetDetallePedido(selectedCodigoPedido);
                txtcodigopedido.Text = detallePedido.CodigoPedido;
                txtfechaentrega.Text = detallePedido.FechaEntrega.ToString();
                txtformaenvio.Text = detallePedido.FormaEnvio;
                txtimporte.Text = detallePedido.Importe.ToString();
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            string codigoCliente = txtcodigopedido.Text;
            DateTime fechaEntrega = DateTime.Parse(txtfechaentrega.Text);
            string formaEnvio = txtformaenvio.Text;
            int importe = int.Parse(txtimporte.Text);
            int insertados = this.repo.InsertarNuevoPedido(codigoCliente, fechaEntrega, formaEnvio, importe);
            MessageBox.Show("Pedidos insertados " + insertados);
        }
        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string prueba = this.lstpedidos.SelectedItem.ToString();        
            int results = this.repo.BorrarPedido(prueba);
            MessageBox.Show("Pedidos eliminados " + results);           
        }
    }
}


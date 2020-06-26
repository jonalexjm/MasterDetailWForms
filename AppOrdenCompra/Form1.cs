using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppOrdenCompra
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbFecha.Text = DateTime.Now.ToString();

          //  Refresh();

        }

        private void Refresh()
        {
            using (Models.dbOrdenCompraEntities1 db = new Models.dbOrdenCompraEntities1())
            {
                dgvListadoItems.DataSource = db.Odens.Select(d => new { d.id, d.fecha, d.total}).ToList();
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            //string establecimiento = txtEstablecimiento.Text.Trim();
            //string propietario = txtPropietario.Text.Trim();
            //string direccion = txtDireccion.Text.Trim();
            //string ciudad = txtCiudad.Text.Trim();
            //string telefono = txtTelefono.Text.Trim();
            //string nit = txtNit.Text.Trim();
            //string fecha =

            string descripcion = txtDescripcion.Text.Trim();
            string cantidad = txtCantidad.Text.Trim();
            string valor_unitario = txtValorUnitario.Text.Trim();
            string subtotal = (decimal.Parse(cantidad) * decimal.Parse(valor_unitario)).ToString();
;
            dgvListadoItems.Rows.Add(new object[] { descripcion, cantidad, valor_unitario, subtotal, "Eliminar" } );

            txtDescripcion.Text = "";
            txtCantidad.Text = "";
            txtValorUnitario.Text = "";

            txtDescripcion.Focus();

            CalcularTotal();



        }

        private void CalcularTotal()
        {
            decimal Total = 0;
            foreach(DataGridViewRow dr in dgvListadoItems.Rows)
            {
                decimal subtotal = decimal.Parse(dr.Cells[3].Value.ToString());

                Total += subtotal;
            }

            lbTotal.Text = Total.ToString();
        }

        private void dgvListadoItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || e.ColumnIndex != dgvListadoItems.Columns["Op"].Index)
            {
                return;
            }
            dgvListadoItems.Rows.RemoveAt(e.RowIndex);
            CalcularTotal();


        }

        private void btnGenerarOrden_Click(object sender, EventArgs e)
        {
            using (Models.dbOrdenCompraEntities1 db = new Models.dbOrdenCompraEntities1())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Models.Oden oOrden = new Models.Oden();

                        oOrden.establecimiento = txtEstablecimiento.Text.Trim();
                        oOrden.direccion = txtDireccion.Text.Trim();
                        oOrden.ciudad = txtCiudad.Text.Trim();
                        oOrden.propietario = txtPropietario.Text.Trim();
                        oOrden.telefono = txtTelefono.Text.Trim();
                        oOrden.nit = txtNit.Text.Trim();
                        oOrden.total = decimal.Parse(lbTotal.Text.ToString());
                        oOrden.fecha = DateTime.Now;

                        db.Odens.Add(oOrden);
                        db.SaveChanges();

                        foreach (DataGridViewRow dr in dgvListadoItems.Rows)
                        {
                            Models.Concepto oConcepto = new Models.Concepto();
                            oConcepto.descripcion = dr.Cells[0].Value.ToString();
                            oConcepto.cantidad = int.Parse(dr.Cells[1].Value.ToString());                            
                            oConcepto.valor_unitario = decimal.Parse(dr.Cells[2].Value.ToString());
                            oConcepto.id_orden = oOrden.id;
                            db.Conceptoes.Add(oConcepto);
                        }

                        db.SaveChanges();


                        dbContextTransaction.Commit();

                        MessageBox.Show("Se guardo exitosamente");

                        txtEstablecimiento.Text = "";
                        txtPropietario.Text = "";
                        txtDireccion.Text = "";
                        txtCiudad.Text = "";
                        txtTelefono.Text = "";
                        txtNit.Text = "";


                    }
                    catch (Exception ex)//si existe un error entonces hace rollback en la base de datos
                    {
                        dbContextTransaction.Rollback();
                    }


                }
                
            }
        }
    }
}

using ApiClientes;
using ApiClientes.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClienteMN01
{
    public partial class Form1 : Form
    {
        private IClienteHelper _Helper;
        DataRow _mCurrentClt;
        public Form1(IClienteHelper hp)
        {
            InitializeComponent();

            _Helper = hp;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dsClientes.Tables[0].Clear();

                string strUrl = _Helper.GetUrl("ClienteMN01", "GET");

                if (!string.IsNullOrEmpty(txtCorreo.Text)) strUrl += "/ " + txtCorreo.Text.Trim();

                var _Result = _Helper.Invoke<List<DTOCliente>>(strUrl, "GET");


                if (_Result.Success && _Result.DataFound)
                {
                    foreach (DTOCliente clt in _Result._Result)
                    {
                        DataRow newRow = dsClientes.Tables[0].NewRow();
                        newRow[0] = clt.Nombre;
                        newRow[1] = clt.Apellido;
                        newRow[2] = clt.Correo;

                        dsClientes.Tables[0].Rows.Add(newRow);
                    }
                }
                dgwClientes.Refresh();
                ClearTxt();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    txtNombre.Focus();
                    throw new ApplicationException("El nombre es requerido");
                }

                if (string.IsNullOrEmpty(txtApellido.Text))
                {
                    txtApellido.Focus();
                    throw new ApplicationException("El apellido es requerido");
                }

                if (string.IsNullOrEmpty(txtCorreo.Text))
                {
                    txtCorreo.Focus();
                    throw new ApplicationException("El crreo es requerido");
                }

                string strUrl = _Helper.GetUrl("ClienteMN01", "GET");
                strUrl += "/ " + txtCorreo.Text.Trim();

                var _Result = _Helper.Invoke<List<DTOCliente>>(strUrl, "GET");
                if (_Result.Success && _Result.DataFound) throw new ApplicationException("Correo Ya Exite");

                strUrl = _Helper.GetUrl("ClienteMN01", "POST");

                DTOCliente newClt = new DTOCliente()
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Correo = txtCorreo.Text.Trim()
                };

                var _Result2 = _Helper.Invoke<bool>(strUrl, "POST", newClt);
                DataRow newRow = dsClientes.Tables[0].NewRow();
                newRow[0] = newClt.Nombre;
                newRow[1] = newClt.Apellido;
                newRow[2] = newClt.Correo;

                dsClientes.Tables[0].Rows.Add(newRow);
                dgwClientes.Refresh();
                ClearTxt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mCurrentClt == null || _mCurrentClt.Table.Rows.Count <= 0)
                {
                    throw new ApplicationException("Debe de seleccionar un cliente");
                }

                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    txtNombre.Focus();
                    throw new ApplicationException("El nombre es requerido");
                }

                if (string.IsNullOrEmpty(txtApellido.Text))
                {
                    txtApellido.Focus();
                    throw new ApplicationException("El apellido es requerido");
                }

                if (string.IsNullOrEmpty(txtCorreo.Text))
                {
                    txtCorreo.Focus();
                    throw new ApplicationException("El crreo es requerido");
                }

                string strUrl;

                if (_mCurrentClt["Correo"].ToString().Trim().ToUpper() != txtCorreo.Text.Trim().ToUpper())
                {
                    strUrl = _Helper.GetUrl("ClienteMN01", "GET");
                    strUrl += "/ " + txtCorreo.Text.Trim();

                    var _Result = _Helper.Invoke<List<DTOCliente>>(strUrl, "GET");
                    if (_Result.Success && _Result.DataFound) throw new ApplicationException("Correo Ya Exite");

                }

                strUrl = _Helper.GetUrl("ClienteMN01", "PUT");

                DTOCliente newClt = new DTOCliente()
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    OldCorreo = _mCurrentClt["Correo"].ToString().Trim()
                };

                var _Result2 = _Helper.Invoke<bool>(strUrl, "PUT", newClt);

                DataRow Row = dsClientes.Tables[0].Select(" Correo = '" + _mCurrentClt["Correo"] + "'")[0];
                Row[0] = newClt.Nombre;
                Row[1] = newClt.Apellido;
                Row[2] = newClt.Correo;

                dgwClientes.Refresh();
                ClearTxt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mCurrentClt == null || _mCurrentClt.Table.Rows.Count <= 0)
                {
                    throw new ApplicationException("Debe de seleccionar un cliente");
                }

                string strUrl = _Helper.GetUrl("ClienteMN01", "DELETE");
                strUrl += "/ " + txtCorreo.Text.Trim();
                var _Result2 = _Helper.Invoke<bool>(strUrl, "DELETE");


                dsClientes.Tables[0].Rows.Remove(_mCurrentClt);
                dgwClientes.Refresh();
                ClearTxt();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearTxt() 
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
        }

        private void dgwClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string Correo = dgwClientes.CurrentRow.Cells[2].Value.ToString().Trim();
            if (!string.IsNullOrEmpty(Correo))
            {
                _mCurrentClt = dsClientes.Tables[0].Select("Correo = '" + Correo + "'")[0];

                if (_mCurrentClt.Table.Rows.Count > 0)
                {
                    txtNombre.Text = _mCurrentClt[0].ToString();
                    txtApellido.Text = _mCurrentClt[1].ToString();
                    txtCorreo.Text = _mCurrentClt[2].ToString();
                }
            }
        }
    }
}

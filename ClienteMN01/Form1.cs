using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace ClienteMN01
{
    public partial class Form1 : Form
    {

        string ConnetionString;
        DataTable Url;
        public Form1()
        {
            InitializeComponent();
            ConnetionString = ConfigurationManager.ConnectionStrings["String"].ConnectionString;

            using (var _Con = new SqlConnection(ConnetionString))
            {
                string query = @" Select * From tblApiConfig ";
                using (var cmd = new SqlCommand (query, _Con ))
                {
                    Url = new DataTable();
                    var da = new SqlDataAdapter(cmd);
                    _Con.Open();
                    da.Fill(Url);
                    
                }
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

                if (string.IsNullOrEmpty(txtApellido .Text))
                {
                    txtApellido .Focus();
                    throw new ApplicationException("El apellido es requerido");
                }

                if (string.IsNullOrEmpty(txtCorreo .Text))
                {
                    txtCorreo .Focus();
                    throw new ApplicationException("El crreo es requerido");
                }
                
                string strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'GET'")[0]["Url"].ToString();
                strUrl += "/ " + txtCorreo.Text.Trim();
                WebRequest request = WebRequest.Create(strUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                WebResponse response = null;
                JavaScriptSerializer js = new JavaScriptSerializer();
                
                response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var result = sr.ReadToEnd().Trim();
                    if(result != "false")
                    { 
                        ClienteModel oldclt = js.Deserialize<ClienteModel>(result);
                        if (oldclt != null || !string.IsNullOrEmpty(oldclt.Correo))
                        {
                            throw new ApplicationException("Correo ya existe");
                        }
                    }
                }
                strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'POST'")[0]["Url"].ToString();
                
                ClienteModel newClt = new ClienteModel() {  
                        Nombre = txtNombre .Text .Trim(), 
                        Apellido = txtApellido.Text .Trim (),
                        Correo = txtCorreo .Text.Trim ()
                    };

                    request =  WebRequest.Create (strUrl);
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    using (var sw = new StreamWriter(request .GetRequestStream ()))
                    {
                       
                        string Json = js.Serialize(newClt);

                        sw.Write(Json);
                        sw.Flush();
                        sw.Dispose();
                    }

                     response = request.GetResponse();

                DataRow newRow = dsClientes.Tables[0].NewRow();
                newRow[0] = newClt.Nombre;
                newRow[1] = newClt.Apellido;
                newRow[2] = newClt.Correo;

                dsClientes.Tables[0].Rows.Add(newRow);
                dgwClientes.Refresh();

                txtNombre.Clear();
                txtApellido.Clear();
                txtCorreo.Clear();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        DataRow _CurrentClt;
        private void dgwClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string Correo = dgwClientes.CurrentRow.Cells[2].Value.ToString().Trim ();
            if (!string.IsNullOrEmpty(Correo))
            { 
                _CurrentClt = dsClientes.Tables[0].Select( "Correo = '" + Correo + "'")[0];

            if (_CurrentClt.Table.Rows.Count > 0)
            {
                txtNombre.Text = _CurrentClt[0].ToString();
                txtApellido.Text = _CurrentClt[1].ToString();
                txtCorreo.Text = _CurrentClt[2].ToString();
            }
            }
           
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                dsClientes.Tables[0].Clear();

                string strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'GET'")[0]["Url"].ToString();
                if (! string.IsNullOrEmpty (txtCorreo.Text))
                    strUrl += "/ " + txtCorreo.Text.Trim();

                WebRequest request = WebRequest.Create(strUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                WebResponse response = null;
                JavaScriptSerializer js = new JavaScriptSerializer();

                response = request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var result = sr.ReadToEnd().Trim();
                    if (result != "false")
                    {
                        if (!string.IsNullOrEmpty(txtCorreo.Text))
                        {
                            ClienteModel oldclt = js.Deserialize<ClienteModel>(result);
                            
                            DataRow newRow = dsClientes.Tables[0].NewRow();
                            newRow[0] = oldclt.Nombre;
                            newRow[1] = oldclt.Apellido;
                            newRow[2] = oldclt.Correo;

                            dsClientes.Tables[0].Rows.Add(newRow);
                            dgwClientes.Refresh();
                        }
                        else
                        {
                            List<ClienteModel> lsClt = js.Deserialize < List<ClienteModel>>(result);
                            if (lsClt.Count > 0)
                            {
                            
                                foreach (ClienteModel clt in lsClt)
                                {
                                    DataRow newRow = dsClientes.Tables[0].NewRow();
                                    newRow[0] = clt.Nombre;
                                    newRow[1] = clt.Apellido;
                                    newRow[2] = clt.Correo;

                                    dsClientes.Tables[0].Rows.Add(newRow);
                                }
                                dgwClientes.Refresh();

                                txtNombre.Clear();
                                txtApellido.Clear();
                                txtCorreo.Clear();
                            }
                        }

                       
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString ());
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_CurrentClt == null || _CurrentClt.Table.Rows .Count <=0 )
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
                WebRequest request;
                JavaScriptSerializer js = new JavaScriptSerializer();
                WebResponse response = null;

                if (_CurrentClt["Correo"].ToString().Trim().ToUpper() != txtCorreo.Text.Trim().ToUpper())
                {
                    strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'GET'")[0]["Url"].ToString();
                    strUrl += "/ " + txtCorreo.Text.Trim();
                    request = WebRequest.Create(strUrl);
                    request.Method = "GET";
                    request.ContentType = "application/json";
                   
                   

                    response = request.GetResponse();
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        var result = sr.ReadToEnd().Trim();
                        if (result != "false")
                        {
                            ClienteModel oldclt = js.Deserialize<ClienteModel>(result);
                            if (oldclt != null || !string.IsNullOrEmpty(oldclt.Correo))
                            {
                                throw new ApplicationException("Correo ya existe");
                            }
                        }
                    }

                }


                strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'PUT'")[0]["Url"].ToString();

                ClienteModel newClt = new ClienteModel()
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Correo = txtCorreo.Text.Trim(),
                    OldCorreo = _CurrentClt["Correo"].ToString().Trim()
                };

                request = WebRequest.Create(strUrl);
                request.Method = "PUT";
                request.ContentType = "application/json";

                using (var sw = new StreamWriter(request.GetRequestStream()))
                {

                    string Json = js.Serialize(newClt);

                    sw.Write(Json);
                    sw.Flush();
                    sw.Dispose();
                }

                response = request.GetResponse();

                DataRow Row = dsClientes.Tables[0].Select(" Correo = '" + _CurrentClt["Correo"] + "'")[0];
                Row[0] = newClt.Nombre;
                Row[1] = newClt.Apellido;
                Row[2] = newClt.Correo;

               
                dgwClientes.Refresh();

                txtNombre.Clear();
                txtApellido.Clear();
                txtCorreo.Clear();


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
                if (_CurrentClt == null || _CurrentClt.Table .Rows .Count <=0)
                {
                    throw new ApplicationException("Debe de seleccionar un cliente");
                }

                string strUrl = Url.Select(" Mantenimiento = 'ClienteMN01' And Metodo = 'DELETE'")[0]["Url"].ToString();
                strUrl += "/ " + txtCorreo.Text.Trim();
                WebRequest request = WebRequest.Create(strUrl);
                request.Method = "DELETE";
                request.ContentType = "application/json";
                WebResponse response = null;
                JavaScriptSerializer js = new JavaScriptSerializer();

                response = request.GetResponse();

               dsClientes.Tables[0].Rows.Remove (_CurrentClt);
                dgwClientes.Refresh();

                txtNombre.Clear();
                txtApellido.Clear();
                txtCorreo.Clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}

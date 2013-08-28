using System;
using System.Xml;
using System.Web;
using System.Net;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class bannersestads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e){
		System.Text.StringBuilder resultado = new System.Text.StringBuilder();
		string estado="0";
		string error="";
		string datos="";
		
		if(Request["id_bloguer"]!=null && Request["id_bloguer"].ToString()!="" && Request["id_componente"]!=null && Request["id_componente"].ToString()!=""){
			SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
			SqlCommand cmd = new SqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conn;
			
			cmd.CommandText = "sp_Banners";
			cmd.Parameters.AddWithValue("@id_bloguer", Request["id_bloguer"].ToString());
			cmd.Parameters.AddWithValue("@id_componente", Request["id_componente"].ToString());	
	
			conn.Open();
			try{
				cmd.ExecuteScalar();
				string res = cmd.Parameters[0].Value.ToString();
				
				if(res==Request["id_bloguer"].ToString()){
					estado="1";
					datos="OK";
				}
				else{
					estado="0";
					error="Error 3: Problemas en BBDD!";
				}
			}
			catch{
				estado="0";
				error="Error 2: Problemas en BBDD!";
			}
			finally{
				conn.Close();
			}			
		}
		else
			error="Error 1: Faltan parámetros!";
		
		
		resultado.Append("<salida>");
			resultado.Append("<estado>");
			resultado.Append(estado);
			resultado.Append("</estado>");
			resultado.Append("<error>");
			resultado.Append(error);	
			resultado.Append("</error>");
			resultado.Append("<datos>");
			resultado.Append(datos);
			resultado.Append("</datos>");
		resultado.Append("</salida>");
		
		Response.Write(resultado);
    }
}
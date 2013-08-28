>%@ Page Language="C#" ContentType="text/html" ResponseEncoding="utf-8" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<html>

<script language="C#" runat="server">

    protected void Page_Load(Object sender, EventArgs e) 
    {
        SqlConnection myConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        SqlDataAdapter myCommand = new SqlDataAdapter("select * from banners_EIFIV", myConnection);

        DataSet ds = new DataSet();
        myCommand.Fill(ds);

        MyDataGrid.DataSource=ds.Tables[0].DefaultView;
        MyDataGrid.DataBind();
    }

</script>

<body bgcolor="#CCCCCC">

  <h3><font face="Verdana">Visor simple de Bannersestads</font></h3>

  <ASP:DataGrid id="MyDataGrid" runat="server"
    Width="1200"
    BackColor="#f5f5f5" 
    BorderColor="black"
    ShowFooter="false" 
    CellPadding=3 
    CellSpacing="0"
    Font-Name="Verdana"
    Font-Size="8pt"
    HeaderStyle-BackColor="#CCCCCC"
    EnableViewState="true"
  />

</body>
</html>

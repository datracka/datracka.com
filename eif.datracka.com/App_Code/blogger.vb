Imports Microsoft.VisualBasic
Imports System.Data


Public Class blogger
    'propios
    Dim _pk_cod_blogger, _fk_cod_tipo_blogger, _fk_newUserId As Integer
    Dim _dateofbirth, _fecha_alta, _fecha_mod As Date
    Dim _email, _password, _blog, _url_blog, _aviso, _firstname, _lastname, _middlename, _city, _estado As String

    ' datos
    Dim da As New datos


    Public Property pk_cod_blogger() As Integer
        Get
            Return _pk_cod_blogger
        End Get
        Set(ByVal value As Integer)
            _pk_cod_blogger = value
        End Set
    End Property
    Public Property fk_cod_tipo_blogger() As Integer
        Get
            Return _fk_cod_tipo_blogger
        End Get
        Set(ByVal value As Integer)
            _fk_cod_tipo_blogger = value
        End Set
    End Property
    Public Property fk_newuserId() As Integer
        Get
            Return _fk_newuserId
        End Get
        Set(ByVal value As Integer)
            _fk_newuserId = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property
    Public Property Aviso() As String
        Get
            Return _aviso
        End Get
        Set(ByVal value As String)
            _aviso = value
        End Set
    End Property
    Public Property Blog() As String
        Get
            Return _blog
        End Get
        Set(ByVal value As String)
            _blog = value
        End Set
    End Property
    Public Property Url_blog() As String
        Get
            Return _url_blog
        End Get
        Set(ByVal value As String)
            _url_blog = value
        End Set
    End Property

    Public Property Firstname() As String
        Get
            Return _firstname
        End Get
        Set(ByVal value As String)
            _firstname = value
        End Set
    End Property

    Public Property Lastname() As String
        Get
            Return _lastname
        End Get
        Set(ByVal value As String)
            _lastname = value
        End Set
    End Property
    Public Property Middlename() As String
        Get
            Return _middlename
        End Get
        Set(ByVal value As String)
            _middlename = value
        End Set
    End Property
    Public Property DateOfBirth() As Date
        Get
            Return _dateofbirth
        End Get
        Set(ByVal value As Date)
            _dateofbirth = value
        End Set
    End Property
    Public Property City() As String
        Get
            Return _city
        End Get
        Set(ByVal value As String)
            _city = value
        End Set
    End Property
    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property Fecha_Alta() As Date
        Get
            Return _fecha_alta
        End Get
        Set(ByVal value As Date)
            _fecha_alta = value
        End Set
    End Property
    Public Property Fecha_Mod() As Date
        Get
            Return _fecha_mod
        End Get
        Set(ByVal value As Date)
            _fecha_mod = value
        End Set
    End Property

#Region "constructores"

    Public Sub New()
        '
    End Sub

    Public Sub New(ByVal id As Integer)
        Carga(id)
    End Sub

#End Region



#Region "metodos"
    Public Sub Carga(ByVal id As Integer)
        Dim dt1 As New Data.DataTable
        dt1 = da.GetBloggerPorId(id)
        'datos propios
        With dt1.Rows(0)
            Me.pk_cod_blogger = id
            Me.fk_cod_tipo_blogger = CType(.Item("fk_cod_tipoblogger"), Integer)
            Me.fk_newuserId = CType(.Item("fk_newuserId"), Integer)

            Me.Email = .Item("gls_email")
            Me.Password = .Item("gls_password")
            Me.Aviso = .Item("gls_aviso")
            Me.Blog = .Item("gls_blog")
            Me.Url_blog = .Item("gls_url_blog")
            Me.Firstname = .Item("gls_firstname")

            Me.Lastname = .Item("gls_lastname")
            Me.Middlename = .Item("gls_middlename")
            Me.DateOfBirth = CType(.Item("gls_DateOfBirth"), Date)
            Me.City = .Item("gls_city")
            Me.Estado = .Item("cod_estado")

            Me.Fecha_Alta = CType(.Item("ctr_fecha_alta"), Date)
            Me.Fecha_Mod = CType(.Item("ctr_fecha_mod"), Date)
        End With
    End Sub

    Public Function Save() As Boolean
        Return da.InsUpdBlogger(Me, "INSERT")
    End Function

    Public Function Update() As Boolean
        Return da.InsUpdBlogger(Me, "UPDATE")
    End Function
#End Region

End Class

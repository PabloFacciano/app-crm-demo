Public Class frmBuscarVenta

    Public Property FechaInicial As Date
        Get
            Return dateFechaInicio.Value
        End Get
        Set(value As Date)
            dateFechaInicio.Value = value
        End Set
    End Property
    Public Property FechaFin As Date
        Get
            Return dateFechaFin.Value
        End Get
        Set(value As Date)
            dateFechaFin.Value = value
        End Set
    End Property
    Public Property PrecioBase As Single
        Get
            txtPrecioTotal.Text = Trim(txtPrecioTotal.Text)
            If txtPrecioTotal.Text = String.Empty Then Return 0
            If Not IsNumeric(txtPrecioTotal.Text) Then Return 0
            Return txtPrecioTotal.Text
        End Get
        Set(value As Single)
            txtPrecioTotal.Text = value
        End Set
    End Property
    Public Property MenorPrecio As Boolean
        Get
            Return rbMenorPrecio.Checked
        End Get
        Set(value As Boolean)
            rbMayorPrecio.Checked = Not value
            rbMenorPrecio.Checked = value
        End Set
    End Property
    Private _idcliente As Integer
    Public Property IdCliente As Integer
        Get
            Return _idcliente
        End Get
        Set(value As Integer)
            _idcliente = value
        End Set
    End Property
    Private _idproducto As Integer
    Public Property IdProducto As Integer
        Get
            Return _idproducto
        End Get
        Set(value As Integer)
            _idproducto = value
        End Set
    End Property


    Private Sub DateFechaInicio_ValueChanged(sender As Object, e As EventArgs) Handles dateFechaInicio.ValueChanged
        If FechaInicial > FechaFin Then FechaInicial = FechaFin : Beep()
    End Sub
    Private Sub DateFechaFin_ValueChanged(sender As Object, e As EventArgs) Handles dateFechaFin.ValueChanged
        If FechaFin < FechaInicial Then FechaFin = FechaInicial : Beep()
    End Sub
    Private Sub TxtPrecio_TextChanged(sender As Object, e As EventArgs) Handles txtPrecioTotal.TextChanged
        If ((Trim(txtPrecioTotal.Text) = String.Empty) Or (IsNumeric(txtPrecioTotal.Text))) Then
            labelErrorPrecio.Text = String.Empty
        Else
            labelErrorPrecio.Text = "Sólo se aceptan números, incluido decimales."
        End If
    End Sub
    Private Sub BtnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Dim C As Cliente = General.BuscarCliente()
        IdCliente = C.ID
        txtCliente.Text = C.Cliente
        C = Nothing
    End Sub
    Private Sub BtnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        Dim P As Producto = General.BuscarProducto()
        IdProducto = P.ID
        txtProducto.Text = P.Nombre
        P = Nothing
    End Sub


    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
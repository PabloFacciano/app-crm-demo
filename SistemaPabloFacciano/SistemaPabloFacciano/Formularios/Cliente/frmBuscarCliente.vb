Public Class frmBuscarCliente

    Private _cliente As Cliente
    Public Property Cliente As Cliente
        Get
            Return _cliente
        End Get
        Set(value As Cliente)
            _cliente = value
            If value IsNot Nothing Then
                txtCliente.Text = value.Cliente
                txtCorreo.Text = value.Correo
                txtTelefono.Text = value.Telefono
            End If
        End Set
    End Property

    Private Sub ObtenerClienteIngresado()
        Cliente.Cliente = txtCliente.Text
        Cliente.Correo = txtCorreo.Text
        If TelefonoEsValido(txtTelefono.Text) Then
            Cliente.Telefono = txtTelefono.Text
        Else
            Cliente.Telefono = Nothing
        End If
    End Sub
    Private Function ElClienteIngresadoEsValido() As Boolean
        Dim resultado As Boolean = False

        If TelefonoEsValido(Cliente.Telefono) Or NombreClienteEsValido(Cliente.Cliente) Or (Cliente.Correo <> String.Empty) Then
            resultado = True
        End If

        Return resultado
    End Function
    Private Sub MostrarErroresDeEntrada()
        ' Cliente
        If Trim(Cliente.Cliente) = String.Empty Then
            labelErrorCliente.Text = "Este campo es obligatorio."
        Else
            labelErrorCliente.Text = String.Empty
        End If
        ' Telefono
        If Not TelefonoEsValido(Cliente.Telefono) Then
            labelErrorTelefono.Text = "El Teléfono ingresado no es válido."
        Else
            labelErrorTelefono.Text = String.Empty
        End If
    End Sub


    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ObtenerClienteIngresado()
        If ElClienteIngresadoEsValido() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MostrarErroresDeEntrada()
        End If
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Cliente = Nothing
        Me.Close()
    End Sub


End Class
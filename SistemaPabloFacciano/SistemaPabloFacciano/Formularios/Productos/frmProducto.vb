Public Class frmProducto

    Private _producto As Producto
    Public Property Producto As Producto
        Get
            Return _producto
        End Get
        Set(value As Producto)
            _producto = value
            If value IsNot Nothing Then
                txtNombre.Text = value.Nombre
                txtPrecio.Text = value.Precio
                comboCategoria.Text = value.Categoria
            End If
        End Set
    End Property

    Private Sub ObtenerproductoIngresado()
        Producto.Categoria = Trim(comboCategoria.Text)
        Producto.Nombre = Trim(txtNombre.Text)
        If IsNumeric(txtPrecio.Text) Then
            Producto.Precio = txtPrecio.Text
        End If
    End Sub
    Private Function ElproductoIngresadoEsValido() As Boolean
        Dim resultado As Boolean = False

        If (Producto.Nombre <> String.Empty) Then
            If (Producto.Categoria <> String.Empty) Then
                If (Producto.Precio <> 0) Then
                    resultado = True
                End If
            End If
        End If

        Return resultado
    End Function
    Private Sub MostrarErroresDeEntrada()
        ' Nombre
        If Producto.Nombre = String.Empty Then
            labelErrorNombre.Text = "Este campo es obligatorio."
        Else
            labelErrorNombre.Text = String.Empty
        End If
        ' Categoria
        If Producto.Categoria = String.Empty Then
            labelErrorCategoria.Text = "Este campo es obligatorio."
        Else
            labelErrorCategoria.Text = String.Empty
        End If
        ' Precio
        If Producto.Precio = 0 Then
            labelErrorPrecio.Text = "Este campo es obligatorio."
        Else
            labelErrorPrecio.Text = String.Empty
        End If
    End Sub

    Private Sub CargarCategoriasGuardadas()
        Try
            Dim ds As DataSet = Producto.SelectTodasLasCategorias()
            If ds Is Nothing Then Exit Sub

            For Each row As DataRow In ds.Tables(0).Rows
                comboCategoria.Items.Add(row.Item(0))
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ObtenerproductoIngresado()
        If ElproductoIngresadoEsValido() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MostrarErroresDeEntrada()
        End If
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Producto = Nothing
        Me.Close()
    End Sub

    Private Sub frmBuscarProducto_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        CargarCategoriasGuardadas()
    End Sub

    Private Sub frmProducto_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.DialogResult <> DialogResult.OK Then
            Me.DialogResult = DialogResult.Cancel
            Me.Producto = Nothing
        End If
    End Sub

End Class
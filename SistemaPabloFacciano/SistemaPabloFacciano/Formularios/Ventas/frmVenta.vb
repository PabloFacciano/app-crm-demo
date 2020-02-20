Public Class frmVenta

    Private _venta As Venta
    Public Property Venta As Venta
        Get
            Return _venta
        End Get
        Set(value As Venta)
            _venta = value
        End Set
    End Property

    Private _cliente As Cliente
    Public Property Cliente As Cliente
        Get
            Return _cliente
        End Get
        Set(value As Cliente)
            _cliente = value
            If value IsNot Nothing Then
                txtCliente.Text = _cliente.Cliente
                Venta.IDCliente = _cliente.ID
            Else
                txtCliente.Text = String.Empty
                Venta.IDCliente = 0
            End If
        End Set
    End Property
    Private _ventaitems As New List(Of VentaItem)
    Public Property VentaItems As List(Of VentaItem)
        Get
            Return _ventaitems
        End Get
        Set(value As List(Of VentaItem))
            _ventaitems = value
        End Set
    End Property


    Private Function IsProductSelected() As Boolean
        Return (grid.SelectedRows.Count > 0)
    End Function

    Private Sub ObtenerVentaIngresada()
        Venta.Fecha = dateFecha.Value
        If Me.Cliente IsNot Nothing Then Venta.IDCliente = Me.Cliente.ID
    End Sub
    Private Function EsVentaValida() As Boolean
        If Venta.IDCliente <> 0 Then
            If VentaItems.Count > 0 Then
                Return True
            End If
        End If
        Return False
    End Function
    Private Sub MostrarErroresEnEntradas()
        If Venta.IDCliente = 0 Then MsgBox("No se ha seleccionado un cliente.", MsgBoxStyle.Critical, "Error") : Exit Sub
        If VentaItems.Count = 0 Then MsgBox("No se ha seleccionado ningún producto.", MsgBoxStyle.Critical, "Error") : Exit Sub
    End Sub

    Public Sub LoadVenta(EsNuevaVenta As Boolean)
        Try
            dateFecha.Value = Venta.Fecha
            If EsNuevaVenta Then
                Me.Cliente = Nothing
            Else
                Me.Cliente = Cliente.SelectByID(Venta.IDCliente)
                LoadVentaItems()
                MostrarVentaItems()
                ActualizarMontoFinal()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
            BtnCancelar_Click()
        End Try
    End Sub
    Private Sub LoadVentaItems()
        If Venta.ID = 0 Then Exit Sub
        If VentaItems.Count > 0 Then VentaItems.Clear()

        Dim data As DataSet = VentaItem.SelectByIdVenta(Venta.ID)

        For Each row As DataRow In data.Tables(0).Rows
            Dim v As New VentaItem With {
                .ID = row.Item("ID"),
                .IDProducto = row.Item("IDProducto"),
                .IDVenta = Venta.ID,
                .Cantidad = row.Item("Cantidad"),
                .PrecioUnitario = row.Item("PrecioUnitario"),
                .PrecioTotal = row.Item("PrecioTotal")
            }
            VentaItems.Add(v)
        Next

        data = Nothing
    End Sub
    Private Sub MostrarVentaItems()

        If grid.RowCount > 0 Then grid.Rows.Clear()

        If VentaItems.Count > 0 Then
            For Each item As VentaItem In VentaItems

                grid.Rows.Add(item.ID, item.IDProducto, Producto.SelectNameByID(item.IDProducto), item.Cantidad, item.PrecioUnitario, item.PrecioTotal)

            Next
        End If

    End Sub
    Private Sub ActualizarMontoFinal()
        Dim total As Single = 0
        If VentaItems.Count > 0 Then
            For Each v As VentaItem In VentaItems
                total += v.PrecioTotal
            Next
            Venta.Total = total
        Else
            total = Venta.Total
        End If
        txtPrecioTotal.Text = "$ " & total
    End Sub




    Private Sub BtnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Me.Cliente = General.BuscarCliente()
    End Sub
    Private Sub BtnCrearCliente_Click(sender As Object, e As EventArgs) Handles btnCrearCliente.Click
        Me.Cliente = General.CrearCliente()
    End Sub

    Private Sub BtnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        Dim producto As Producto = General.BuscarProducto()

        If producto Is Nothing Then Exit Sub
        For Each item As VentaItem In VentaItems
            If item.IDProducto = producto.ID Then Exit Sub
        Next

        Dim nuevo_ventaitem As New VentaItem

        nuevo_ventaitem.ID = 0
        nuevo_ventaitem.IDProducto = producto.ID
        nuevo_ventaitem.PrecioUnitario = producto.Precio
        nuevo_ventaitem.Cantidad = 1
        nuevo_ventaitem.PrecioTotal = producto.Precio

        ' nuevo_ventaitem.IDVenta: Se agrega despues de insertar/actualizar la venta.

        VentaItems.Add(nuevo_ventaitem)

        MostrarVentaItems()
        ActualizarMontoFinal()
    End Sub
    Private Sub BtnEditarCantidadProducto_Click(sender As Object, e As EventArgs) Handles btnEditarCantidadProducto.Click
        If Not IsProductSelected() Then
            Beep()
            Exit Sub
        End If

        Dim id_producto As Integer = grid.SelectedRows(0).Cells("IDProducto").Value
        For Each v As VentaItem In VentaItems
            If v.IDProducto = id_producto Then

                Dim nueva_cantidad As String = Trim(InputBox("Ingrese la cantidad:", "Cantidad", v.Cantidad))

                If (nueva_cantidad = String.Empty) Then Exit Sub
                If (Not IsNumeric(nueva_cantidad)) Then Exit Sub
                If (Int(nueva_cantidad) <= 0) Then Exit Sub
                If (v.Cantidad = Int(nueva_cantidad)) Then Exit Sub

                v.Cantidad = Int(nueva_cantidad)
                v.PrecioTotal = v.Cantidad * v.PrecioUnitario

                If v.ID <> 0 Then
                    VentaItem.Update(v)
                End If

                MostrarVentaItems()
                ActualizarMontoFinal()

                Exit Sub
            End If
        Next

    End Sub
    Private Sub BtnEliminarProducto_Click(sender As Object, e As EventArgs) Handles btnEliminarProducto.Click
        If (Not IsProductSelected()) Then
            Beep()
            Exit Sub
        End If

        Dim id_ventaitem As Integer = grid.SelectedRows(0).Cells("Key").Value

        If id_ventaitem <> 0 Then
            If MsgBox("Está seguro de eliminar este producto de la venta?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Eliminar Producto") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If


        Dim id_producto As Integer = grid.SelectedRows(0).Cells("IDProducto").Value
        Try

            ' Eliminar de Base de Datos
            VentaItem.DeleteById(id_ventaitem)

            ' Eliminar de Lista/Memoria
            For index As Integer = 0 To VentaItems.Count - 1
                If VentaItems(index).IDProducto = id_producto Then VentaItems.RemoveAt(index) : Exit For
            Next

            MostrarVentaItems()
            ActualizarMontoFinal()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Sub BtnGuardar_Click() Handles btnGuardar.Click
        ObtenerVentaIngresada()

        If EsVentaValida() Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MostrarErroresEnEntradas()
        End If
    End Sub
    Private Sub BtnCancelar_Click() Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Venta = Nothing
        'Me.Cliente = Nothing
        'Me.VentaItems = Nothing
        Me.Close()
    End Sub

    Private Sub frmVenta_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.DialogResult <> DialogResult.OK Then
            Me.DialogResult = DialogResult.Cancel
            Me.Venta = Nothing
        End If
    End Sub
End Class
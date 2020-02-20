Public Class frmAdministracionProductos

    Private Function CantidadRegistrosCargados() As Integer
        Return grid.Rows.Count
    End Function
    Private Function HayRegistrosCargados() As Boolean
        Return (CantidadRegistrosCargados() > 0)
    End Function
    Private Function HayRegistroSeleccionado() As Boolean
        Return (grid.SelectedRows.Count > 0)
    End Function

    Private Sub HabilitarPanel(value)
        ' panelControl.Enabled  = value
        panelControl.Visible = value
    End Sub

    Public ReadOnly Property ProductoSeleccionado As Producto
        Get
            If Not HayRegistroSeleccionado() Then Return Nothing
            Dim row As DataGridViewRow = grid.SelectedRows(0)
            Dim c As New Producto With {
                .ID = row.Cells("ID").Value,
                .Nombre = row.Cells("Nombre").Value,
                .Precio = row.Cells("Precio").Value,
                .Categoria = row.Cells("Categoria").Value
            }
            row = Nothing
            Return c
        End Get
    End Property

    Private Sub CambiarEstado(nuevo_estado As String)
        estado.Text = nuevo_estado
    End Sub
    Sub CargarTodosLosRegistros()
        CargarRegistros(Producto.SelectAll())
    End Sub
    Sub CargarRegistros(ds As DataSet)
        CambiarEstado("Cargando registros.")
        HabilitarPanel(False)

        Try
            If ds IsNot Nothing Then grid.DataSource = ds.Tables(0)
            ' grid.Refresh() ' No es necesario
            MostrarResultados()
        Catch ex As Exception
            CambiarEstado("Error al cargar registros.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        HabilitarPanel(True)
    End Sub
    Sub MostrarResultados()
        Dim hayRegs As Boolean = HayRegistrosCargados()
        panelGrid.Visible = hayRegs
        panelSinRegistros.Visible = Not hayRegs

        If hayRegs Then
            Dim cantidad As Integer = CantidadRegistrosCargados()
            CambiarEstado(cantidad & " registro(s).")

            grid.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            grid.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            grid.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            grid.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        Else
            CambiarEstado("No hay registros que mostrar.")
        End If
    End Sub


    Public Property ProductoBase As Producto
    Public Property BuscarProductoMenorPrecio As Boolean
    Private Property ModoBusqueda As Boolean = False
    Sub ActivarModoBusqueda()
        ModoBusqueda = True
        Me.Text = "Buscar Producto"
        CambiarEstado("Haga doble click en el registro para seleccionarlo.")
        panelTop.Visible = False
    End Sub

    Private Function BuscarProductos(p As Producto, menorprecio As Boolean) As DataSet

        If p.Nombre <> String.Empty Then
            Return Producto.SelectByName(p)
        ElseIf p.Categoria <> String.Empty Then
            Return Producto.SelectByCategoria(p)
        ElseIf p.Precio <> 0 Then
            If menorprecio Then
                Return Producto.SelectByMenorPrecio(p)
            Else
                Return Producto.SelectByMayorPrecio(p)
            End If
        End If

    End Function

    Private Sub FrmAdministracionProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ModoBusqueda And (ProductoBase IsNot Nothing) Then
            Dim ds As DataSet = BuscarProductos(ProductoBase, BuscarProductoMenorPrecio)
            CargarRegistros(ds)
        Else
            CargarTodosLosRegistros()
        End If
    End Sub

    Private Sub BtnAñadir_Click() Handles btnAñadir.Click
        Dim nuevo_producto As New Producto
        Using frm As New frmProducto
            frm.producto = nuevo_producto
            frm.ShowDialog()
            nuevo_producto = frm.producto
        End Using
        If nuevo_producto Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)

            Producto.Insert(nuevo_producto)

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Private Sub BtnEditar_Click() Handles btnEditar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        Dim p As Producto = Me.ProductoSeleccionado
        Using frm As New frmProducto
            frm.Producto = p
            frm.ShowDialog()
            p = frm.Producto
        End Using
        If p Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)

            Producto.Update(p)

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub
    Private Sub BtnEliminar_Click() Handles btnEliminar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        Dim id_producto As Integer = ProductoSeleccionado.ID
        Dim cantidad_ventas As Integer = Producto.CantidadVentasPorIdProducto(id_producto)

        If cantidad_ventas > 0 Then
            Const mensaje As String = "El producto seleccionado se incluye {0} venta(s)." & vbNewLine & "No se puede eliminar."
            MsgBox(String.Format(mensaje, cantidad_ventas), MsgBoxStyle.Critical, "Eliminar")
            Exit Sub
        End If

        If MsgBox("¿Eliminar este registro?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Eliminar") = MsgBoxResult.No Then
            CambiarEstado("Operación cancelada.")
            Exit Sub
        End If

        Try
            CambiarEstado("Eliminando registro.")
            HabilitarPanel(False)

            Producto.DeleteById(id_producto)

            CargarTodosLosRegistros()
            CambiarEstado("Registro eliminado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al eliminar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub
    Private Sub BtnRecargar_Click() Handles btnRecargar.Click
        CargarTodosLosRegistros()
    End Sub
    Private Sub BtnBuscar_Click() Handles btnBuscar.Click
        ' Ingresar producto
        Dim buscar_producto As New Producto
        Dim buscar_xMenorPrecio As Boolean
        Using frm As New frmBuscarProducto
            frm.Producto = buscar_producto
            frm.ShowDialog()
            buscar_producto = frm.Producto
            buscar_xMenorPrecio = frm.BuscarPrecioMenor
        End Using
        If buscar_producto Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        ' Buscar
        Try
            CambiarEstado("Buscando registros.")
            HabilitarPanel(False)

            Dim regs As DataSet = BuscarProductos(buscar_producto, buscar_xMenorPrecio)

            btnQuitarFiltros.Visible = True
            btnRecargar.Visible = False
            CargarRegistros(regs)
        Catch ex As Exception
            CambiarEstado("Error al buscar.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Private Sub BtnQuitarFiltros_Click(sender As Object, e As EventArgs) Handles btnQuitarFiltros.Click
        btnQuitarFiltros.Visible = False
        btnRecargar.Visible = True
        CargarTodosLosRegistros()
    End Sub

    Private Sub grid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles grid.CellDoubleClick
        If ModoBusqueda Then
            If HayRegistroSeleccionado() Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        Else
            BtnEditar_Click()
        End If
    End Sub

    Private Sub frmAdministracionProductos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.DialogResult <> DialogResult.OK Then
            Me.DialogResult = DialogResult.Cancel
            grid.ClearSelection()
        End If
    End Sub
End Class
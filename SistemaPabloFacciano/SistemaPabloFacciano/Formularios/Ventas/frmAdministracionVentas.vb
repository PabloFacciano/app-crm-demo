Imports System.Data.SqlClient
Public Class frmAdministracionVentas


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

    Public ReadOnly Property VentaSeleccionada As Venta
        Get
            If Not HayRegistroSeleccionado() Then Return Nothing
            Dim row As DataGridViewRow = grid.SelectedRows(0)
            Dim v As New Venta With {
                .ID = row.Cells("ID").Value,
                .Fecha = row.Cells("Fecha").Value,
                .IDCliente = row.Cells("IDCliente").Value,
                .Total = row.Cells("Total").Value
            }
            row = Nothing
            Return v
        End Get
    End Property

    Private Sub CambiarEstado(nuevo_estado As String)
        estado.Text = nuevo_estado
    End Sub
    Sub CargarTodosLosRegistros()
        CargarRegistros(Venta.SelectAll_ConNombreDeCliente())
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
            grid.Columns(1).Visible = False
            grid.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            grid.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            grid.Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        Else
            CambiarEstado("No hay registros que mostrar.")
        End If
    End Sub

    Private Property ModoBusqueda As Boolean = False
    Sub ActivarModoBusqueda()
        ModoBusqueda = True
        Me.Text = "Buscar Venta"
        CambiarEstado("Haga doble click en el registro para seleccionarlo.")
        panelTop.Visible = False
    End Sub


    Private Sub FrmAdministracionVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ModoBusqueda Then

        Else
            CargarTodosLosRegistros()
        End If
    End Sub

    Private Sub BtnAñadir_Click() Handles btnAñadir.Click

        ' Ingrsar Venta + Items
        Dim productos_elegidos As List(Of VentaItem)
        Dim nueva_venta As New Venta
        nueva_venta.Fecha = Now

        Using frm As New frmVenta
            frm.Venta = nueva_venta
            frm.LoadVenta(EsNuevaVenta:=True)
            frm.ShowDialog()
            nueva_venta = frm.Venta
            productos_elegidos = frm.VentaItems
        End Using
        If nueva_venta Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If

        ' Subir datos a BD 
        ' Actualizar código próximamente para utilizar Transacciones
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)

            ' Guardar Venta
            Dim idventa As Integer = Venta.Insert(nueva_venta)

            ' Guardar Items de Venta
            For Each item As VentaItem In productos_elegidos
                item.IDVenta = idventa
                VentaItem.Insert(item)
            Next

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Private Sub BtnEditar_Click() Handles btnEditar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        ' Ingrsar Venta + Items
        Dim v As Venta = VentaSeleccionada

        Using frm As New frmVenta
            frm.Venta = v
            frm.LoadVenta(EsNuevaVenta:=False)
            frm.ShowDialog()
            v = frm.Venta
        End Using
        If v Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If

        ' Subir datos a BD 
        ' Actualizar código próximamente para utilizar Transacciones
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)


            ' Guardar Venta
            Venta.Update(v)

            ' Los Items de Venta se guardan automáticamente.

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub
    Private Sub BtnEliminar_Click() Handles btnEliminar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        Dim id_venta As Integer = VentaSeleccionada.ID
        Dim cantidad_items As Integer = Venta.CantidadItemsPorIdVenta(id_venta)

        If MsgBox("¿Eliminar este registro?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Eliminar") = MsgBoxResult.No Then
            CambiarEstado("Operación cancelada.")
            Exit Sub
        End If

        Try
            CambiarEstado("Eliminando registro.")
            HabilitarPanel(False)

            VentaItem.DeleteByIdVenta(id_venta)
            Venta.DeleteById(id_venta)

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

        ' Ingresar Parámetros de Busqueda
        Dim fecha_inicio As Date = Now
        Dim fecha_fin As Date = Now
        Dim precio_base As Single
        Dim menor_precio As Boolean
        Dim id_cliente As Integer
        Dim id_producto As Integer
        Dim result As DialogResult = DialogResult.Cancel
        Using frm As New frmBuscarVenta
            frm.FechaInicial = fecha_inicio
            frm.FechaFin = fecha_fin
            result = frm.ShowDialog()
            fecha_inicio = frm.FechaInicial
            fecha_fin = frm.FechaFin
            precio_base = frm.PrecioBase
            menor_precio = frm.MenorPrecio
            id_cliente = frm.IdCliente
            id_producto = frm.IdProducto
        End Using
        If result <> DialogResult.OK Then
            Exit Sub
        End If

        ' Buscar
        Try
            CambiarEstado("Buscando registros.")
            HabilitarPanel(False)

            Dim regs As DataSet = Nothing

            If precio_base <> 0 Then
                If menor_precio Then
                    regs = Venta.SelectByMenorTotal_ConNombreDeCliente(precio_base)
                Else
                    regs = Venta.SelectByMayorTotal_ConNombreDeCliente(precio_base)
                End If
            ElseIf id_producto <> 0 Then
                regs = Venta.SelectByIdProducto_ConNombreDeCliente(id_producto)
            ElseIf id_cliente <> 0 Then
                regs = Venta.SelectByIdCliente_ConNombreDeCliente(id_cliente)
            Else
                regs = Venta.SelectByFechas_ConNombreDeCliente(fecha_inicio, fecha_fin)
            End If


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
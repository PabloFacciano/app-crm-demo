Public Class frmAdministracionClientes

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

    Public ReadOnly Property ClienteSeleccionado As Cliente
        Get
            If Not HayRegistroSeleccionado() Then Return Nothing
            Dim row As DataGridViewRow = grid.SelectedRows(0)
            Dim c As New Cliente With {
                .ID = row.Cells("ID").Value,
                .Cliente = row.Cells("Cliente").Value,
                .Telefono = row.Cells("Telefono").Value,
                .Correo = row.Cells("Correo").Value
            }
            row = Nothing
            Return c
        End Get
    End Property

    Private Sub CambiarEstado(nuevo_estado As String)
        estado.Text = nuevo_estado
    End Sub
    Sub CargarTodosLosRegistros()
        CargarRegistros(Cliente.SelectAll())
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

    Public Property ClienteBase As Cliente
    Private Property ModoBusqueda As Boolean = False
    Sub ActivarModoBusqueda()
        ModoBusqueda = True
        Me.Text = "Buscar Cliente"
        CambiarEstado("Haga doble click en el registro para seleccionarlo.")
        panelTop.Visible = False
    End Sub

    Private Function BuscarCliente(c As Cliente) As DataSet

        If (c.Correo <> String.Empty) Then
            Return Cliente.SelectByCorreo(c)
        ElseIf NombreClienteEsValido(c.Cliente) Then
            Return Cliente.SelectByName(c)
        ElseIf TelefonoEsValido(c.Telefono) Then
            Return Cliente.SelectByTelefono(c)
        End If

    End Function

    Private Sub FrmAdministracionClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ModoBusqueda And (ClienteBase IsNot Nothing) Then
            Dim ds As DataSet = BuscarCliente(ClienteBase)
            CargarRegistros(ds)
        Else
            CargarTodosLosRegistros()
        End If
    End Sub

    Private Sub BtnAñadir_Click() Handles btnAñadir.Click
        Dim nuevo_cliente As New Cliente
        Using frm As New frmCliente
            frm.Cliente = nuevo_cliente
            frm.ShowDialog()
            nuevo_cliente = frm.Cliente
        End Using
        If nuevo_cliente Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)

            Cliente.Insert(nuevo_cliente)

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Private Sub BtnEditar_Click() Handles btnEditar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        Dim c As Cliente = Me.ClienteSeleccionado
        Using frm As New frmCliente
            frm.Cliente = c
            frm.ShowDialog()
            c = frm.Cliente
        End Using
        If c Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        Try
            CambiarEstado("Guardando registro.")
            HabilitarPanel(False)

            Cliente.Update(c)

            CargarTodosLosRegistros()
            CambiarEstado("Registro guardado con éxito.")
        Catch ex As Exception
            CambiarEstado("Error al guardar el registro.")
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub
    Private Sub BtnEliminar_Click() Handles btnEliminar.Click
        If Not HayRegistroSeleccionado() Then Beep() : Exit Sub

        Dim id_cliente As Integer = ClienteSeleccionado.ID
        Dim cantidad_ventas As Integer = Cliente.CantidadVentasPorIdCliente(id_cliente)

        If cantidad_ventas > 0 Then
            Const mensaje As String = "El Cliente seleccionado posee {0} venta(s) a su nombre." & vbNewLine & "No se puede eliminar."
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

            Cliente.DeleteById(id_cliente)

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
        ' Ingresar Cliente
        Dim buscar_cliente As New Cliente
        Using frm As New frmBuscarCliente
            frm.Cliente = buscar_cliente
            frm.ShowDialog()
            buscar_cliente = frm.Cliente
        End Using
        If buscar_cliente Is Nothing Then
            CambiarEstado("Operacion cancelada.")
            Exit Sub
        End If
        ' Buscar
        Try
            CambiarEstado("Buscando registros.")
            HabilitarPanel(False)

            Dim regs As DataSet = BuscarCliente(buscar_cliente)

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
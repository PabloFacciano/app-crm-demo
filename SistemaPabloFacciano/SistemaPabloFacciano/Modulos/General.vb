Imports System.Configuration

Module General

    Public Function GetConnectionString() As String
        Return ConfigurationManager.ConnectionStrings("cnxString").ToString()
    End Function

    Public Function BuscarCliente() As Cliente
        Dim C As Cliente = Nothing

        ' Ingresar Cliente
        Dim buscar_cliente As New Cliente
        Using frm As New frmBuscarCliente
            frm.Cliente = buscar_cliente
            frm.ShowDialog()
            buscar_cliente = frm.Cliente
        End Using
        If buscar_cliente Is Nothing Then Return Nothing
        ' Buscar
        Try
            Using frm As New frmAdministracionClientes
                frm.ClienteBase = buscar_cliente
                frm.ActivarModoBusqueda()
                frm.ShowDialog()
                C = frm.ClienteSeleccionado
            End Using

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return C
    End Function
    Public Function CrearCliente() As Cliente

        Dim nuevo_cliente As New Cliente
        Using frm As New frmCliente
            frm.Cliente = nuevo_cliente
            frm.ShowDialog()
            nuevo_cliente = frm.Cliente
        End Using
        If nuevo_cliente Is Nothing Then Return Nothing

        Try
            Cliente.Insert(nuevo_cliente)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return nuevo_cliente
    End Function

    Public Function BuscarProducto() As Producto
        Dim P As Producto = Nothing

        ' Ingresar Producto
        Dim buscar_producto As New Producto
        Dim buscar_xMenorPrecio As Boolean
        Using frm As New frmBuscarProducto
            frm.Producto = buscar_producto
            frm.ShowDialog()
            buscar_producto = frm.Producto
            buscar_xMenorPrecio = frm.BuscarPrecioMenor
        End Using
        If buscar_producto Is Nothing Then Return Nothing
        ' Buscar
        Try

            Dim regs As DataSet = Nothing

            Using frm As New frmAdministracionProductos
                frm.ProductoBase = buscar_producto
                frm.BuscarProductoMenorPrecio = buscar_xMenorPrecio
                frm.ActivarModoBusqueda()
                frm.ShowDialog()
                P = frm.ProductoSeleccionado
            End Using

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        Return P
    End Function

End Module

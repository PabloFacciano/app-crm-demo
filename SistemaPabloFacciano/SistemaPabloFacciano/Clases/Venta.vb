Public Class Venta

    Public Property ID As Integer
    Public Property IDCliente As Integer
    Public Property Fecha As Date
    Public Property Total As Single


    Public Shared Function SelectAll() As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Ventas.Fecha,Ventas.Total FROM Ventas"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function

    Public Shared Function SelectAll_ConNombreDeCliente() As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Clientes.Cliente,Ventas.Fecha,Ventas.Total FROM Ventas INNER JOIN Clientes ON (Clientes.ID = Ventas.IDCliente)"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByFechas_ConNombreDeCliente(fechaInicio As Date, fechaFin As Date) As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Clientes.Cliente,Ventas.Fecha,Ventas.Total FROM Ventas INNER JOIN Clientes ON (Clientes.ID = Ventas.IDCliente) WHERE ((Ventas.Fecha >= @_fechainicio) AND (Ventas.Fecha <= @_fechafin))"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_fechainicio", fechaInicio)
                comando.Parameters.AddWithValue("@_fechafin", fechaFin)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByIdCliente_ConNombreDeCliente(idcliente As Integer) As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Clientes.Cliente,Ventas.Fecha,Ventas.Total FROM Ventas INNER JOIN Clientes ON (Clientes.ID = Ventas.IDCliente) WHERE Ventas.IDCliente=@_idcliente"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idcliente", idcliente)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByIdProducto_ConNombreDeCliente(idproducto As Integer) As DataSet
        Dim query As String = "SELECT "
        query &= "Ventas.ID, Ventas.IDCliente, Clientes.Cliente, Ventas.Fecha, Ventas.Total "
        query &= "FROM Ventas "
        query &= "INNER Join Clientes "
        query &= "On Clientes.ID = Ventas.IDCliente "
        query &= "INNER Join VentasItems "
        query &= "On VentasItems.IDVenta = Ventas.ID "
        query &= "INNER Join Productos "
        query &= "On Productos.ID = VentasItems.IDProducto "
        query &= "WHERE Productos.ID = @_idproducto"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idproducto", idproducto)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByMenorTotal_ConNombreDeCliente(preciobase As Single) As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Clientes.Cliente,Ventas.Fecha,Ventas.Total FROM Ventas INNER JOIN Clientes ON (Clientes.ID = Ventas.IDCliente) WHERE Ventas.Total < @_total"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_total", preciobase)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByMayorTotal_ConNombreDeCliente(preciobase As Single) As DataSet
        Const query As String = "SELECT Ventas.ID,Ventas.IDCliente,Clientes.Cliente,Ventas.Fecha,Ventas.Total FROM Ventas INNER JOIN Clientes ON (Clientes.ID = Ventas.IDCliente) WHERE Ventas.Total > @_total"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_total", preciobase)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function

    Public Shared Function Insert(v As Venta) As Integer
        Const query As String = "INSERT INTO Ventas(Ventas.IDCliente,Ventas.Fecha,Ventas.Total) OUTPUT inserted.ID VALUES (@_idcliente, @_fecha, @_total)"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idcliente", v.IDCliente)
                comando.Parameters.AddWithValue("@_fecha", v.Fecha)
                comando.Parameters.AddWithValue("@_total", v.Total)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    If _ds.Tables(0).Rows.Count > 0 Then
                        Return Int(_ds.Tables(0).Rows(0).Item(0))
                    End If
                End Using
            End Using
        End Using

    End Function
    Public Shared Sub Update(v As Venta)
        Const query As String = "UPDATE Ventas SET Ventas.IDCliente=@_idcliente,Ventas.Fecha=@_fecha,Ventas.total=@_total WHERE Ventas.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", v.ID)
                comando.Parameters.AddWithValue("@_idcliente", v.IDCliente)
                comando.Parameters.AddWithValue("@_fecha", v.Fecha)
                comando.Parameters.AddWithValue("@_total", v.Total)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub DeleteById(id_venta As Integer)
        Const query As String = "DELETE FROM Ventas WHERE Ventas.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", id_venta)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub

    Public Shared Function CantidadItemsPorIdVenta(id_venta As Integer) As Integer
        Const query As String = "SELECT Count(*) FROM VentasItems WHERE VentasItems.IDVenta=@_idventa"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idventa", id_venta)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using

        Return Int(ds.Tables(0).Rows(0).Item(0))
    End Function
End Class

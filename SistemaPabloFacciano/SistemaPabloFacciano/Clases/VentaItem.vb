Public Class VentaItem

    ' Mensaje para el Administrador de Base de Datos: 
    ' > El campo ID no es necesario, ya que se pueden utilizar IDVenta junto a IDProducto para hacer una clave compuesta (de dos campos). 4ta Forma Normal de Base de Datos.
    ' > El campo PrecioTotal debería ser un campo calculado automáticamente.
    ' ;)

    Public Property ID As Integer
    Public Property IDVenta As Integer
    Public Property IDProducto As Integer
    Public Property PrecioUnitario As Single
    Public Property Cantidad As Integer
    Public Property PrecioTotal As Single


    Public Shared Function SelectAll() As DataSet
        Const query As String = "SELECT VentasItems.ID,VentasItems.IDVenta,VentasItems.IDProducto,VentasItems.PrecioUnitario,VentasItems.Cantidad,VentasItems.PrecioTotal FROM VentasItems"

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
    Public Shared Function SelectByIdVenta(id_venta As Integer) As DataSet
        Const query As String = "SELECT VentasItems.ID,VentasItems.IDVenta,VentasItems.IDProducto,VentasItems.PrecioUnitario,VentasItems.Cantidad,VentasItems.PrecioTotal FROM VentasItems WHERE VentasItems.IDVenta=@_idventa"

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
        Return ds

    End Function


    Public Shared Sub Insert(v As VentaItem)
        Const query As String = "INSERT INTO VentasItems(VentasItems.IDVenta,VentasItems.IDProducto,VentasItems.PrecioUnitario,VentasItems.Cantidad,VentasItems.PrecioTotal) VALUES (@_idventa,@_idproducto,@_preciounitario,@_cantidad,@_preciototal)"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idventa", v.IDVenta)
                comando.Parameters.AddWithValue("@_idproducto", v.IDProducto)
                comando.Parameters.AddWithValue("@_preciounitario", v.PrecioUnitario)
                comando.Parameters.AddWithValue("@_cantidad", v.Cantidad)
                comando.Parameters.AddWithValue("@_preciototal", v.PrecioTotal)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub Update(v As VentaItem)
        Const query As String = "UPDATE VentasItems SET VentasItems.IDVenta=@_idventa,VentasItems.IDProducto=@_idproducto,VentasItems.PrecioUnitario=@_preciounitario,VentasItems.Cantidad=@_cantidad,VentasItems.PrecioTotal=@_preciototal WHERE VentasItems.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", v.ID)
                comando.Parameters.AddWithValue("@_idventa", v.IDVenta)
                comando.Parameters.AddWithValue("@_idproducto", v.IDProducto)
                comando.Parameters.AddWithValue("@_preciounitario", v.PrecioUnitario)
                comando.Parameters.AddWithValue("@_cantidad", v.Cantidad)
                comando.Parameters.AddWithValue("@_preciototal", v.PrecioTotal)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub DeleteById(id_ventaitem As Integer)
        Const query As String = "DELETE FROM VentasItems WHERE VentasItems.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", id_ventaitem)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub DeleteByIdVenta(id_venta As Integer)
        Const query As String = "DELETE FROM VentasItems WHERE VentasItems.IDVenta=@_idventa"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idventa", id_venta)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub

End Class

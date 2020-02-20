Public Class Producto

    Public Property ID As Integer
    Public Property Nombre As String
    Public Property Precio As Single
    Public Property Categoria As String

    Public Shared Function SelectAll() As DataSet
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos"

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
    Public Shared Function SelectByID(idproducto As Integer) As Producto
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos WHERE Productos.ID = @_idproducto"

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

        If ds Is Nothing Then Return Nothing

        Dim row As DataRow = ds.Tables(0).Rows(0)
        Dim c As New Producto() With {
            .ID = row("ID"),
            .Categoria = row("Categoria"),
            .Nombre = row("Nombre"),
            .Precio = row("Precio")
        }
        row = Nothing

        Return c
    End Function
    Public Shared Function SelectNameByID(idproducto As Integer) As String
        Const query As String = "SELECT Productos.Nombre FROM Productos WHERE Productos.ID = @_idproducto"

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

        If ds Is Nothing Then Return Nothing
        Dim resultado As String = ds.Tables(0).Rows(0).Item(0)

        Return resultado
    End Function
    Public Shared Function SelectByName(p As Producto) As DataSet
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos WHERE Productos.Nombre LIKE @_nombre"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_nombre", "%" & p.Nombre & "%")
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByMenorPrecio(p As Producto) As DataSet
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos WHERE Productos.Precio < @_precio"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_precio", p.Precio)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByMayorPrecio(p As Producto) As DataSet
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos WHERE Productos.Precio > @_precio"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_precio", p.Precio)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByCategoria(p As Producto) As DataSet
        Const query As String = "SELECT Productos.ID, Productos.Nombre, Productos.Precio, Productos.Categoria FROM Productos WHERE Productos.Categoria LIKE @_categoria"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_categoria", "%" & p.Categoria & "%")
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectTodasLasCategorias() As DataSet
        Const query As String = "SELECT DISTINCT Productos.Categoria FROM Productos"

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

    Public Shared Sub Insert(p As Producto)
        Const query As String = "INSERT INTO Productos(Productos.Nombre,Productos.Precio,Productos.Categoria) VALUES (@_nombre,@_precio,@_categoria)"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_nombre", p.Nombre)
                comando.Parameters.AddWithValue("@_precio", p.Precio)
                comando.Parameters.AddWithValue("@_categoria", p.Categoria)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub Update(p As Producto)
        Const query As String = "UPDATE Productos SET Productos.Nombre=@_nombre, Productos.Precio=@_precio, Productos.Categoria=@_categoria WHERE Productos.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", p.ID)
                comando.Parameters.AddWithValue("@_nombre", p.Nombre)
                comando.Parameters.AddWithValue("@_precio", p.Precio)
                comando.Parameters.AddWithValue("@_categoria", p.Categoria)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub DeleteById(id_producto As Integer)
        Const query As String = "DELETE FROM Productos WHERE Productos.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", id_producto)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub

    Public Shared Function CantidadVentasPorIdProducto(id_producto As Integer) As Integer
        Const query As String = "SELECT Count(*) FROM VentasItems WHERE VentasItems.IDProducto=@_idproducto GROUP BY VentasItems.IDVenta"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idproducto", id_producto)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using

        Dim hayResultados As Boolean = (ds.Tables(0).Rows.Count > 0)
        If hayResultados Then
            Return Int(ds.Tables(0).Rows(0).Item(0))
        Else
            Return 0
        End If

    End Function


End Class

Imports System.Data.SqlClient

Public Class Cliente

    Public Property ID As Integer
    Public Property Cliente As String
    Public Property Telefono As Long
    Public Property Correo As String



    Public Shared Function SelectAll() As DataSet
        Const query As String = "SELECT Clientes.ID, Clientes.Cliente, Clientes.Telefono, Clientes.Correo FROM Clientes"

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
    Public Shared Function SelectByID(idcliente As Integer) As Cliente
        Const query As String = "SELECT Clientes.ID, Clientes.Cliente, Clientes.Telefono, Clientes.Correo FROM Clientes WHERE Clientes.ID=@_idcliente"

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

        If ds Is Nothing Then Return Nothing

        Dim row As DataRow = ds.Tables(0).Rows(0)
        Dim c As New Cliente() With {
            .ID = row("ID"),
            .Cliente = row("Cliente"),
            .Correo = row("Correo"),
            .Telefono = row("Telefono")
        }
        row = Nothing

        Return c
    End Function
    Public Shared Function SelectByName(c As Cliente) As DataSet
        Const query As String = "SELECT Clientes.ID, Clientes.Cliente, Clientes.Telefono, Clientes.Correo FROM Clientes WHERE Clientes.Cliente LIKE @_nombre"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_nombre", "%" & c.Cliente & "%")
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByTelefono(c As Cliente) As DataSet
        Const query As String = "SELECT Clientes.ID, Clientes.Cliente, Clientes.Telefono, Clientes.Correo FROM Clientes WHERE Clientes.Telefono=@_telefono"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_telefono", c.Telefono)
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Function SelectByCorreo(c As Cliente) As DataSet
        Const query As String = "SELECT Clientes.ID, Clientes.Cliente, Clientes.Telefono, Clientes.Correo FROM Clientes WHERE Clientes.Correo LIKE @_correo"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_correo", "%" & c.Correo & "%")
                Using adap As New SqlClient.SqlDataAdapter(comando)
                    Dim _ds As New DataSet
                    adap.Fill(_ds)
                    ds = _ds
                End Using
            End Using
        End Using
        Return ds

    End Function
    Public Shared Sub Insert(c As Cliente)
        Const query As String = "INSERT INTO Clientes(Clientes.Cliente,Clientes.Telefono,Clientes.Correo) VALUES (@_cliente,@_telefono,@_correo)"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_cliente", c.Cliente)
                comando.Parameters.AddWithValue("@_telefono", c.Telefono)
                comando.Parameters.AddWithValue("@_correo", c.Correo)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub Update(c As Cliente)
        Const query As String = "UPDATE Clientes SET Clientes.Cliente=@_cliente, Clientes.Telefono=@_telefono, Clientes.Correo=@_correo WHERE Clientes.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", c.ID)
                comando.Parameters.AddWithValue("@_cliente", c.Cliente)
                comando.Parameters.AddWithValue("@_telefono", c.Telefono)
                comando.Parameters.AddWithValue("@_correo", c.Correo)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Sub DeleteById(id_cliente As Integer)
        Const query As String = "DELETE FROM Clientes WHERE Clientes.ID=@_id"

        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_id", id_cliente)
                comando.ExecuteNonQuery()
            End Using
        End Using

    End Sub
    Public Shared Function CantidadVentasPorIdCliente(id_cliente As Integer) As Integer
        Const query As String = "SELECT Count(*) FROM Ventas WHERE Ventas.IDCliente=@_idcliente"

        Dim ds As DataSet = Nothing
        Using con As New SqlClient.SqlConnection(GetConnectionString)
            con.Open()
            Using comando As New SqlClient.SqlCommand(query, con)
                comando.Parameters.AddWithValue("@_idcliente", id_cliente)
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

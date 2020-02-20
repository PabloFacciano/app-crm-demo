Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' Los siguientes eventos están disponibles para MyApplication:
    ' Inicio: Se genera cuando se inicia la aplicación, antes de que se cree el formulario de inicio.
    ' Apagado: Se genera después de haberse cerrado todos los formularios de aplicación.  Este evento no se genera si la aplicación termina de forma anómala.
    ' UnhandledException: Se genera si la aplicación encuentra una excepción no controlada.
    ' StartupNextInstance: Se genera cuando se inicia una aplicación de instancia única y dicha aplicación está ya activa. 
    ' NetworkAvailabilityChanged: Se genera cuando se conecta o desconecta la conexión de red.
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MsgBox(e.Exception.Message, MsgBoxStyle.Critical, "Error no controlado!")
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            MsgBox("Sistema Demo ya se está ejecutando en otra ventana.", MsgBoxStyle.Information, "Sistema Demo")
        End Sub

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            Try
                Using con As New SqlClient.SqlConnection(GetConnectionString)
                    con.Open()
                End Using
            Catch ex As Exception
                MsgBox("No pudimos conectarnos a la base de datos." & vbNewLine & "Deberías verificar la cadena de conexion en App.config", MsgBoxStyle.Critical, "Sistema - Pablo Facciano")
            End Try
        End Sub

    End Class
End Namespace

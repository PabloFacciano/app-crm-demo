Module Validaciones

    Public Function NombreClienteEsValido(nombre As String) As Boolean
        Return (Trim(nombre) <> String.Empty)
    End Function

    Public Function EmailEsValido(email As String) As Boolean
        Dim resultado As Boolean = False

        If email.Contains("@") Then
            Dim index_arroba As Integer = email.IndexOf("@")
            Dim dominio As String = email.Substring(index_arroba + 1)
            If dominio.Contains(".") Then
                resultado = True
            End If
        End If

        Return resultado
    End Function

    Public Function TelefonoEsValido(telefono As String) As Boolean
        Dim resultado As Boolean = False

        If IsNumeric(telefono) Then
            If Int(telefono > 0) Then
                resultado = True
            End If
        End If

        Return resultado
    End Function

End Module

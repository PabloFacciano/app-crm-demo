Public Class frmMain
    Private Sub BtnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        Me.Hide()
        Using frm As New frmAdministracionClientes
            frm.ShowDialog()
        End Using
        Me.Show()
    End Sub

    Private Sub btnProductos_Click(sender As Object, e As EventArgs) Handles btnProductos.Click
        Me.Hide()
        Using frm As New frmAdministracionProductos
            frm.ShowDialog()
        End Using
        Me.Show()
    End Sub

    Private Sub btnVentas_Click(sender As Object, e As EventArgs) Handles btnVentas.Click
        Me.Hide()
        Using frm As New frmAdministracionVentas
            frm.ShowDialog()
        End Using
        Me.Show()
    End Sub

    Private Sub LinkMiWeb_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkMiWeb.LinkClicked
        Process.Start(linkMiWeb.Text)
    End Sub

End Class

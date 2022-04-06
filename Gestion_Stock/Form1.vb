Imports System.Data.SqlClient
Public Class Form1
    Public dr As SqlDataReader
    Public cmd As New SqlCommand

    Public dr2 As SqlDataReader
    Public cmd2 As New SqlCommand

    Private Sub afficher()
        cn.Open()
        Dim listems As ListViewItem
        Dim str As String = "select * from Produits"
        cmd = New SqlCommand(str, cn)
        dr = cmd.ExecuteReader
        ListView1.Items.Clear()
        While (dr.Read)
            listems = Me.ListView1.Items.Add(dr("id_prod"))
            listems.SubItems.Add(dr("libelle"))
            listems.SubItems.Add(dr("pu_prod"))
            listems.SubItems.Add(dr("type_prod"))
            listems.SubItems.Add(dr("prix_vente"))
            listems.SubItems.Add(dr("note_prod"))
            listems.SubItems.Add(dr("code_barre"))
        End While

        dr.Close()

        Dim str2 As String = "select count(*) as total,max(pu_prod) as maxpu,min(pu_prod) as minpu from Produits"
        cmd2 = New SqlCommand(str2, cn)
        dr2 = cmd2.ExecuteReader
        dr2.Read()
        Me.lbl_total.Text = "Nombre Prod: " & dr2.Item(0).ToString
        Me.lbl_pumax.Text = "Max.Pu: " & dr2.Item(1).ToString
        Me.lbl_pumin.Text = "Min_pu : " & dr2.Item(2).ToString
        dr2.Close()

        cn.Close()
    End Sub
    Private Sub type()
        cn.Open()
        Dim str As String = "select distinct type_prod from Produits"
        cmd = New SqlCommand(str, cn)
        dr = cmd.ExecuteReader
        While (dr.Read)
            Me.combo_type.Items.Add(dr("type_prod")).ToString()
        End While

        dr.Close()
        cn.Close()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficher()
        type()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txt_idprod.Text = Nothing Or txt_libelle.Text = Nothing Or txt_pu.Text = Nothing Or combo_type.Text = Nothing Or txt_pv.Text = Nothing Or txt_note.Text = Nothing Or txt_cb.Text = Nothing Then
            MsgBox("Remplir Tout Les Champs Pour Ajouter !")
        Else
            If MsgBox("Voulez Vous Vraiment Ajouter?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.Yes Then
                cn.Open()
                cmd.CommandText = "insert into produits values('" & txt_idprod.Text & "','" & txt_libelle.Text & "','" & txt_pu.Text & "','" & combo_type.Text & "', '" & txt_pv.Text & "', '" & txt_note.Text & "', '" & txt_cb.Text & "')"
                cmd.ExecuteNonQuery()
                MsgBox("Ajouté Avec Succes :)")
                cn.Close()
                afficher()
            End If
        End If
    End Sub

    Private Sub ListView1_Click(sender As Object, e As EventArgs) Handles ListView1.Click
        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Selectionner un Produit !")
        Else
            cn.Open()
            cmd.CommandText = "select * from produits where id_prod='" & ListView1.SelectedItems(0).Text.ToString & "'"
            cmd.ExecuteNonQuery()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read Then
                Me.txt_idprod.Text = dr.Item(0).ToString
                Me.txt_libelle.Text = dr.Item(1).ToString
                Me.txt_pu.Text = dr.Item(2).ToString
                Me.txt_pu.Text = Replace(txt_pu.Text, ",", ".")

                Me.combo_type.Text = dr.Item(3).ToString
                Me.txt_pv.Text = dr.Item(4).ToString
                Me.txt_pv.Text = Replace(txt_pv.Text, ",", ".")

                Me.txt_note.Text = dr.Item(5)
                Me.txt_cb.Text = dr.Item(6).ToString
                dr.Close()

            End If
            cn.Close()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If txt_idprod.Text = Nothing Or txt_libelle.Text = Nothing Or txt_pu.Text = Nothing Or combo_type.Text = Nothing Or txt_pv.Text = Nothing Or txt_note.Text = Nothing Or txt_cb.Text = Nothing Then
            MsgBox("Remplir Tout Les Champs Pour Modifier !")
        Else
            If MsgBox("Voulez Vous Vraiment Modifier?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.Yes Then
                cn.Open()
                cmd.CommandText = "update produits set libelle='" & txt_libelle.Text & "',pu_prod=" & txt_pu.Text & ",type_prod='" & combo_type.Text & "',prix_vente=" & txt_pv.Text & ",note_prod='" & txt_note.Text & "',code_barre='" & txt_cb.Text & "' where id_prod=(" & txt_idprod.Text & ")"
                cmd.ExecuteNonQuery()
                MsgBox("Modifier Avec Succes :)")
                cn.Close()
                afficher()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If txt_idprod.Text = Nothing Or txt_libelle.Text = Nothing Or txt_pu.Text = Nothing Or combo_type.Text = Nothing Or txt_pv.Text = Nothing Or txt_note.Text = Nothing Or txt_cb.Text = Nothing Then
            MsgBox("Remplir Tout Les Champs Pour Supprimer !")
        Else
            If MsgBox("Voulez Vous Vraiment Supprimer?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = Windows.Forms.DialogResult.Yes Then
                cn.Open()
                cmd.CommandText = "delete from produits where id_prod='" & txt_idprod.Text & "'"
                cmd.ExecuteNonQuery()
                MsgBox("Suppression Avec Succes :)")
                cn.Close()
                afficher()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txt_idprod.Text = Nothing
        txt_libelle.Text = Nothing
        txt_pu.Text = Nothing
        combo_type.Text = Nothing
        txt_pv.Text = Nothing
        txt_note.Text = Nothing
        txt_cb.Text = Nothing

        txt_idprod.Focus()
    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles txt_rech.KeyUp
        cn.Open()
        Dim listems As ListViewItem
        Dim str As String = "select * from Produits where " & combo_rech.Text & " like '%' + @1 + '%' "
        cmd = New SqlCommand(str, cn)
        cmd.Parameters.AddWithValue("@1", txt_rech.Text)
        dr = cmd.ExecuteReader
        ListView1.Items.Clear()
        While (dr.Read)
            listems = Me.ListView1.Items.Add(dr("id_prod"))
            listems.SubItems.Add(dr("libelle"))
            listems.SubItems.Add(dr("pu_prod"))
            listems.SubItems.Add(dr("type_prod"))
            listems.SubItems.Add(dr("prix_vente"))
            listems.SubItems.Add(dr("note_prod"))
            listems.SubItems.Add(dr("code_barre"))
        End While
        dr.Close()


        Dim str2 As String = "select count(*) as total,max(pu_prod) as maxpu,min(pu_prod) as minpu from Produits where" & combo_rech.Text
        cmd2 = New SqlCommand(str2, cn)
        cmd2.Parameters.AddWithValue("@1", txt_rech.Text)
        dr2 = cmd2.ExecuteReader
        dr2.Read()
        Me.lbl_total.Text = "Nombre Prod: " & dr2.Item(0).ToString
        Me.lbl_pumax.Text = "Max.Pu: " & dr2.Item(1).ToString
        Me.lbl_pumin.Text = "Min_Pu: " & dr2.Item(2).ToString

        dr2.Close()
        cn.Close()
    End Sub

    Private Sub combo_rech_TextChanged(sender As Object, e As EventArgs) Handles combo_rech.TextChanged
        txt_rech.Text = Nothing
        txt_rech.Focus()
    End Sub
End Class

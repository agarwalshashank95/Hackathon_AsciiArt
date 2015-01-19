Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        init()
        strFileNameAndPath = OpenFile()
        If (strFileNameAndPath <> "") Then
            PictureBox1.Image = New Bitmap(strFileNameAndPath)
            TextBox1.Text = strFileNameAndPath
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If (strFileNameAndPath = "") Then
            MsgBox("You did not select a file!")
        Else
            
            If RadioButton1.Checked = True Then
                getImageAscii(resizeImg(New Bitmap(strFileNameAndPath), 4.5, 4.5))
            Else
                getTextAscii(resizeImg(New Bitmap(strFileNameAndPath), 4.5, 9))
            End If
        End If
    End Sub
End Class

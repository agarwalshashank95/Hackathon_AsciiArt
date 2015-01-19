Imports System.Drawing.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Text
Module AsciiArt

    Dim f As Font


    Dim AsciiChars() As String = {"&nbsp;", ".", "-", ":", "*", "+", "=", "%", "@", "#", "#"}
    Dim txt As String

    Dim r, g, b As Integer

    Dim lum As Integer

    Dim temp As Bitmap

    Public strFileNameAndPath As String

    Public Sub init()
        txt = " .`-_':,;^=+/\|)\\<>)ivxclrs{*}I?!][1taeo7zOQjLunTJCwfy325Fp6mqSghVd4EgXPGZbYkOA&8U$@KHDBWNMR%#"
        f = New Font("Comic Sans MS", 10, FontStyle.Regular)
    End Sub

    Public Function getImageAscii(ByVal img As Bitmap) As Bitmap


        temp = New Bitmap(9 * img.Width, 9 * img.Height)

        Dim px, py As Integer
        px = 0
        py = 0


        Dim gr As Graphics = Graphics.FromImage(temp)
        gr.FillRectangle(Brushes.Black, 0, 0, temp.Width, temp.Height)

        Dim data As BitmapBytesRGB24
        data = New BitmapBytesRGB24(img)
        data.LockBitmap()

        Dim pos As Integer
        pos = 0

        Dim c As Color

        Dim brush As SolidBrush

        For py = 0 To img.Height - 1
            For px = 0 To img.Width - 1

                pos = (py * data.RowSizeBytes) + (px * 3)
                b = data.ImageBytes(pos)
                g = data.ImageBytes(pos + 1)
                r = data.ImageBytes(pos + 2)
                lum = (r + g + b) / 3
                c = Color.FromArgb(r, g, b)
                brush = New SolidBrush(c)
                gr.DrawString(txt.Substring(((lum * txt.Length) >> 8), 1), f, brush, 9 * px, 9 * py)
            Next px
        Next py

        data.UnlockBitmap()
        gr.Dispose()

        temp.Save(strFileNameAndPath + "ascii.jpg")
        defaultOpenFile(strFileNameAndPath + "ascii.jpg")
       
    End Function
    Public Function getTextAscii(ByVal img As Bitmap) As Bitmap

        Dim line As StringBuilder = New StringBuilder()

        Dim c As String
        c = Chr(34)

        Dim px, py As Integer
        px = 0
        py = 0

        Dim writer As StreamWriter

        If Form1.RadioButton2.Checked = True Then
            writer = New StreamWriter(strFileNameAndPath + "ascii.htm")
        Else
            writer = New StreamWriter(strFileNameAndPath + "asciicolour.htm")
        End If

        writer.WriteLine("<html>")
        writer.WriteLine("<head>")
        writer.WriteLine("<title>ASCII Art Generator IEM Hackathon</title>")
        writer.WriteLine("<style>")

        If Form1.RadioButton2.Checked = True Then
            writer.WriteLine("BODY, TD {font-family:monospace, Courier; font-size:8px; color:#000000; background-color:#FFFFFF}")
        Else
            writer.WriteLine("BODY, TD {font-family:monospace, Courier; font-size:8px; color:#000000; background-color:#000000}")
        End If

        writer.WriteLine("</style>")
        writer.WriteLine("</head>")
        writer.WriteLine("<table border = " + c + "0" + c + ">")
        writer.WriteLine("<tr>")
        writer.WriteLine("<td nowrap><font face=" + c + "monospace, Courier" + c + " size = " + c + "1" + c + " color = " + c + "000000" + c + ">")

        writer.WriteLine("")

        Dim data As BitmapBytesRGB24
        data = New BitmapBytesRGB24(img)
        data.LockBitmap()

        Dim pos As Integer
        pos = 0



        For py = 0 To img.Height - 1
            line = New StringBuilder
            For px = 0 To img.Width - 1

                pos = (py * data.RowSizeBytes) + (px * 3)
                b = data.ImageBytes(pos)
                g = data.ImageBytes(pos + 1)
                r = data.ImageBytes(pos + 2)
                lum = (r + g + b) / 3
               
                If Form1.RadioButton2.Checked = True Then
                    line.Append(AsciiChars(10 - (lum * 10) / 255))
                Else
                    line.Append("<font color=" + c + RGB2HTMLColor(r, g, b) + c + ">" + AsciiChars((lum * 10) / 255) + "</font>")
                End If
            Next px
            writer.WriteLine(line.ToString + "<br>")
        Next py

        data.UnlockBitmap()


        writer.WriteLine("</font>")
        writer.WriteLine("</td>")
        writer.WriteLine("</tr>")
        writer.WriteLine("</table>")
        writer.WriteLine("</body>")
        writer.WriteLine("</html>")

        
        writer.Close()

        If Form1.RadioButton2.Checked = True Then
            defaultOpenFile(strFileNameAndPath + "ascii.htm")
        Else
            defaultOpenFile(strFileNameAndPath + "asciicolour.htm")
        End If



    End Function



    Public Function resizeImg(ByVal img As Bitmap, ByVal kx As Double, ByVal ky As Double) As Bitmap
        Dim newHeight, newWidth As Integer
        newHeight = img.Height / ky
        newWidth = img.Width / kx
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.NearestNeighbor
            graphicsHandle.DrawImage(img, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

    Public Function OpenFile() As String
        Dim strFileName = ""
        Dim fileDialogBox As New OpenFileDialog()

        fileDialogBox.Filter = "All Files|*.*"

        fileDialogBox.FilterIndex = 1

        fileDialogBox.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)

        'Check to see if the user clicked the open button
        If (fileDialogBox.ShowDialog() = DialogResult.OK) Then
            strFileName = fileDialogBox.FileName
        End If

        'return the name of the file
        Return strFileName
    End Function

    Public Sub defaultOpenFile(ByVal path As String)

        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(path)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("File couldnt be found!", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Function RGB2HTMLColor(ByVal R As Byte, ByVal G As Byte, ByVal B As Byte) As String

        Dim HexR, HexB, HexG As Object

        'R
        HexR = Hex(R)
        If Len(HexR) < 2 Then HexR = "0" & HexR

        'Get Green Hex
        HexG = Hex(G)
        If Len(HexG) < 2 Then HexG = "0" & HexG

        HexB = Hex(B)
        If Len(HexB) < 2 Then HexB = "0" & HexB


        RGB2HTMLColor = "#" & HexR & HexG & HexB
    End Function


End Module

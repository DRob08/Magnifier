Imports System.Drawing.Drawing2D

Public Class Form1
    Dim rect As Rectangle
    Private myMagnifier As SpiritMagnifier.frmMagnifier
    ' Public Shared SavedImage As Image

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
    End Sub


    ''' <summary>
    ''' Function to check if Form is open
    ''' </summary>
    ''' <param name="name">Name - Name of the form</param>
    ''' <returns> True - form is open
    '''           False - form is closed</returns>
    ''' <remarks></remarks>
    Private Function CheckFrmOpen(name As String) As Boolean
        Dim fc As FormCollection = Application.OpenForms
        For Each frm As Form In fc
            If frm.Text = name Then
                Return True
            End If
        Next
        Return False
    End Function

    'Private Sub PictureBox1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
    '    If myMagnifier IsNot Nothing Then
    '        If Not CheckFrmOpen(myMagnifier.Text) Then
    '            PictureBox1.Image = SavedImage
    '        End If
    '    End If
    'End Sub



    Private Sub UcMagnifier1_Click_1(sender As System.Object, e As System.EventArgs) Handles UcMagnifier1.Click
        'Make sure the picturebox is not null and image is loaded
        If PictureBox1 IsNot Nothing And PictureBox1.Image IsNot Nothing Then

            'Check if Magnifier you should open or close
            If myMagnifier Is Nothing OrElse myMagnifier.Text = "" Then

                'create magnifier object send picturebox as parameter
                myMagnifier = New SpiritMagnifier.frmMagnifier(Me.PictureBox1)

                'show Magnifier form
                myMagnifier.Show()

            ElseIf CheckFrmOpen(myMagnifier.Text) Then

                myMagnifier.WindowState = FormWindowState.Normal
                'close Magnifier
                myMagnifier.Close()
            End If
        End If
    End Sub

    'Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    '    Dim testImage As Image = Image.FromFile("c:\70854915_1.fob")


    '    testImage = ResizeImage(testImage, New Size(testImage.Width / 3, testImage.Height / 3), InterpolationMode.High)

    '    PictureBox1.Image = testImage
    'End Sub


    ''' <summary>
    ''' Author: MiamiDade/Darwin
    ''' Description: Function to resize Original Image and draw
    ''' image with new dimensions
    ''' </summary>
    ''' <param name="input">Image</param>
    ''' <param name="newSize">New size for Original Image</param>
    ''' <param name="interpolation">Using Interpolation</param>
    ''' <returns>a New Image with different size</returns>
    ''' <remarks></remarks>
    Function ResizeImage(input As Image, newSize As Size, interpolation As InterpolationMode) As Image
        Dim newImg As New Bitmap(newSize.Width, newSize.Height)
        'newImg = New Bitmap(newSize.Width, newSize.Height)

        Using g As Graphics = Graphics.FromImage(newImg)
            'Prevent using images internal thumbnail
            input.RotateFlip(RotateFlipType.Rotate180FlipNone)
            input.RotateFlip(RotateFlipType.Rotate180FlipNone)

            'Interpolation
            g.InterpolationMode = interpolation

            'Draw the image with the new dimensions
            g.DrawImage(input, 0, 0, newSize.Width, newSize.Height)
            'g.DrawImage(input, 0, 0)

        End Using

        Return DirectCast(newImg, Image)
    End Function
End Class

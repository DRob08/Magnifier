Imports System.Drawing.Drawing2D

Public Class frmMagnifier
    Private selectedArea As Rectangle = Nothing
    Private loadedImage As Image = Nothing
    Private thumbnail As Image = Nothing
    Private selectionColor As Color = Nothing
    Private pboSmall As PictureBox = Nothing
    Private isClosing As Boolean = False
    Public Shared savedImage As Image


    Private downStart As DateTime
    Private downEnd As DateTime

    ' The offset from the mouse's down position
    ' and the picture's upper left corner.
    Private OffsetX, OffsetY As Integer
    Private Dragging As Boolean = False
    Private isMoving As Boolean = False

    Private firstLoad As Boolean = True

    Dim WithEvents timer As New Timer
    Dim milliseconds As Integer


    Private inDrawing As Boolean = False
    Private xStart As Integer = 0
    Private yStart As Integer = 0
    Private xEnd As Integer = 0
    Private yEnd As Integer = 0


    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Constructor that creates Magnifier object 
    ''' when passed a picturebox object
    ''' </summary>
    ''' <param name="pboInrtx">Picturebox that contains image to magnify </param>
    ''' <remarks>Second Constructor to Magnify Image passed through the Picturebox</remarks>
    Public Sub New(ByRef pboInrtx As PictureBox)
        InitializeComponent()

        If pboInrtx IsNot Nothing And pboInrtx.Image IsNot Nothing Then
            Try
                'assign object for manipulation
                pboSmall = pboInrtx

                'Get Original Image and edit
                loadedImage = pboInrtx.Image

                'Save Original Image
                savedImage = loadedImage

                'Initialize selected area Rectangle
                selectedArea = New Rectangle(0, 0, pboSmall.Width - 40, 100)

                'Load User INI Configurations
                LoadConfiguration(selectedArea, Me, firstLoad)

                'add handlers for object's events
                AddHandler pboSmall.Click, AddressOf PictureBox_Click
                AddHandler pboSmall.Paint, AddressOf PictureBox_Paint
                AddHandler pboSmall.MouseDown, AddressOf pictureBox_MouseDown
                AddHandler pboSmall.MouseMove, AddressOf pictureBox_MouseMove
                AddHandler pboSmall.MouseUp, AddressOf pictureBox_MouseUp

                resizePictureArea()

                picZoom.Invalidate()

                'Update zoom
                UpdateZoom(loadedImage, selectedArea, thumbnail, tZoom.Value, picZoom)

            Catch ex As Exception
                MsgBox("Error When Loading Magnifier - Please Restart Spirit Magnifier " + ex.ToString, vbCritical, "Magnifier Error")
            End Try
           
        End If
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Deafult Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Create a thumbnail image maintaining aspect ratio
    ''' use low interpolation
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub resizePictureArea()
        Try
            thumbnail = ResizeImage(loadedImage, New Size(pboSmall.Width, pboSmall.Height), InterpolationMode.High)
            pboSmall.Invalidate()
        Catch ex As Exception
            MsgBox("Error When Resizing Magnified Area - Please Restart Spirit Magnifier " + ex.ToString, vbCritical, "Magnifier Error")
        End Try

    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description:  Update the selected area when the user clicks on PIctureBox
    '''  containing the image
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PictureBox_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        downEnd = Now

        Dim duration As TimeSpan = downEnd.Subtract(downStart)
        milliseconds = duration.TotalMilliseconds

        If milliseconds <= 399 And Not CheckSelecArea(selectedArea) Then
            Dim mouseLoc As Point = pboSmall.PointToClient(Cursor.Position)
            Dragging = False
            inDrawing = False
            'Calculate Bound picturebox Limits
            Dim RightLimit As Single = pboSmall.Width - selectedArea.Width / tZoom.Value
            Dim BottomLimit As Single = pboSmall.Height - selectedArea.Height / tZoom.Value

            selectedArea.X = mouseLoc.X - ((selectedArea.Width / tZoom.Value) / 2)
            selectedArea.Y = mouseLoc.Y - ((selectedArea.Height / tZoom.Value) / 2)

            If selectedArea.Left < 0 Then
                selectedArea.X = 0
            ElseIf selectedArea.X > RightLimit Then
                selectedArea.X = RightLimit - 1
            End If

            If selectedArea.Top < 0 Then
                selectedArea.Y = 0
            ElseIf selectedArea.Y > BottomLimit Then
                selectedArea.Y = BottomLimit - 1
            End If

        End If
   
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description:
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pictureBox_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try
            'Set Variables to False
            Dragging = False
            inDrawing = False

            'Set focus back to form in case is hidden
            Me.Focus()

            'Check to resize rectangle to default size if needed
            CheckSelecArea(selectedArea)

            'Redeaw and repaint
            pboSmall.Invalidate()
            UpdateZoom(loadedImage, selectedArea, thumbnail, tZoom.Value, picZoom)

            'Call Garbage Collector to reduce Memory Process/Memory Issues Problem
            GC.Collect()
        Catch ex As Exception
            MsgBox("Magnifier Error Detected - Please Restart Spirit Magnifier " + ex.ToString, vbCritical, "Magnifier Error")
        End Try
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pictureBox_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        ' We're not dragging the image. See if we're over
        ' it.
        ' Dim new_cursor As Cursor = Cursors.Default

        If (selectedArea.Contains(e.X, e.Y)) And CheckFrmOpen(Me.Text) Then
            pboSmall.Cursor = Cursors.SizeAll
        Else
            pboSmall.Cursor = Cursors.Default
        End If

        ' See if we're dragging.
        If (Dragging) Then
            isMoving = True
            ' We're dragging the image. Move it.
            selectedArea.X = e.X + OffsetX
            selectedArea.Y = e.Y + OffsetY

            SetRectBounds()

        ElseIf milliseconds > 400 Then

            If inDrawing = False Then Exit Sub

            Dim mouseLoc As Point = pboSmall.PointToClient(Cursor.Position)

            xEnd = mouseLoc.X
            yEnd = mouseLoc.Y

            If xStart < xEnd Then
                selectedArea.X = xStart
            Else
                selectedArea.X = xEnd
            End If
            If yStart < yEnd Then
                selectedArea.Y = yStart
            Else
                selectedArea.Y = yEnd
            End If

            'Call Function to make sure the rectangle stays inside picturebox
            SetRectBounds()

            selectedArea.Width = Math.Abs(xEnd - xStart)
            selectedArea.Height = Math.Abs(yEnd - yStart)

            frmMagnifier_Resize_1(sender, e)

        End If


    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub pictureBox_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        downStart = Now
        ' See if we're over the picture.
        ' If (PointIsOverPicture(e.X, e.Y)) Then
        If (selectedArea.Contains(e.X, e.Y)) Then
            ' Start dragging.
            Dragging = True

            ' Record the offset from the mouse to the picture's
            ' origin.
            OffsetX = selectedArea.X - e.X
            OffsetY = selectedArea.Y - e.Y
        ElseIf milliseconds > 400 Then

            inDrawing = True
            xStart = e.X
            yStart = e.Y
        End If
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetRectBounds()
        'Calculate Bound picturebox Limits
        Dim RightLimit As Single = pboSmall.Width - selectedArea.Width / tZoom.Value
        Dim BottomLimit As Single = pboSmall.Height - selectedArea.Height / tZoom.Value

        If selectedArea.Left < 0 Then
            selectedArea.X = 0
        ElseIf selectedArea.X > RightLimit Then
            selectedArea.X = RightLimit - 1
        End If

        If selectedArea.Top < 0 Then
            selectedArea.Y = 0
        ElseIf selectedArea.Y > BottomLimit Then
            selectedArea.Y = BottomLimit - 1
        End If
    End Sub
    Private Sub EggTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timer.Tick
        milliseconds += 1
    End Sub

    '' Return true if the point is over the picture's current
    '' location.
    'Private Function PointIsOverPicture(ByVal x As Integer, ByVal y As Integer) As Boolean
    '    Try
    '        'CHECK THIS IF I STILL NEED IT AFTER
    '        newImg = New Bitmap(pboSmall.Image)
    '        ' See if it's over the picture's bounding rectangle.
    '        If ((x < selectedArea.Left) OrElse _
    '            (y < selectedArea.Top) OrElse _
    '            (x >= selectedArea.Right) _
    '            OrElse (y >= selectedArea.Bottom)) _
    '                Then Return False

    '        'See if the point is above a non-transparent pixel.
    '        Dim i As Integer = x - selectedArea.X
    '        Dim j As Integer = y - selectedArea.Y
    '        Return (newImg.GetPixel(i, j).A > 0)
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    Finally
    '        newImg.Dispose()
    '    End Try
    '    Return False
    'End Function

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Paint event only while this form is open
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PictureBox_Paint(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If Not isClosing Then
                If loadedImage IsNot Nothing Then
                    AdjustArea(selectedArea, tZoom.Value, pboSmall, thumbnail)
                End If
            End If
            GC.Collect()
        Catch ex As Exception
            MsgBox("Magnifier Error While adjusting area - Please Restart Spirit Magnifier " + ex.ToString, vbCritical, "Magnifier Error")
        End Try
       
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Form closing Event- returns original image to PictureBox when 
    ''' Closing this form.
    ''' Isclosing = TRUE when form is closed - this boolean 
    ''' is used in the paint event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMagnifier_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'save X,Y Coordinates for Magnifier
        SaveConfiguration(selectedArea, Me, tZoom.Value)

        'Set boolean to True - used in Paint event
        isClosing = True

        'return original saved image to picturebox
        pboSmall.Image = savedImage

        GC.Collect()

        'Dispose Object
        Me.Dispose()
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Zoom Factor scrollbar that updates the zoom as 
    ''' users scrolls up or down
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tZoom_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tZoom.Scroll
        'Redraw.
        pboSmall.Invalidate()

        'UpdateZoom()
        UpdateZoom(loadedImage, selectedArea, thumbnail, tZoom.Value, picZoom)
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMagnifier_Resize_1(sender As System.Object, e As System.EventArgs) Handles MyBase.Resize
        Try
            If Not firstLoad Then
                If selectedArea.Width > 0 Then
                    If inDrawing Then
                        Me.Width = selectedArea.Width * 2
                        Me.Height = selectedArea.Height * 2

                    Else
                        selectedArea.Width = Me.Width / 2
                        selectedArea.Height = Me.Height / 2

                        SetRectBounds()

                        'Redraw.
                        pboSmall.Invalidate()

                        'UpdateZoom()
                        UpdateZoom(loadedImage, selectedArea, thumbnail, tZoom.Value, picZoom)
                    End If

                End If
            End If
        Catch ex As Exception
            MsgBox("Magnifier Error While Resizing Window - Please Restart Spirit Magnifier " + ex.ToString, vbCritical, "Magnifier Error")
        End Try

       
    End Sub
    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description:
    ''' </summary>
    ''' <param name="inRectSel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckSelecArea(ByVal inRectSel As Rectangle) As Boolean
        If inRectSel.Width <= 50 Or inRectSel.Height <= 20 Then
            setMinRect()
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub setMinRect()
        selectedArea.Width = Me.Width / 2
        selectedArea.Height = Me.Height / 2
    End Sub

End Class
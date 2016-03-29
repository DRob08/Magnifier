Imports System.Text
Imports System.Drawing.Drawing2D
Imports System.Runtime

Public Module MagnifierInit
    ''' <summary>
    ''' This object is found in spiritInit.This object was created here for
    ''' tetsing purposes
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Magnifier_Window_Position
        Public MagnifierOn As String
        Public MagnifierWindowTop As String
        Public MagnifierWindowLeft As String
        Public MagnifierWindowWidth As String
        Public MagnifierWindowHeight As String
        Public MagnifierTop As String
        Public MagnifierLeft As String
        Public MagnifierHeight As String
        Public MagnifierWidth As String
        Public MagnifierZoom As String
    End Class


    Private selectionColor As Color
    Private MagnifierPos As Magnifier_Window_Position = New Magnifier_Window_Position
    Public tempString As StringBuilder
    Public appPath As String = "C:\Spirit\bin\"
    Dim newImgRtn As Bitmap = Nothing
    Dim newBmp As Bitmap = Nothing



#Region "Methods"
    ''' <summary>
    '''  
    ''' 
    ''' </summary>
    ''' <param name="lpSectionName"></param>
    ''' <param name="lpKeyName"></param>
    ''' <param name="lpDefault"></param>
    ''' <param name="lpReturnedString"></param>
    ''' <param name="nSize"></param>
    ''' <param name="lpFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpSectionName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    ''' <summary>
    ''' This function write to Spirit.INI file
    ''' 
    ''' </summary>
    ''' <param name="lpApplicationName"></param>
    ''' <param name="lpKeyName"></param>
    ''' <param name="lpString"></param>
    ''' <param name="lpFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Declare Function WritePrivateProfileString Lib "Kernel32" Alias "WritePrivateProfileStringA" _
    (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long

    ''' <summary>
    ''' Author: Darwin Robinson
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

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Draws the selection of rectangle on Image
    ''' </summary>
    ''' <param name="input">Image</param>
    ''' <param name="selectedArea">User selected area</param>
    ''' <param name="selectColor">Color used for the selected area rectangle</param>
    ''' <returns>Image-Mark the selected area rectable on Image</returns>
    ''' <remarks></remarks>
    Function MarkImage(input As Image, selectedArea As Rectangle, selectColor As Color) As Image
        'Dim newImg As New Bitmap(input.Width, input.Height)
        newImgRtn = New Bitmap(input.Width, input.Height)
        Using g As Graphics = Graphics.FromImage(newImgRtn)
            'Prevent using images internal thumbnail
            input.RotateFlip(RotateFlipType.Rotate180FlipNone)
            input.RotateFlip(RotateFlipType.Rotate180FlipNone)

            g.DrawImage(input, 0, 0)

            'Draw the selection rect
            Using p As New Pen(Brushes.Red, 3)
                g.DrawRectangle(p, selectedArea)
            End Using
        End Using

        Return DirectCast(newImgRtn, Image)
    End Function

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Stretches out selected image
    ''' </summary>
    ''' <param name="input">Imge to zoom in</param>
    ''' <param name="zoomArea">Magnified area</param>
    ''' <param name="sourceArea">Original selected area</param>
    ''' <returns>returns an image zoom in</returns>
    ''' <remarks></remarks>
    Function ZoomImage(input As Image, zoomArea As Rectangle, sourceArea As Rectangle) As Image
        'Dim newBmp As New Bitmap(sourceArea.Width, sourceArea.Height)
        newBmp = New Bitmap(sourceArea.Width, sourceArea.Height)
        Using g As Graphics = Graphics.FromImage(newBmp)
            'high interpolation
            g.InterpolationMode = InterpolationMode.High

            g.DrawImage(input, sourceArea, zoomArea, GraphicsUnit.Pixel)
        End Using

        Return newBmp

    End Function

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 'Map the area selected in the thumbail to the actual image size
    ''' </summary>
    ''' <remarks></remarks>
    Sub UpdateZoom(ByRef inLoadImage As Image, ByRef inRecArea As Rectangle, ByRef inThumb As Image, value As Integer, ByRef pboInZoom As PictureBox)
        If inLoadImage IsNot Nothing And inThumb IsNot Nothing Then
            Dim zoomArea As New Rectangle()
            zoomArea.X = inRecArea.X * inLoadImage.Width / inThumb.Width
            zoomArea.Y = inRecArea.Y * inLoadImage.Height / inThumb.Height
            zoomArea.Width = inRecArea.Width * inLoadImage.Width / inThumb.Width
            zoomArea.Height = inRecArea.Height * inLoadImage.Height / inThumb.Height

            'Adjust the selected area to the current zoom value
            zoomArea.Width /= value
            zoomArea.Height /= value

            pboInZoom.Image = ZoomImage(inLoadImage, zoomArea, pboInZoom.ClientRectangle)
            pboInZoom.Refresh()
          
        End If
    End Sub
    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <param name="sourceRec"></param>
    ''' <param name="value"></param>
    ''' <param name="pboIn"></param>
    ''' <param name="thumbIn"></param>
    ''' <remarks></remarks>
    Sub AdjustArea(ByRef sourceRec As Rectangle, value As Integer, ByRef pboIn As PictureBox, ByRef thumbIn As Image)
        'Adjust the selected area to reflect the zoom value
        Dim adjustedArea As New Rectangle()
        adjustedArea.X = sourceRec.X
        adjustedArea.Y = sourceRec.Y
        adjustedArea.Width = sourceRec.Width / value
        adjustedArea.Height = sourceRec.Height / value

        'Draw the selected area on the thumbnail
        pboIn.Image = MarkImage(thumbIn, adjustedArea, selectionColor)

    End Sub
    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: 
    ''' </summary>
    ''' <param name="sourceRec"></param>
    ''' <param name="inFrm"></param>
    ''' <param name="inFirstLoad"></param>
    ''' <remarks></remarks>
    Sub LoadConfiguration(ByRef sourceRec As Rectangle, ByRef inFrm As Form, ByRef inFirstLoad As Boolean)
        ReadSpiritIni()
        If MagnifierPos IsNot Nothing And inFrm IsNot Nothing Then

            Try
                'Rectangle X,Y Position
                sourceRec.X = CInt(MagnifierPos.MagnifierLeft)
                sourceRec.Y = CInt(MagnifierPos.MagnifierTop)
                sourceRec.Width = If(CInt(MagnifierPos.MagnifierWidth) > 50, CInt(MagnifierPos.MagnifierWidth), 100)
                sourceRec.Height = If(CInt(MagnifierPos.MagnifierHeight) > 20, CInt(MagnifierPos.MagnifierHeight), 100)

                'Form X,Y,W,H Position
                inFrm.Left = If(MagnifierPos.MagnifierWindowLeft = "", inFrm.Left, CInt(MagnifierPos.MagnifierWindowLeft))
                inFrm.Top = If(MagnifierPos.MagnifierWindowTop = "", inFrm.Top, CInt(MagnifierPos.MagnifierWindowTop))
                inFrm.Width = If(MagnifierPos.MagnifierWindowWidth = "", inFrm.Width, CInt(MagnifierPos.MagnifierWindowWidth))
                inFrm.Height = If(MagnifierPos.MagnifierWindowHeight = "", inFrm.Height, CInt(MagnifierPos.MagnifierWindowHeight))

                'This boolean is used in the  Form Resize Event
                inFirstLoad = False
            Catch ex As Exception

                sourceRec.X = sourceRec.X
                sourceRec.Y = sourceRec.Y
                sourceRec.Width = 100
                sourceRec.Height = 100

                'Form X,Y,W,H Position
                inFrm.Left = inFrm.Left
                inFrm.Top = inFrm.Top
                inFrm.Width = inFrm.Width
                inFrm.Height = inFrm.Height

                'This boolean is used in the  Form Resize Event
                inFirstLoad = False

            End Try

         
        End If
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Helper function to read from spirit.ini
    ''' this is implemented in SpiritInit so this MagnifierPos object should come
    ''' from that class
    ''' </summary>
    ''' <remarks></remarks>
    Sub ReadSpiritIni()
        Try
            If MagnifierPos Is Nothing Then
                MagnifierPos = New Magnifier_Window_Position
            End If

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierWindowTop", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierWindowTop = tempString.ToString.Trim(" ")

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierWindowLeft", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierWindowLeft = tempString.ToString.Trim(" ")

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierWindowWidth", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierWindowWidth = tempString.ToString.Trim(" ")


            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierWindowHeight", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierWindowHeight = tempString.ToString.Trim(" ")


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierTop", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierTop = tempString.ToString.Trim(" ")

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierLeft", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierLeft = tempString.ToString.Trim(" ")

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierHeight", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierHeight = tempString.ToString.Trim(" ")

            tempString = New StringBuilder(255)
            GetPrivateProfileString("Magnifier Window Position", "MagnifierWidth", " ", tempString, 255, appPath + "spirit.ini")
            MagnifierPos.MagnifierWidth = tempString.ToString.Trim(" ")


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Reads spirit.ini to bring zoomvalue
    ''' this is a new field that needs to be added to Magnifier_Window_Position class
    ''' as well as to the spirit.ini
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConfigZoomValue() As Integer
        tempString = New StringBuilder(255)
        GetPrivateProfileString("Magnifier Window Position", "MagnifierZoom", " ", tempString, 255, appPath + "spirit.ini")
        MagnifierPos.MagnifierZoom = tempString.ToString.Trim(" ")

        Return CInt(MagnifierPos.MagnifierZoom)
    End Function

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: This function is implemented in spiritIni this was done here for testing purposes
    ''' </summary>
    ''' <param name="sourceRec"></param>
    ''' <param name="inFrm"></param>
    ''' <param name="zValue"></param>
    ''' <remarks></remarks>
    Sub SaveConfiguration(ByVal sourceRec As Rectangle, ByVal inFrm As Form, zValue As Integer)
        Try

            '''''''''''''''''''''''''''''''''''''''''''RECTANGLE POSITION''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            MagnifierPos.MagnifierTop = Convert.ToString(sourceRec.Location.Y)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierTop", MagnifierPos.MagnifierTop, appPath + "spirit.ini")

            MagnifierPos.MagnifierLeft = Convert.ToString(sourceRec.Location.X)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierLeft", MagnifierPos.MagnifierLeft, appPath + "spirit.ini")

            MagnifierPos.MagnifierHeight = Convert.ToString(sourceRec.Height)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierHeight", MagnifierPos.MagnifierHeight, appPath + "spirit.ini")

            MagnifierPos.MagnifierWidth = Convert.ToString(sourceRec.Width)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierWidth", MagnifierPos.MagnifierWidth, appPath + "spirit.ini")

            '''''''''''''''''''''''''''''''''''''''''''''WINDOW POSITION BELOW'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            MagnifierPos.MagnifierWindowTop = Convert.ToString(inFrm.Location.Y)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierWindowTop", MagnifierPos.MagnifierWindowTop, appPath + "spirit.ini")

            MagnifierPos.MagnifierWindowLeft = Convert.ToString(inFrm.Location.X)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierWindowLeft", MagnifierPos.MagnifierWindowLeft, appPath + "spirit.ini")

            MagnifierPos.MagnifierWindowWidth = Convert.ToString(inFrm.Width)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierWindowWidth", MagnifierPos.MagnifierWindowWidth, appPath + "spirit.ini")

            MagnifierPos.MagnifierWindowHeight = Convert.ToString(inFrm.Height)
            WritePrivateProfileString("Magnifier Window Position", "MagnifierWindowHeight", MagnifierPos.MagnifierWindowHeight, appPath + "spirit.ini")

            'Dispose bitmap objects
            newImgRtn.Dispose()
            newBmp.Dispose()

        Catch ex As Exception
            MsgBox("Error Saving Configuration - Please Contact Spirit Support " + ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Author: Darwin Robinson
    ''' Description: Function to check if Form is open
    ''' </summary>
    ''' <param name="name">Name - Name of the form</param>
    ''' <returns> True - form is open
    '''           False - form is closed</returns>
    ''' <remarks></remarks>
    Public Function CheckFrmOpen(name As String) As Boolean
        Dim fc As FormCollection = Application.OpenForms
        For Each frm As Form In fc
            If frm.Text = name Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region


End Module



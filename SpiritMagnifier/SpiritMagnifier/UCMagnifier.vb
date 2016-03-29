Public Class UCMagnifier
    ''' <summary>
    ''' Default Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        InitializeComponent()
        WireAllControls(Me)
    End Sub

    ''' <summary>
    ''' Helper function to wire all control
    ''' and let the user control when an event occurred
    ''' </summary>
    ''' <param name="cont"></param>
    ''' <remarks></remarks>
    Private Sub WireAllControls(cont As Control)
        For Each ctl As Control In cont.Controls
            AddHandler ctl.Click, AddressOf ctl_Click
            If ctl.HasChildren Then
                WireAllControls(ctl)
            End If
        Next
    End Sub

    ''' <summary>
    '''  the click event 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctl_Click(sender As Object, e As EventArgs)
        Me.InvokeOnClick(Me, EventArgs.Empty)
    End Sub

End Class

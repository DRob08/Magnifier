<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCMagnifier
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnMagnifier = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnMagnifier
        '
        Me.btnMagnifier.AutoSize = True
        Me.btnMagnifier.BackColor = System.Drawing.Color.White
        Me.btnMagnifier.BackgroundImage = Global.SpiritMagnifier.My.Resources.Resources.Magnifying_Glass
        Me.btnMagnifier.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMagnifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMagnifier.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMagnifier.Location = New System.Drawing.Point(0, 0)
        Me.btnMagnifier.Name = "btnMagnifier"
        Me.btnMagnifier.Size = New System.Drawing.Size(78, 75)
        Me.btnMagnifier.TabIndex = 0
        Me.btnMagnifier.Text = "Magnifier"
        Me.btnMagnifier.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnMagnifier.UseVisualStyleBackColor = False
        '
        'UCMagnifier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.btnMagnifier)
        Me.Name = "UCMagnifier"
        Me.Size = New System.Drawing.Size(86, 81)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnMagnifier As System.Windows.Forms.Button

End Class

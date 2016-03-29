<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMagnifier
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMagnifier))
        Me.tZoom = New System.Windows.Forms.TrackBar()
        Me.picZoom = New System.Windows.Forms.PictureBox()
        CType(Me.tZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tZoom
        '
        Me.tZoom.Location = New System.Drawing.Point(120, 0)
        Me.tZoom.Maximum = 3
        Me.tZoom.Minimum = 1
        Me.tZoom.Name = "tZoom"
        Me.tZoom.Size = New System.Drawing.Size(187, 45)
        Me.tZoom.TabIndex = 2
        Me.tZoom.Value = 1
        Me.tZoom.Visible = False
        '
        'picZoom
        '
        Me.picZoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.picZoom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picZoom.Location = New System.Drawing.Point(0, 0)
        Me.picZoom.Name = "picZoom"
        Me.picZoom.Size = New System.Drawing.Size(440, 457)
        Me.picZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picZoom.TabIndex = 0
        Me.picZoom.TabStop = False
        '
        'frmMagnifier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 457)
        Me.Controls.Add(Me.tZoom)
        Me.Controls.Add(Me.picZoom)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(385, 274)
        Me.Name = "frmMagnifier"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "SPIRIT.NET - Magnifier"
        CType(Me.tZoom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picZoom As System.Windows.Forms.PictureBox
    Friend WithEvents tZoom As System.Windows.Forms.TrackBar
End Class

Imports Microsoft.VisualBasic.ComponentModel.Collection

Public Class CultureMedium

    ReadOnly cells As Cell()()()

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="shape">
    ''' should be 3 elements for: width, height, long
    ''' </param>
    Sub New(shape As Integer())
        cells = RectangularArray.Cubic(Of Cell)(shape(0), shape(1), shape(2))
    End Sub
End Class

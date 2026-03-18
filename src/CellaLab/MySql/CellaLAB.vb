Imports Oracle.LinuxCompatibility.MySQL.Uri

Public Class CellaLAB : Inherits cellaLab_model.db_cellaLab

    Public Sub New(mysqli As ConnectionUri)
        MyBase.New(mysqli)
    End Sub
End Class

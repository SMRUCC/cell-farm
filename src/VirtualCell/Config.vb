Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions

Public Class Config

    ''' <summary>
    ''' the file path to the model files, usually be the relative path to the config file
    ''' </summary>
    ''' <returns></returns>
    Public Property models As String()
    Public Property mapping As Definition
    Public Property kinetics As FluxBaseline

End Class

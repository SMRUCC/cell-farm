Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions

Public Class Config

    ''' <summary>
    ''' the file path to the model files, usually be the relative path to this config file
    ''' </summary>
    ''' <returns></returns>
    Public Property models As String()
    ''' <summary>
    ''' some specific biological process in the runtime compiler required some metabolite 
    ''' for links, example as ATP, O2, H2O. 
    ''' this config value provides the compound id mapping to such special metabolites
    ''' </summary>
    ''' <returns></returns>
    Public Property mapping As Definition
    ''' <summary>
    ''' parameter for configs the biological process kinetic behaviour
    ''' </summary>
    ''' <returns></returns>
    Public Property kinetics As FluxBaseline

End Class

Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions

Public Class Config

    ''' <summary>
    ''' the file path to the model files, usually be the relative path to this config file.
    ''' 
    ''' multiple model file could be listed at here for run virtual cell simulation for synthetic microbiota.
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

    Public Property iterations As Integer = 1000
    Public Property resolution As Integer = 2000
    Public Property tqdm_progress As Boolean = True
    Public Property debug As Boolean = False

    ''' <summary>
    ''' a list of gene ids for knockout, example as ["b0002","b0008"]
    ''' </summary>
    ''' <returns></returns>
    Public Property knockouts As String()

    ''' <summary>
    ''' the cell copy number, which is used for reset the gene template number
    ''' </summary>
    ''' <returns></returns>
    Public Property copy_number As Dictionary(Of String, Integer)

End Class

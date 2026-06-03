Imports System.Runtime.CompilerServices
Imports SMRUCC.genomics.GCModeller.Assembly.GCMarkupLanguage.v2

Module SaveModel

    ''' <summary>
    ''' Save virtual cell model into the model library table
    ''' </summary>
    ''' <param name="cella"></param>
    ''' <param name="model"></param>
    ''' <remarks>
    ''' 这个函数只是把模型的摘要数据保存进入数据库中，并非将完整的模型数据和所有的参数细节都保存进入数据库中
    ''' </remarks>
    <Extension>
    Public Sub SaveModelData(cella As CellaLAB, model As VirtualCell)

    End Sub
End Module

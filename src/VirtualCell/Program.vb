Imports System.ComponentModel
Imports System.IO
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.json
Imports SMRUCC.genomics.GCModeller.Assembly.GCMarkupLanguage.v2
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Engine
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.ModelLoader
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Dynamics.Core
Imports SMRUCC.genomics.GCModeller.ModellingEngine.IO
Imports SMRUCC.genomics.GCModeller.ModellingEngine.IO.Raw
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Model.Cellular

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine, executeEmpty:=AddressOf getConfigTemplate)
    End Function

    Private Function getConfigTemplate() As Integer
        Dim template As New Config With {
            .models = {"model.xml"},
            .mapping = Definition.MetaCyc({"ADENOSINE", "CYTIDINE", "GUANOSINE", "URIDINE", "THYMIDINE"}, Double.NaN),
            .kinetics = New FluxBaseline,
            .debug = False,
            .knockouts = {"b0002", "b0008"}
        }

        Call Console.WriteLine(template.GetJson(indent:=True, comment:=True))

        Return 0
    End Function

    <ExportAPI("--convert")>
    <Usage("--convert --data <result.vcelldata> [--output <output.vcellPack>]")>
    Public Function ConvertMatrix(data As String, args As CommandLine)
        Dim output As String = args("--output") Or data.ChangeSuffix("vcellPack")
        Dim rawdata As New Reader(data.OpenReadonly)

        Using save As Stream = output.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False),
            matrixPack = New VCellMatrixWriter(save)

            Call rawdata.LoadIndex()
            Call matrixPack.ConvertPackData(rawdata)
        End Using

        Return 0
    End Function

    <ExportAPI("--run")>
    <Description("run the virtual cell simulation.")>
    <Usage("--run <config.json> [--output <result.vcelldata>]")>
    Public Function Run(args As CommandLine) As Integer
        Dim config_file As String = args.Tokens(1)
        Dim output As String = args("--output") Or config_file.ChangeSuffix(".vcelldata")
        Dim config As Config = config_file _
            .ReadAllText _
            .ParseJson _
            .CreateObject(Of Config)
        Dim model As VirtualCell = Nothing
        Dim modelData As CellularModule
        Dim modelList As New List(Of CellularModule)

        If output.ExtensionSuffix <> "vcelldata" Then
            Call $"The output result file '{output}' is not has the '*.vcelldata' extension name!".warning
        End If
        If config.models.IsNullOrEmpty Then
            Call "no virtual cell model was provided for run the experiment!".error
            Return 404
        End If

        Dim cellular_id As New List(Of String)
        Dim symbolNames As New Dictionary(Of String, String)
        Dim modelDataList As New List(Of CellularModule)

        For Each name As String In config.models
            If Not name.FileExists Then
                name = config_file.ParentPath & "/" & name
            End If
            If Not name.FileExists Then
                Call $"missing virtual cell model file: {name}".error
                Return 404
            End If

            model = name.LoadXml(Of VirtualCell)
            modelData = model.CreateModel
            modelList.Add(modelData)
            cellular_id.Add(modelData.CellularEnvironmentName)

            For Each idName As KeyValuePair(Of String, String) In model.GetMetaboliteSymbolNames
                symbolNames(idName.Key) = idName.Value
            Next

            Call modelDataList.Add(modelData)
        Next

        Dim pull = SyntheticMicrobialNetwork.CreateNetwork(modelDataList, config.mapping, config.kinetics)
        Dim engine As New Engine(config.mapping, config.kinetics, cellular_id.ToArray,
                                 config.iterations,
                                 config.resolution,
                                 config.tqdm_progress,
                                 config.debug) With {
            .models = modelList _
                .ToArray,
            .fluxIndex = pull.fluxIndex _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a.Value.ToArray
                              End Function)
        }

        Call engine _
            .SetModel(pull.mass, pull.network) _
            .MakeKnockout(config.knockouts) _
            .SetCultureMedium(config.cultureMedium)

        If Not config.copy_number.IsNullOrEmpty Then

        End If

        Using storage As New StorageDriver(output, engine, graph_debug:=False)
            Call storage.SetSymbolNames(symbolNames)
            Call engine.AttachBiologicalStorage(storage)
            Call engine.MakeNetworkSnapshot(storage.GetStream)

            Return engine.Run()
        End Using
    End Function
End Module

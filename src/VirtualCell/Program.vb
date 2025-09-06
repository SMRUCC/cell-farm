Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.json
Imports SMRUCC.genomics.GCModeller.Assembly.GCMarkupLanguage.v2
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Engine
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.ModelLoader
Imports SMRUCC.genomics.GCModeller.ModellingEngine.Dynamics.Core
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
            .kinetics = New FluxBaseline
        }

        Call Console.WriteLine(template.GetJson(indent:=True, comment:=True))

        Return 0
    End Function

    <ExportAPI("--run")>
    <Description("run the virtual cell simulation.")>
    <Usage("--run <config.json> [--time <default=2500> --resolution 5000 --output <result.dat>]")>
    Public Function Run(args As CommandLine) As Integer
        Dim config_file As String = args.Tokens(1)
        Dim output As String = args("--output") Or config_file.ChangeSuffix(".dat")
        Dim config As Config = config_file _
            .ReadAllText _
            .ParseJson _
            .CreateObject(Of Config)
        Dim model As VirtualCell = Nothing
        Dim massTable As New MassTable
        Dim modelData As CellularModule
        Dim processList As New List(Of Channel)
        Dim cellular_id As New List(Of String)
        Dim modelList As New List(Of CellularModule)

        If config.models.IsNullOrEmpty Then
            Call "no virtual cell model was provided for run the experiment!".error
            Return 404
        End If

        Dim symbolNames As New Dictionary(Of String, String)
        Dim fluxIndex As New Dictionary(Of String, List(Of String))

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
            cellular_id.Add(modelData.CellularEnvironmentName)
            modelList.Add(modelData)
            massTable.SetDefaultCompartmentId(modelData.CellularEnvironmentName)

            Dim loader As New Loader(config.mapping, config.kinetics, massTable:=massTable)

            For Each idName As KeyValuePair(Of String, String) In model.GetMetaboliteSymbolNames
                symbolNames(idName.Key) = idName.Value
            Next

            With loader.CreateEnvironment(modelData)
                Call processList.AddRange(.processes)
            End With

            Dim modelFluxIndex = loader.GetFluxIndex

            For Each part_key As String In modelFluxIndex.Keys
                If Not fluxIndex.ContainsKey(part_key) Then
                    Call fluxIndex.Add(part_key, New List(Of String))
                End If

                Call fluxIndex(part_key).AddRange(modelFluxIndex(part_key))
            Next
        Next

        Dim engine As New Engine(config.mapping, config.kinetics, cellular_id.ToArray,
                                 config.iterations,
                                 config.resolution,
                                 config.tqdm_progress,
                                 config.debug) With {
            .models = modelList _
                .ToArray,
            .fluxIndex = fluxIndex _
                .ToDictionary(Function(a) a.Key,
                              Function(a)
                                  Return a.Value.ToArray
                              End Function)
        }

        Call engine.SetModel(massTable, processList)

        Using storage As New StorageDriver(output, engine, graph_debug:=True)
            Call storage.SetSymbolNames(symbolNames)
            Call engine.AttachBiologicalStorage(storage)
            Call engine.MakeNetworkSnapshot(storage.GetStream)

            Return engine.Run()
        End Using
    End Function
End Module

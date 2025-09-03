Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.json
Imports SMRUCC.genomics.GCModeller.ModellingEngine.BootstrapLoader.Definitions

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


    End Function
End Module

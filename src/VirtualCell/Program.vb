Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("--run")>
    <Usage("--run <config.json> [--time <default=2500> --resolution 5000 --output <result.dat>]")>
    Public Function Run(args As CommandLine) As Integer
        Dim config_file As String = args.Tokens(1)
        Dim output As String = args("--output") Or config_file.ChangeSuffix(".dat")
        Dim config As Config = config_file.LoadJsonFile(Of Config)

    End Function
End Module

Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main(args As String()) As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    <ExportAPI("--run")>
    <Usage("--run <model.xml/model-list.txt> [--extracellular <default=Extracellular> --time <default=2500> --resolution 5000 --output <result.dat>]")>
    Public Function Run(args As CommandLine) As Integer

    End Function
End Module

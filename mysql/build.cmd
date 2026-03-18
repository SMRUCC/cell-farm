REM @echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set R_src="../src/CellaLab\MySql\cellaLab"

%reflector% --reflects /sql ./cad_lab.sql -o %R_src% /namespace cellaLab_model --language visualbasic /split /auto_increment.disable

REM pause
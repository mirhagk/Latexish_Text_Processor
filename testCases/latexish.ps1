$program = '..\Latexish_Text_Processor\ConsoleApplication\bin\debug\ConsoleApplication.exe'


& $program test.texi -e html
& $program bql.texi -e sql
& $program email.texi -e html --math --preprocess
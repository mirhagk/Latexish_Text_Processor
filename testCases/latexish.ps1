$program = '..\Latexish_Text_Processor\ConsoleApplication\bin\debug\ConsoleApplication.exe'


& $program test.texi -e html
& $program bql.texi -e sql
& $program emailComplex.texi -e preprocessed --language --preprocess
& $program email.texi -e html --math --preprocess
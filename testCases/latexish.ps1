$program = '..\Latexish_Text_Processor\Latexish_Text_Processor\bin\debug\Latexish_Text_Processor.exe'


& $program test.texi -e html
& $program bql.texi -e sql
& $program emailComplex.texi -e preprocessed --language --preprocess
& $program email.texi -e html --math --preprocess
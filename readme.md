Latexish Text Processor
=====

A text processor that allows latex-style macros.

Plans
----

The basic functionality has all been created, the next step is to set up the command line interface.

After that the additional commands described below should be added

Commands to add:
---

###Process and Ignore
`[ this section is processed but not included ]`

Using single square brackets will cause the processor to process whatever is contained inside, however it will ignore the result. This is useful for wrapping a section of `\newCommand` macros without their whitespace appearning.

###Raw Insert
`[[ this section is included but not processed ]]`

Using double square brackets will cause the processor to skip processing but include the contents as is. The only command that works in this one is `\]` which is needed to escape square brackets (note it's only needed when there's 2 square brackets in a row, it ignores single square brackets)


###Comment
`[[[ this section is neither processed nor included ]]]`

Using triple square brackets is equivalent to having double square brackets within single square brackets. The inner set prevents the processor from processing the input, and the outer set prevents it from being included in the result. This is a true comment, and forces the system to ignore what's contained inside. 


###Line continuation

A `\` character at the end of the line denotes that the line should wrap to the following line. The processor will ignore the newline as well as any leading whitespace of the next line.

Macro Provider
---

If the built-in system commands aren't enough, and custom macros can't be built from them (or would be too complex) then you can use a macro provider.

Macro providers are a script, or application that can provide macros. The processor calls the script or application and gets a list of supported macros. It will then add these to the list of macros, and when it comes across one of these macros, it will send a request with the parameters to the macro provider. Whatever the macro provider responds with will be included in the output.

Any providers are specified when running the processor, and the processor will start the application and communicate over stdin and stdout. The protocol is described below:

The macro provider will be started, and passed various command line parameters. These parameters can be used to describe the context, but they are currently unused.

The processor will send commands to the macro provider, which must respond to the command, and then wait for a new command. Each command will end with a newline (it uses `stream.WriteLine()`), and the response must end with a newline as well. This means newlines can't be in the message, so if they are required then it must instead send `\n`.

Commands may have parameters, which are separated with the pipe character (`|`). If the pipe character is required you may specify it using `\|`. (so be careful using `string.split()`)
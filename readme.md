Latexish Text Processor
=====

A text processor that allows latex-style macros.

Roadmap
---

Current version: 0.2

+ 0.3 - Standard library created
+ 0.4 - CLI created
+ 0.5 - Minimized parser
+ 1.0 - Fully useable system, with specification, documentation, and examples
+ 2.0 - Ability to define symbols
+ 2.1 - Package manager

Features
---

Macros can output other macros. For instance:

	\foo

Could output

	\bar

which would then be processed, and possibly return

	baz

This is very powerful, and lets you build up useful libraries of helper functions, and create semantic documents rather than syntatic documents. Here's an example. Assume that a `\bold` and a `\italics` tags exist. Let's say you're writing a screenplay. Instead of doing:

	\italics{bob}: "Hey don't do that"
	\italics{craig}: \bold{Continues to do what he was doing}

You can define some macros like so:

	\newCommand{actor}{1}{\italics{#1}:}
	\newCommand{says}{1}{"#1"}
	\newCommand{does}{1}{\bold{#1}}

Then you can write:

	\actor{bob} \says{Hey don't do that}
	\actor{craig} \does{Continues to do what he was doing}

The benefit of doing this is that now the document has only *semantic* formatting, that is the only formatting that exists in the body is formatting that has real meaning, rather than what you decide the style should look like. If the editor later says "Actually I think you should use single quotes instead" you only need to change it in one place (in this case you do `\newCommand{says}{1}{'#1'}`) 
	

Plans
----

The basic functionality has all been created, the next step is to set up the command line interface, and create the standard library and several examples.



Standard library
---

+ `\processAndIgnore` - *eager* - process whatever is inside the tags, but ignore the result. This is useful for wrapping a section of `\newCommand` macros without their whitespace appearing
+ `\rawInsert` - *lazy* - don't process whatever is contained within this section, just include it verbatim. The only command that works within this mode is `\}` which is required in order to escape `}` so that the command doesn't end early.
+ `\comment` - *lazy* - equivalent to both `\processAndIgnore` and `\rawInsert`. Again the only command that works inside it `\}`
+ `\now{format}` - returns the current date and time formatted as specified

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

Commands may have parameters, which are separated with the pipe character (`|`). If the pipe character is required you may specify it using `\|`. (so be careful using `string.split()`). The following list of commands are supported:

+ `listAvailable` - The macro provider should return a list of macros available, separated by pipes, each one followed by the number of parameters it accepts, for example:
	`Test | 1 | Email | 4`
Provides 2 commands, one test one that takes 1 argument, and an e-mail command that takes 4.
+ `call|%macroname%|%param1%|%param2%` - The call command is followed by the name of a macro and then the parameters. The provider will return the text to substitute at that location  

More commands may be supported in future versions
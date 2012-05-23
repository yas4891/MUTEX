cd bin\Debug

"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge" /targetplatform:v4 /out:mutex.exe GSTConsole.exe GSTAppLogic.dll DataRepository.dll Tokenizer.dll CTokenizer.dll GSTLibrary.dll log4net.dll Antlr3.Runtime.dll

cd ..\Release

"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge" /targetplatform:v4 /out:mutex.exe GSTConsole.exe GSTAppLogic.dll DataRepository.dll Tokenizer.dll CTokenizer.dll GSTLibrary.dll log4net.dll Antlr3.Runtime.dll

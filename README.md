MUTEX
=====

MUTEX helps you prevent software plagiarism among students. 
It was designed for real-time comparison of source codes handed in by students at university. 
It is used at the University of the German Armed Forces (http://www.unibw.de)

How it works
----------
Given a new piece of source code (named 'NEW_SRC'), MUTEX will load the set of previously handed in source codes ('OLD_SET'). 
It will then parse NEW_SOURCE and each element of OLD_SET into separate token sequences. 
Comparing NEW_SRC against each element in OLD_SET, it will calculate the similarity between the two token sequences. After that it will determine the 
maximum similarity between NEW_SRC and *any* element in OLD_SET. If the similarity is above a pre-defined threshold (default: 50 percent), NEW_SRC will 
be considered a rip-off. MUTEX will store NEW_SRC in the database for further use and return the maximum similarity - along with an identifier for the corresponding element of OLD_SET - on stdout. 



Usage 
-----------
1. Load solution
2. Build solution as "Release"
3. run "GSTConsole/bin/Release/mutex.exe"


Greedy-String-Tiling algorithm 
------------
To determine the similarity between two sequences A and B MUTEX uses an algorithm known as Greedy String Tiling. 
You can read more about that at http://page.mi.fu-berlin.de/prechelt/Biblio/jplagTR.pdf 

MUTEX implements two version of the GST algorithm: 
1. The original algorithm with average complexity of O(n^2) and worst-case complexity O(n ^ 3) is implemented in GSTLibrary/tile/GSTAlgorithm.cs
2. an optimized version with average complexity of O(n) is implemented in GSTLibrary/tile/HashingGSTAlgorithm.cs

It is recommended to use the optimized version HashingGSTAlgorithm


What are all those folders for? 
------------
If time permits, I will try to clean up this mess. 

Until then, here is a list of the different projects:

- GreedyStringTiling (top-level directory): a small graphical demo application used during presentations; nothing useful in here
- ThirdPartyLibs: a collection of the libraries needed to compile/run MUTEX
- DataRepository: handles storing/retrieving values from the database
- Tokenizer: contains the language-agnostic elements needed for parsing source code
- CTokenizer: implements a specialised grammar for the C programming language with the aim to better detect plagiarism
- GSTLibrary: the "good" stuff - most everything that relates to the GST algorithm can be found in this project
- GSTAppLogic: ties all the ends (data storage, tokenizer, GST algorithm) together into a functional piece of software
- GSTConsole: a basic command line UI for MUTEX

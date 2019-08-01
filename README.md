# Code-Analysis-Software-Tool
A graduate coursework software project taught by professor Jim Fawcett

## Overview
Code analysis consists of extracting lexical content from source code files, analyzing the code's syntax from its lexical content, and building an Abstract Syntax Tree (AST) that holds the results of our analysis. It is then fairly easy to build several backends that can do further analyses on the AST to construct code metrics, search for particular constructs, evaluate package dependencies, or some other interesting features of the code.

A typical application of remote code analysis is for Code Repositories. For that, Quality Assurance staff will run analyses on code in a remote repository from clients on their desktops. Also, developers will analyze code, written by other developers, that they need for their own work.

### Phase 1
The Phase 1 of this project consisted of writing an Operational Concept Document(OCD). It's purpose was to think critically about the design and implementation of the project before commiting to code. The complete documentation on the project is provided in the OCD present in this repository.

### Phase 2
The Phase 2 consisted of build parsers for the software tool, mainly the Tokenizer and Semi-expression package.
#### Tokenizer
The Tokenizer extracts words, called tokens, from a stream of characters.Token boundaries are white-space characters, transitions between alphanumeric and punctuator characters, and comment and string boundaries. Certain classes of punctuator characters belong to single character or two character tokens so they require special rules for extraction.
#### Semi-expression
Semi-expression groups tokens into sets, each of which contain all the information needed to analyze some grammatical construct without containing extra tokens that have to be saved for subsequent analyses. SemiExpressions are determined by special terminating characters: semicolon, open brace, closed brace, and newline when preceeded on the same line with 'using'.

### Phase 3
In Phase 3, a local test package dependency analyzer was built with the following packages:
#### TypeTable and TypeAnalysis
The TypeTable provides a container that stores type information needed for dependency analysis. The TypeAnalysis finds all the types defined in each of a collection of C# source files. It does this by building rules to detect type definitions - classes, structs, enums, and aliases.
#### Dependency Analysis
The DependencyAnalysis package finds, for each file in a specified collection, all other files from the collection on which they depend. File A depends on file B, if and only if, it uses the name of any type defined in file B. It might do that by calling a method of a type or by inheriting the type. Note that this intentionally does not record dependencies of a file on files outsied the file set, e.g., language and platform libraries.
#### Strong Component
A strong component is the largest set of files that are all mutually dependent. That is, all the files whcih can be reached from any other file in the set by following direct or transitive dependency links. The term 'Strong Component' comes from the theory of directed graphs. There are a number of algorithms for finding strong components in graphs. tarjan Algorithm was implemented to achieve this.

### Phase 4
In Phase 4, a remote pacakage dependency analyzer was built using the following packages:
#### Comm
The Comm package implements asynchronous message passing communication using the Windows Communication Foundation Framework (WCF), which provides a well-engineered set of communication functionalities wrapping sockets and windows IPC.
#### Server
A package residing on a remote machine that exposes an HTTP endpoint for Comm Channel connections. The Server implements all the functionalities developed in Phase #3.
#### Client
A package, based on Windows Presentation Foundation (WPF), residing on the local machine. This package provides facilities for connecting a channel to the remote Server. This package provides the capabilitiy for sending requests messages for each of the functionalities of Phase #3, and for receiving messages with the results, and displaying the resulting information.

## <How to run the Code/>
The code directory consists of three bat files namely clean.bat, compile.bat and run.bat. Run the following scripts sequentially. Three windows would pop-up mainly, the Navigation server, Navigation console client and WPF client.
(The code could also be run as shown below in the output. Note: Start Visual Studio in Administrator mode)

## Output

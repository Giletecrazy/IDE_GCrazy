This was college project made in more or less 2 months, it can do lexical and syntactic anlyses in real time as we type or per user request, by default it runs in real time as a background worker.
The syntactic analyse uses ANTLR and the Lexical one uses Regular expressions.
When changing the analyses mode the text needs to be changed so that the background worker detects a difference and triggers the anlyser.

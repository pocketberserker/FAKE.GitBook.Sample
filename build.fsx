#r @"packages/FAKE/tools/FakeLib.dll"
#I @"packages/FAKE.GitBook/lib/net451"
#I "packages/FSharp.Formatting/lib/net40"
#I "packages/FSharp.Compiler.Service/lib/net40"
#I "packages/FSharpVSPowerTools.Core/lib/net45"
#r @"packages/FAKE.GitBook/lib/net451/Fake.GitBook.dll"
open Fake

Target "Generate" (fun _ ->
  GitBook id (fun p -> { p with SrcDir = currentDirectory @@ "src" }) Html
)

RunTargetOrDefault "Generate"


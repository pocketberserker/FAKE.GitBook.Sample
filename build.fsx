#r @"packages/FAKE/tools/FakeLib.dll"
#I @"packages/FAKE.GitBook/lib/net451"
#I "packages/FSharp.Formatting/lib/net40"
#I "packages/FSharp.Compiler.Service/lib/net40"
#I "packages/FSharpVSPowerTools.Core/lib/net45"
#r @"packages/FAKE.GitBook/lib/net451/Fake.GitBook.dll"
open Fake
open Fake.Git

let gitOwner = "pocketberserker"
let gitHome = "https://github.com/" + gitOwner
let gitName = "FAKE.GitBook.Sample"

Target "Generate" (fun _ ->
  GitBook id (fun p -> { p with SrcDir = currentDirectory @@ "src" }) Html
)

Target "Release" (fun _ ->
  let tempDocsDir = "temp/gh-pages"
  CleanDir tempDocsDir
  Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

  CopyRecursive "gitbook/_book" tempDocsDir true |> tracefn "%A"
  StageAll tempDocsDir
  Git.Commit.Commit tempDocsDir (sprintf "auto commit on AppVeyor %s" (environVar "APPVEYOR_BUILD_NUMBER"))
  Branches.push tempDocsDir
)

RunTargetOrDefault "Generate"


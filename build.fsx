#r @"packages/FAKE/tools/FakeLib.dll"
#I "packages/FSharp.Formatting/lib/net40"
#I "packages/FSharp.Compiler.Service/lib/net40"
#I "packages/FSharpVSPowerTools.Core/lib/net45"
#r @"packages/FAKE.GitBook/lib/net451/Fake.GitBook.dll"
open Fake
open Fake.Git
open System

let gitOwner = "pocketberserker"
let gitHome = "https://github.com/" + gitOwner
let gitName = "FAKE.GitBook.Sample"

Target "Generate" (fun _ ->
  GitBook id id [Html]
)

Target "GenerateAll" (fun _ ->
  GitBook id id [ Html; Pdf "book"; EPub "book" ]
)

let release () =
  let tempDocsDir = "temp/gh-pages"
  CleanDir tempDocsDir
  Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

  CopyRecursive "gitbook/_book" tempDocsDir true |> tracefn "%A"
  StageAll tempDocsDir
  Git.Commit.Commit tempDocsDir (sprintf "auto commit on AppVeyor %s" (environVar "APPVEYOR_BUILD_NUMBER"))
  Branches.push tempDocsDir

Target "Release" release

Target "ReleaseFromAppveyor" (fun _ ->
  let branch = environVar "APPVEYOR_REPO_BRANCH"
  let pr = environVar "APPVEYOR_PULL_REQUEST_NUMBER"
  if branch = "master" && String.IsNullOrEmpty(pr) then release ()
)

RunTargetOrDefault "Generate"


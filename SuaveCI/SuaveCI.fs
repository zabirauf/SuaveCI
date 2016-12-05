module SuaveCI

open Suave
open Suave.Successful
open Suave.Web
open Suave.Operators
open Suave.Filters

open System
open System.Net
open System.Threading.Tasks

let helloWorld _ = 
    printfn "Saying hello world from F# %O" DateTime.UtcNow
    OK (sprintf "<html><body><h1>Hello World - %O</h1></body></html>" DateTime.UtcNow)

let app = 
        GET >=> path "/" >=> request helloWorld

let config (port : string) = 
    let ip127  = IPAddress.Parse("127.0.0.1")
    let ipZero = IPAddress.Parse("0.0.0.0")

    { defaultConfig with 
        logger = Logging.Loggers.saneDefaultsFor Logging.LogLevel.Verbose
        bindings=[ (if port = String.Empty then HttpBinding.mk HTTP ip127 (uint16 8080)
                    else HttpBinding.mk HTTP ipZero (uint16 port)) ] }
                   
[<EntryPoint>]
let main argv =
    let port = match argv with 
                | [|portArg|] -> portArg
                | _ -> String.Empty
    startWebServer (config port) app
    0 // return an integer exit code
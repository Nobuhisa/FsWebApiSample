open System
open System.Net.Http
open System.Net.Http.Headers
open System.IO


let baseUrl  = "http://localhost:48213/"
let action   = "Api/Files/Upload"


/// 指定したファイルをコンテンツコンテナに登録する。
let addContent (container:MultipartFormDataContent) filePath =
    let file = new StreamContent(File.OpenRead(filePath))
    ContentDispositionHeaderValue("attachment")
    |> fun header ->
        header.FileName <- Path.GetFileName(filePath)
        file.Headers.ContentDisposition <- header
    container.Add(file)

/// 指定したファイルをWeb API経由でアップロード。
let upload files = async {
    use container = new MultipartFormDataContent()
    use client    = new HttpClient(BaseAddress = Uri(baseUrl))

    files |> List.iter (addContent container)
    use! response = client.PostAsync(action, container) |> Async.AwaitTask
    printfn "%s" response.ReasonPhrase // 結果を出力
}


[<EntryPoint>]
let main _ =
    printfn "アップロードを開始します..."

    [@"../../fsharp_logo.png"]
    |> upload
    |> Async.RunSynchronously

    Console.ReadKey() |> ignore
    0
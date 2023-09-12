namespace SharpBeat.Lib.GUI
open System.Net.Http
open System.Net.Http.Headers
open System.Net
open FSharp.JsonApi



module Song =
    type Song = {
        Id :  int64
        Title : string
        Artist : string
        Hash : string
        Duration : int16
        Genre : string
    } with
    override this.ToString () : string =
        sprintf "%s - %s" this.Artist this.Title
    member this.url () : string =
        sprintf "http://179.50.238.48:8080/%s/outputlist.m3u8" this.Hash

module Api =
    open Song

    let apiUrl = "http://179.50.238.48:8080/api/songs/"
    let private requestAsync (apiUrl: string) (query: string): Option<HttpResponseMessage> = 
        let client = new HttpClient()
        client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue("application/json"))
        async {
                try
                    let! response = client.GetAsync(apiUrl + query) |> Async.AwaitTask
                    client.Dispose()
                    return Some response
                with
                | ex -> return None
        } |> Async.RunSynchronously
    
    // NOTE: the reason I'm not using option here is because if there's an error
    // the caller will just get an empty list
    let private handleResponse (response: HttpResponseMessage): Song list = 
        if response.StatusCode = HttpStatusCode.OK then
            async {
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                let posts = Newtonsoft.Json.JsonConvert.DeserializeObject<Song list>(content)
                return posts
            } |> Async.RunSynchronously
        else
            []

    let getSongs (query: string) : Song list =
        requestAsync apiUrl query
        |> function
        | Some(response) -> handleResponse response
        | None -> []

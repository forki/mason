﻿namespace Mason

open System
open System.Collections
open System.IO
open System.Linq
open System.Text

open Kajabity.Tools.Java


type JavaProperties(file: FileInfo, encoding: Encoding) as self =
    let _props = Kajabity.Tools.Java.JavaProperties()
    do
        match null2opt file with
        | Some f ->
            use stream = f.OpenRead()
            _props.Load(stream, match null2opt encoding with Some e -> e | None -> Encoding.UTF8)
        | None -> raise (ArgumentNullException "file")
    member __.Keys with get() = _props.Keys.Cast<string>()
    member __.Item with get(key) = 
                        match null2opt key with
                        | Some k -> 
                            match null2opt (_props.GetProperty(k)) with
                            | Some s -> 
                                if ((s.[0] = '"') && (s.[s.Length - 1] = '"')) then s.[1..s.Length - 2]
                                else s
                            | None -> null
                        | None -> raise (ArgumentNullException("key"))
                        
    new (file: FileInfo) = JavaProperties(file, Encoding.UTF8)
    new (path: string, encoding: Encoding) = JavaProperties(new FileInfo(path), encoding)
    new (path: string) = JavaProperties(new FileInfo(path))

    interface IMasonProperties with
        member __.Keys with get() = self.Keys
        member __.Item with get(key) = self.[key]
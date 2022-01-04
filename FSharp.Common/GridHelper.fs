namespace FSharp.Common

open System

module GridHelper =

    let CoordinatesToIndex (x: int, y: int, rowSize: int, totalSize: int) =
        if x < 0 || x >= rowSize then
            None
        elif y < 0 || y >= (totalSize / rowSize) then
            None
        else
            let index = (y * rowSize) + x
            index |> Some


    let IndexToCoordinates (index: int, rowSize: int, totalSize: int) =
        if index < 0 || index >= totalSize then
            None
        else
            let y = index / rowSize
            let x = index - (y * rowSize)
            (x, y) |> Some


    let Offsets4 = [
        ( 0,-1);
        ( 1, 0);
        ( 0, 1);
        (-1, 0);
        ]

    let Offsets8 = [
        ( 0,-1); ( 1,-1);
        ( 1, 0); ( 1, 1);
        ( 0, 1); (-1, 1);
        (-1, 0); (-1,-1);
        ]


    let GetAdjacentIndexes(idx: int, rowSize: int, totalSize: int, offsets: (int * int) list) =
        let y = idx / rowSize
        let x = idx - (y * rowSize)

        let result = 
            offsets
            |> List.map (fun (ox, oy) -> CoordinatesToIndex(x+ox, y+oy, rowSize, totalSize))
            |> List.where (fun i -> i.IsSome)
            |> List.map (fun i -> i.Value)
        result
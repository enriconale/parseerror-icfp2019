
namespace ICFP2019

type Action = W | S | A | D | Z | E | Q | B of int * int | F | L
with
    member self.mumumu x =
        match x with
        | A -> W
        | S -> A
        | _ -> failwith "fff"





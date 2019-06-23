
namespace ICFP2019

type Action = W | S | A | D | Z | E | Q | B of int * int | F | L | R of int * int | C 
with
    override self.ToString () = sprintf "%A" self

type Goal =
    | GoTo of int * int
    | UseDrill
with
    override self.ToString () = sprintf "%A" self



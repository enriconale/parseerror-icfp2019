
namespace ICFP2019

type Action = W | S | A | D | Z | E | Q | B of int * int | F | L | R of int * int | C 

type Goal =
    | GoTo of int * int
    | UseDrill
    // TODO: aggiungerne altri



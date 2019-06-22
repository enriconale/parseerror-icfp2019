
namespace ICFP2019

type Action = W | S | A | D | Z | E | Q | B of int * int | F | L

type Objective =
    | GoTo of int * int
    | UseDrill
    | UseManip
    // TODO: aggiungerne altri



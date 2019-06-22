
namespace ICFP2019
{

    public partial class Status
    {
        public void execute(Action a)
        {
            if (a.IsW) wrappy.Loc += new Point(0, 1);
            if (a.IsA) wrappy.Loc += new Point(-1, 0);
            if (a.IsD) wrappy.Loc += new Point(1, 0);
            if (a.IsS) wrappy.Loc += new Point(0, -1);

            if (a.IsE) wrappy.rotateClockwise();
            if (a.IsQ) wrappy.rotateAntiClockwise();

            if (a.IsB)
            {
                Action.B b = (Action.B) a;
                wrappy.Manips.Add(new Point(b.Item1, b.Item2));
                // TODO: checkare che il punto sia davvero adiacente
            }
            // TODO: gestire F, L e gli altri booster

            updateMap();
        }

        public void updateMap()
        {
            foreach (Point p in wrappy.Manips)
            {
//                int x = wrappy.Loc.x + p.x 
            }
        }


    }

}
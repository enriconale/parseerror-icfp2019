using System;

namespace ICFP2019.Properties
{
    public class ShortestTileAlgo
    {
        private Map<Tile> map;
        public static readonly int UNREACHABLE = Int32.MaxValue;

        public ShortestTileAlgo(Map<Tile> map)
        {
            this.map = map;
        }

        private int minAround(Map<int> distMap, Point p)
        {
            int min = Int32.MaxValue;
            if (p.x > 0 && distMap[p.x - 1, p.y] < min) min = distMap[p.x - 1, p.y];
            if (p.x < map.W && distMap[p.x + 1, p.y] < min) min = distMap[p.x + 1, p.y];
            if (p.y > 0 && distMap[p.x, p.y - 1] < min) min = distMap[p.x, p.y - 1];
            if (p.x < map.H && distMap[p.x, p.y + 1] < min) min = distMap[p.x, p.y + 1];
            return min;
        }

        private Map<int> calculateMap(Wrappy w)
        {
            Map<int> distMap = new Map<int>(map.W, map.H);

            distMap.fillWith(UNREACHABLE);
            distMap[w.Loc] = 0;

            Point p = new Point();

            // cycle through all radii
            for (int r = 1; r < Math.Max(map.W, map.H); ++r)
            {
                // cycle through rhombus (i.e. L-1 circle) side length, coordinates
                // are computed as (i, r-i), depends on which side (have l-1 distance
                // = r from wrappy position)
                for (int i = 0; i < r; ++i)
                {
                    // compute offset for each side, sign of
                    // x/y offset based on side is:
                    //
                    //         1     @     0
                    //        -/+  @   @  +/+
                    //           @       @
                    //         @     X     @
                    //           @       @
                    //        -/-  @   @  +/-
                    //         2     @     3
                    //
                    for (int side = 0; side < 4; ++side)
                    {
                        // trick: 0 = 3 (mod 3)
                        int xSign = (side % 3 == 0) ? 1 : -1;
                        int ySign = (side < 2) ? 1 : -1;
                        p.x = w.Loc.x + i * xSign;
                        p.y = w.Loc.y + (r - i) * ySign;
                        if (map.validCoordinate(p) && map[p] != Tile.Obstacle)
                            distMap[p] = minAround(distMap, p) + 1;
                    }
                }
            }
            return distMap;
        }
    }
}

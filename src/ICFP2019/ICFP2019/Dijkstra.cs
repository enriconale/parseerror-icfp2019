﻿using System;
using System.Collections.Generic;

namespace ICFP2019.Dijkstra
{

    public class Edge
    {
        public int V2, Length;

    }

    public class Vertex
    {
        public int Id;
        public List<Edge> OutgoingEdges;
    }

    public class Graph
    {
        public static readonly int UNREACHABLE = int.MaxValue;
        public static readonly double GOTO_PRI = 0.001;
        public static readonly double SKIP_PRI = 1000.0;

        private List<Vertex> vertices = new List<Vertex>();
        private Map<Tile> map;

        public Graph(Map<Tile> m)
        {
            this.map = m;
            populate();
        }

        internal int idAt(int x, int y)
        {
            return x + y * map.W;
        }

        internal Edge edgeAt(int x, int y)
        {
            return new Edge
            {
                V2 = idAt(x, y),
                Length = map[x, y] == Tile.Obstacle ? UNREACHABLE : 1
            };
        }

        public void populate()
        {
            for (int y = 0; y < map.H; ++y)
                for (int x = 0; x < map.W; ++x)
                {
                    Vertex v = new Vertex
                    {
                        Id = idAt(x, y),
                        OutgoingEdges = new List<Edge>()
                    };
                    if (y <= map.H - 2) v.OutgoingEdges.Add(edgeAt(x, y + 1));
                    if (x >= 1) v.OutgoingEdges.Add(edgeAt(x - 1, y));
                    if (x <= map.W - 2) v.OutgoingEdges.Add(edgeAt(x + 1, y));
                    if (y >= 1) v.OutgoingEdges.Add(edgeAt(x, y - 1));
                    vertices.Add(v);
                }
        }

        public int N
        {
            get
            {
                return vertices.Count;
            }
        }

        public struct Result
        {
            public Map<int> distMap;
            public List<PriGoal> priGoals;
        }

        public Result CalculateMap(Wrappy w, Status status)
        {
            Point p = w.Loc;
            return CalculateMap(vertices[idAt(p.x, p.y)], w, status);
        }


        private Result CalculateMap(Vertex v, Wrappy w, Status status)
        {
            List<Goal> goals = status.goals;
            List<PriGoal> priGoals = new List<PriGoal>();
            Map<int> distMap = new Map<int>(map.W, map.H, UNREACHABLE);
            distMap[w.Loc] = 0;
            //int min = int.MaxValue;
            var crossingEdges = new MinHeap<int, int>(N);
            var minVertices = new bool[N];
            minVertices[v.Id] = true;
            int max_goals = Math.Min(map.H, goals.Count);

            foreach (var outgoingEdge in v.OutgoingEdges)
            {
                // Use a Heapify operation!
                crossingEdges.Insert(outgoingEdge.V2, outgoingEdge.Length);
            }

            while (crossingEdges.Count > 0)
            {
                //DijkstraPrettyPrinter.printDijkstraMap(distMap, w);
                var minEdge = crossingEdges.ExtractMin();

                int v2 = minEdge.Key;
                var newVertex = vertices[v2];
                int X = v2 % map.W, Y = v2 / map.W;
                distMap[X, Y] = minEdge.Value;
                if (minEdge.Value != 0)
                {
                    // cerca il GoTo di coordinate X, Y (cioè il più vicino, appena estratto dal minheap)
                    int i;
                    if ((i = goals.FindIndex((g) =>
                    {
                        if (g.IsGoTo)
                        {
                            Goal.GoTo goTo = (Goal.GoTo)g;
                            int x = goTo.Item1, y = goTo.Item2;
                            return x == X && y == Y;
                        }
                        else return false;
                    })) >= 0)
                    {
                        Goal.GoTo goTo = (Goal.GoTo)goals[i];
                        int x = goTo.Item1, y = goTo.Item2;

                        double pri = minEdge.Value;

                        if (w.remainingFastWheel > 0)
                            if (Math.Abs(w.Loc.x - x) % 2 == 1
                                || Math.Abs(w.Loc.y - y) % 2 == 1)
                                pri *= SKIP_PRI;
                            else
                                pri *= GOTO_PRI;

                        else
                        {
                            if (status.boosters.Exists((kvp) =>
                                kvp.Value.x == x
                                && kvp.Value.y == y
                                && kvp.Key == Booster.CloningPlatform))
                            {
                                if (status.collectedBoosters.Contains(Booster.Cloning)
                                    || status.map[x, y] == Tile.Empty)
                                    pri *= GOTO_PRI;
                                else
                                    pri *= SKIP_PRI;
                            }

                            if (status.boosters.Exists((kvp) =>
                                kvp.Value.x == x
                                && kvp.Value.y == y
                                && kvp.Key == Booster.Manipulator))
                            {
                                pri *= GOTO_PRI;
                            }

                        }

                        priGoals.Add(new PriGoal { goal = goTo, pri = pri });
                        //if (priGoals.Count >= max_goals) goto quit;
                        //return new Result { distMap = distMap, priGoals = priGoals };
                    }
                }

                minVertices[v2] = true;
                //if (minEdge.Value < min)
                //    min = minEdge.Value;

                foreach (var newEdge in newVertex.OutgoingEdges)
                {
                    if (minVertices[newEdge.V2] || newEdge.Length == UNREACHABLE)
                        continue;

                    var edgeLength = newEdge.Length + minEdge.Value;

                    var edge = crossingEdges.Find(newEdge.V2);
                    if (edge != null)
                    {
                        if (edgeLength < edge.Value)
                        {
                            crossingEdges.Update(newEdge.V2, edgeLength);
                        }
                    }
                    else
                    {
                        crossingEdges.Insert(newEdge.V2, edgeLength);
                    }
                }
            }
            quit:
            return new Result { distMap = distMap, priGoals = priGoals };
        }

    }


    public class MinHeap<TKey, TValue>
            where TValue : IComparable<TValue>
    {
        public class KeyValuePair
        {
            private TKey _key;
            private TValue _value;

            public KeyValuePair(TKey key, TValue value)
            {
                _key = key;
                _value = value;
            }

            public TKey Key
            {
                get
                {
                    return _key;
                }
            }

            public TValue Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                }
            }
        }

        private KeyValuePair[] _heap;
        private int _count = 0;
        private Dictionary<TKey, int> _heapIndex;
        private static readonly int BufferSize = 100;

        public MinHeap()
        {
            _heap = new KeyValuePair[BufferSize];
            _heapIndex = new Dictionary<TKey, int>(BufferSize);
        }

        public MinHeap(int capacity)
        {
            _heap = new KeyValuePair[capacity];
            _heapIndex = new Dictionary<TKey, int>(capacity);
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void Insert(TKey key, TValue value)
        {
            var kvp = new KeyValuePair(key, value);
            _heapIndex[key] = _count;
            _heap[_count++] = kvp;
            BubbleUp(_count - 1);
        }

        public void Delete(TKey key)
        {
            int nodeIndex = _heapIndex[key];
            var kvp = _heap[--_count];
            _heap[nodeIndex] = kvp;
            _heapIndex[kvp.Key] = nodeIndex;
            _heapIndex.Remove(key);
            int parentIndex = GetParentIndex(nodeIndex);
            bool bubbleUp = false;
            if (parentIndex > 0)
            {
                var parentNode = _heap[parentIndex];
                if (kvp.Value.CompareTo(parentNode.Value) < 0)
                {
                    Swap(nodeIndex, parentIndex);
                    _heapIndex[key] = parentIndex;
                    _heapIndex[parentNode.Key] = nodeIndex;
                    BubbleUp(parentIndex);
                    bubbleUp = true;
                }
            }
            if (!bubbleUp)
            {
                if (_count > (nodeIndex + 1))
                {
                    BubbleDown(nodeIndex);
                }
            }
        }

        public KeyValuePair Find(TKey key)
        {
            if (_heapIndex.ContainsKey(key))
                return _heap[_heapIndex[key]];
            return null;
        }

        public void Update(TKey key, TValue value)
        {
            var nodeIndex = _heapIndex[key];
            _heap[nodeIndex].Value = value;

            int parentIndex = GetParentIndex(nodeIndex);
            bool bubbleUp = false;
            if (parentIndex >= 0)
            {
                var parentNode = _heap[parentIndex];
                if (parentNode.Value.CompareTo(value) > 0)
                {
                    Swap(nodeIndex, parentIndex);
                    _heapIndex[key] = parentIndex;
                    _heapIndex[parentNode.Key] = nodeIndex;
                    BubbleUp(parentIndex);
                    bubbleUp = true;
                }
            }
            if (!bubbleUp)
            {
                int minIndex = GetMinChildIndex(nodeIndex);
                if (minIndex > 0)
                {
                    var minNode = _heap[minIndex];
                    if (value.CompareTo(minNode.Value) > 0)
                    {
                        Swap(nodeIndex, minIndex);
                        _heapIndex[key] = minIndex;
                        _heapIndex[minNode.Key] = nodeIndex;
                        BubbleDown(minIndex);
                    }
                }
            }
        }

        public KeyValuePair ExtractMin()
        {
            KeyValuePair min = _heap[0];
            _count--;
            if (_count > 0)
            {
                var kvp = _heap[_count];
                _heap[0] = kvp;
                TKey key = _heap[0].Key;
                _heapIndex[key] = 0;
                _heapIndex.Remove(min.Key);
                if (_count > 1)
                {
                    BubbleDown(0);
                }
            }
            return min;
        }

        private void BubbleUp(int nodeIndex)
        {
            if (nodeIndex != 0)
            {
                int parentIndex = GetParentIndex(nodeIndex);
                var node = _heap[nodeIndex];
                var parentNode = _heap[parentIndex];
                if (parentNode.Value.CompareTo(node.Value) > 0)
                {
                    Swap(nodeIndex, parentIndex);
                    _heapIndex[node.Key] = parentIndex;
                    _heapIndex[parentNode.Key] = nodeIndex;
                    BubbleUp(parentIndex);
                }
            }
        }

        private void BubbleDown(int nodeIndex)
        {
            var node = _heap[nodeIndex];
            int minIndex = GetMinChildIndex(nodeIndex);
            if (minIndex > 0)
            {
                var minNode = _heap[minIndex];
                if (node.Value.CompareTo(minNode.Value) > 0)
                {
                    Swap(minIndex, nodeIndex);
                    _heapIndex[node.Key] = minIndex;
                    _heapIndex[minNode.Key] = nodeIndex;
                    BubbleDown(minIndex);
                }
            }
        }

        private void Swap(int node1, int node2)
        {
            var tmpNode = _heap[node1];
            _heap[node1] = _heap[node2];
            _heap[node2] = tmpNode;
        }

        private int GetParentIndex(int index)
        {
            return (index + 1) / 2 - 1;
        }

        private int GetChildStartIndex(int index)
        {
            return index * 2 + 1;
        }

        private int GetChildEndIndex(int index)
        {
            return index * 2 + 2;
        }

        private int GetMinChildIndex(int index)
        {
            int startIndex = GetChildStartIndex(index);
            if (startIndex >= _count)
                return -1;
            int endIndex = GetChildEndIndex(index);
            if (endIndex >= _count)
                return startIndex;
            return (_heap[startIndex].Value.CompareTo(_heap[endIndex].Value) < 0)
                    ? startIndex : endIndex;
        }
    }
}
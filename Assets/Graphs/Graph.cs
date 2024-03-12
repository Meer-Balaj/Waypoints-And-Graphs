using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph 
{
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph() { }

    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEgde(GameObject fromNode, GameObject ToNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(ToNode);

        if(from != null && to != null)
        {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgeList.Add(e);
        }
    }

    Node FindNode(GameObject id)
    {
        foreach (Node n in nodes)
        {
            if(n.GetID()== id)
            {
                return n;
            }
        }
        return null;
    }

    public bool AStar(GameObject startID, GameObject endID)
    {
        if(startID == endID)
        {
            pathList.Clear();
            return false;
        }

        Node start = FindNode(startID);
        Node end = FindNode(endID);

        if (start == null || end == null)
        {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tentative_is_better;

        //g is the total distance travelled
        start.g = 0;
        //h is the distance from one node to the next node
        start.h = Distance(start, end);
        // sum of h and g
        start.f = start.h;

        open.Add(start);
        while(open.Count > 0)
        {
            int i = LowestF(open);
            Node thisNode = open[i];
            if( thisNode.GetID() == endID)
            {
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbour;
            foreach(Edge e in thisNode.edgeList)
            {
                neighbour = e.endNode;

                if(closed.IndexOf(neighbour) > -1)
                {
                    continue;
                }

                tentative_g_score = thisNode.g + Distance(thisNode, neighbour);
                if(open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tentative_is_better = true;
                }
                else if(tentative_g_score < neighbour.g)
                {
                    tentative_is_better = true;
                }
                else
                {
                    tentative_is_better = false;
                }

                if(tentative_is_better)
                {
                    neighbour.cameFrom = thisNode;
                    neighbour.g = tentative_g_score;
                    neighbour.h = Distance(thisNode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }

            }
        }
        return false;
    }

    public void ReconstructPath(Node startID, Node endID)
    {
        pathList.Clear();
        pathList.Add(endID);

        var p = endID.cameFrom;
        while(p != startID && p!= null)
        {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startID);
    }

    float Distance(Node a, Node b)
    {
        return (Vector3.SqrMagnitude(a.GetID().transform.position - b.GetID().transform.position));
    }

    int LowestF(List<Node> l)
    {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestf = l[0].f;

        for (int i = 1; i < count; i++)
        {
            if (l[i].f < lowestf)
            {
                lowestf = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;
    }
}

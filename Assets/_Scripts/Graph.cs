using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    //member variables
    List<Node> m_Nodes;
    public GameObject nodeType;
    GameObject NodeObject;

    public delegate float heuristicCheck(Node a, Node b);
    public heuristicCheck m_HeuristicCheck;

    //accessor
    public List<Node> Nodes
    {
        get
        {
            return m_Nodes;
        }

        set
        {
            m_Nodes = value;
        }
    }

    //automatically generates a graph based on what's input.
    public void CreateGraph(int x, int y, float s)
    {
        Nodes = new List<Node>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                NodeObject = Instantiate(nodeType, new Vector3(i*s, j*s, 0), Quaternion.identity);
                NodeObject.GetComponent<Node>().setup(new Vector2(i, j), 0);
                NodeObject.transform.SetParent(gameObject.transform);
                Nodes.Add(NodeObject.GetComponent<Node>());
            }
        }
        foreach(Node node in Nodes)
        {
            node.createEdges(Nodes, s);
        }
    }

    //Add functions
    void AddNode(Node node)
    {
        Nodes.Add(node);
    }
    /*void AddConnection(Node a, Node b, float cost)
    {
        Edge edge = new Edge(b, cost);
        a.Edges.Add(edge);
    }*/

    //Find Functions
    public Node FindNodeAtIndex(int x, int y)
    {
        foreach (Node node in Nodes)
        {
            if(node.Index.x == x && node.Index.y == y)
            {
                return node;
            }
        }

        return null;
    }

    //Searching Algorithms
    public List<Node> DjikstraSearch(Node start, Node target)
    {
        //SET ALL THE Ns TO NULL, AND ALL THE Gs TO INFINITY//
        foreach(Node node in Nodes)
        {
            node.Parent = null;
            node.GScore = 999999.0f;
        }

        //let openList be a list of nodes
        List<Node> openList = new List<Node>();

        //let closedList be a list of nodes
        List<Node> closedList = new List<Node>();


        //PUSH START NODE ONTO PRIORITY QUEUE, SET ITS N TO ITSELF AND G TO 0//
        start.Parent = start;
        start.GScore = 0;
        openList.Add(start);

        //WHILE QUEUE IS NOT EMPTY
        while(openList.Count > 0)
        {
            //Sort openList by node.GScore
            openList.Sort((a, b) => { return a.GScore.CompareTo(b.GScore); });

            //GET THE CURRENT NODE OF THE TOP OF THE QUEUE AND REMOVE IT//
            Node currentNode = openList[0]; //hopefully that returns the first element.

            //remove currentNode from openlist
            openList.RemoveAt(0);

            //Process the node
            if (currentNode == target)
            {
                break;
            }

            //MARK IT AS TRAVERSED//
            //add currentNode to closed list
            closedList.Add(currentNode);
            currentNode.ShowTraversed();

            //LOOP THROUGH IT'S EDGES//
            foreach (Edge edge in currentNode.Edges)
            {
                //get this edge's end node
                Node edgeTarget = edge.End;

                //IF END NODE NOT TRAVERSED//
                if (!(closedList.Contains(edgeTarget)))
                {
                    //CALCULATE CURRENT NODES G SCORE + THE EDGE COST
                    float newScore = currentNode.GScore + edge.Cost;

                    //IF COST IS LESS THAN EXISTING G COST IN END NODE
                    if(newScore < edgeTarget.GScore)
                    {
                        //SET END NODES N TO THE CURRENT NODE
                        edgeTarget.Parent = currentNode;
                        
                        //SET END NODES G TO THE CURRENT NODES G + EDGE COST
                        edgeTarget.GScore = newScore;

                        //IF END NODE IS NOT IN THE QUEUE
                        if (!(openList.Contains(edgeTarget)))
                        {
                            //PUSH END NODE ONTO THE QUEUE
                            openList.Add(edgeTarget);
                        }
                    }
                }
            }


        }

        //create the list of nodes to return
        List<Node> returnList = new List<Node>();

        //set the first node to the end node
        Node nextNode = target;

        //go back through each node and add it.
        while(nextNode != nextNode.Parent)
        {
            returnList.Add(nextNode);
            nextNode = nextNode.Parent;
        }

        //add the starting node
        returnList.Add(nextNode.Parent);

        //return dat shiz
        return returnList;
    }

    public List<Node> AStarSearch(Node start, Node target)
    {
        m_HeuristicCheck = heuristic_Manhattan;

        //SET ALL THE Ns TO NULL, AND ALL THE Gs TO INFINITY//
        foreach (Node node in Nodes)
        {
            node.Parent = null;
            node.GScore = 999999.0f;
            node.FScore = 999999.0f;
            node.HScore = 999999.0f;
        }

        //let openList be a list of nodes
        List<Node> openList = new List<Node>();

        //let closedList be a list of nodes
        List<Node> closedList = new List<Node>();


        //PUSH START NODE ONTO PRIORITY QUEUE, SET ITS N TO ITSELF, G TO 0, HSCORE, AND FSCORE//
        start.Parent = start;
        start.GScore = 0;
        start.HScore = m_HeuristicCheck(start, target);
        start.FScore = start.GScore + start.HScore;
        openList.Add(start);

        //WHILE QUEUE IS NOT EMPTY
        while (openList.Count > 0)
        {
            //Sort openList by node.GScore
            //openList.Sort();
            openList.Sort((a, b) => { return a.FScore.CompareTo(b.FScore); });
            
            //GET THE CURRENT NODE OF THE TOP OF THE QUEUE AND REMOVE IT//
            Node currentNode = openList[0]; //hopefully that returns the first element.

            //remove currentNode from openlist
            openList.RemoveAt(0);

            //Process the node
            if (currentNode == target)
            {
                break;
            }

            //MARK IT AS TRAVERSED//
            //add currentNode to closed list
            closedList.Add(currentNode);
            currentNode.ShowTraversed();

            //LOOP THROUGH IT'S EDGES//
            foreach (Edge edge in currentNode.Edges)
            {
                //get this edge's end node
                Node edgeTarget = edge.End;

                //IF END NODE NOT TRAVERSED//
                if (!(closedList.Contains(edgeTarget)))
                {
                    //CALCULATE CURRENT NODES G SCORE + THE EDGE COST
                    float newFScore = currentNode.GScore + edge.Cost + m_HeuristicCheck(currentNode, edge.End);
                    float newGScore = currentNode.GScore + edge.Cost;
                    float newHScore = m_HeuristicCheck(currentNode, edge.End);

                    //IF COST IS LESS THAN EXISTING F COST IN END NODE
                    if (newFScore < edgeTarget.FScore)
                    {
                        //SET END NODES N TO THE CURRENT NODE
                        edgeTarget.Parent = currentNode;

                        //SET THE END NODES F TO THE CURRENT NODES G + EDGE COST + H OF END NODE
                        edgeTarget.FScore = newFScore;

                        edgeTarget.GScore = newGScore;
                        edgeTarget.HScore = newHScore;

                        //IF END NODE IS NOT IN THE QUEUE
                        if (!(openList.Contains(edgeTarget)))
                        {
                            //PUSH END NODE ONTO THE QUEUE
                            openList.Add(edgeTarget);
                        }
                    }
                }
            }


        }

        //create the list of nodes to return
        List<Node> returnList = new List<Node>();

        //set the first node to the end node
        Node nextNode = target;

        //go back through each node and add it.
        while (nextNode != nextNode.Parent)
        {
            returnList.Add(nextNode);
            nextNode = nextNode.Parent;
        }

        //add the starting node
        returnList.Add(nextNode.Parent);

        //return dat shiz
        return returnList;
    }


    //Heuristics
    static float heuristic_Djikstra(Node a, Node b)
    {
        return 0;
    }
    static float heuristic_Manhattan(Node a, Node b)
    {
        return (b.transform.position.x - a.transform.position.x) + (b.transform.position.y - a.transform.position.y);
    }
    static float heuristic_Distance(Node a, Node b)
    {
        float x = b.transform.position.x - a.transform.position.x;
        float y = b.transform.position.y - a.transform.position.y;

        return Mathf.Sqrt(x * x + y * y);
    }
}

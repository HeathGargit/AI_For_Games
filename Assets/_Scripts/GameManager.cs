using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Graph m_Graph;
    public float scale = 1;
    public int x = 5, y = 5, startX = 0, startY = 0, endX = 0, endY = 0;

	// Use this for initialization
	void Start () {
        m_Graph.CreateGraph(x, y, scale);
	}
	
	// Update is called once per frame
	void Update () {
        //uncolour all the nodes
        foreach (Node node in m_Graph.Nodes)
        {
            node.UnHighlightNode();
        }

        //grab the start and the end nodes from the engine
        Node startNode = m_Graph.FindNodeAtIndex(startX, startY);
        Node endNode = m_Graph.FindNodeAtIndex(endX, endY);

        //Debug.Log("x:" + startNode.Index.x + " y:" + startNode.Index.y);
        //Debug.Log("x:" + endNode.Index.x + " y:" + endNode.Index.y);

        //find the path!
        List<Node> highlightThese = m_Graph.DjikstraSearch(startNode, endNode);
        //List<Node> highlightThese = m_Graph.AStarSearch(startNode, endNode);

        //colour the path!
        foreach (Node node in highlightThese)
        {
            node.HighlightNode();
        }
    }
}

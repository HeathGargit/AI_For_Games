using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class Node : MonoBehaviour {

    public enum NodeType
    {
        Grass,
        Mountain,
        Swamp
    }

    public void Start()
    {
        
    }

    //member variables
    [SerializeField]
    Vector2 m_Index;
    bool m_Highlighted;
    List<Edge> m_Edges;
    NodeType m_NodeType;
    [SerializeField]
    float m_GScore;
    [SerializeField]
    float m_HScore;
    [SerializeField]
    float m_FScore;
    Node m_Parent;

    public GameObject EdgeType;
    GameObject EdgeObject;

    //accessors
    public float GScore
    {
        get
        {
            return m_GScore;
        }

        set
        {
            m_GScore = value;
        }
    }
    public List<Edge> Edges
    {
        get
        {
            return m_Edges;
        }

        set
        {
            m_Edges = value;
        }
    }
    public Vector2 Index
    {
        get
        {
            return m_Index;
        }

        set
        {
            m_Index = value;
        }
    }
    public Node Parent
    {
        get
        {
            return m_Parent;
        }

        set
        {
            m_Parent = value;
        }
    }
    public float HScore
    {
        get
        {
            return m_HScore;
        }

        set
        {
            m_HScore = value;
        }
    }
    public float FScore
    {
        get
        {
            return m_FScore;
        }

        set
        {
            m_FScore = value;
        }
    }

    //coz we don't use a constructor....
    public void setup(Vector2 index, NodeType nodeType)
    {
        m_Index = index;
        m_Highlighted = false;
        m_NodeType = nodeType;
    }

    //required interface overload
    /*public int CompareTo(Node node)
    {
        return m_GScore.CompareTo(node.GScore);
    }*/

    //node changey colour stuff.
    public void HighlightNode()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
    public void UnHighlightNode()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }
    public void ShowTraversed()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.magenta;
    }

    //auto-create edges to other nodes.
    public void createEdges(List<Node> otherNodes, float searchRange)
    {
        m_Edges = new List<Edge>();
        foreach (Node node in otherNodes)
        {
            if (Vector3.Distance(transform.position, node.transform.position) < (searchRange * 1.1) && (node != this))
            {
                EdgeObject = Instantiate(EdgeType, gameObject.transform.position, Quaternion.identity);
                EdgeObject.GetComponent<Edge>().setup(node, 1);
                EdgeObject.transform.SetParent(gameObject.transform);
                m_Edges.Add(EdgeObject.GetComponent<Edge>());
            }
        }
    }
}

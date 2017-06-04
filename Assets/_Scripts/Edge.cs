using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {

    //member variables
    [SerializeField]
    float m_Cost;
    Node m_End;
    LineRenderer m_LineRenderer;
    
    //call on startup
    public void setup(Node end, float cost)
    {
        m_End = end;
        m_Cost = cost;
        m_LineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[2] { transform.position, end.transform.position };
        m_LineRenderer.SetPositions(points);
    }
    
    //accessors
    public float Cost
    {
        get
        {
            return m_Cost;
        }

        set
        {
            m_Cost = value;
        }
    }
    public Node End
    {
        get
        {
            return m_End;
        }

        set
        {
            m_End = value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : System.IEquatable<Node>{

    public Vector3 loc;
    public float G;
    public float H;
    public float F;
    public Node parent;

	public Node(Vector3 _loc)
    {
        loc = _loc;
        G = 0;
        H = -1;
        F = -1;
    }

    public Node(Vector3 _loc, Node _parent)
    {
        loc = _loc;
        parent = _parent;
    }

    public bool Equals(Node other)
    {
        if (this.loc == other.loc)
            return true;
        else
            return false;
    }
}

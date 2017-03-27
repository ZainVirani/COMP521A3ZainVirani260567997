using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour {

    Vector3 target;
    int professor;
    int plaque;
    int[] memory;
    Vector3[] locations;

    CoopAStar CAS;
    BuildMap map;

	// Use this for initialization
	void Start () {
        memory = new int[4];
        for (int i = 0; i < memory.Length; i++)
        {
            memory[i] = -1;
        }
        locations = new Vector3[4];
        CAS = GameObject.FindGameObjectWithTag("floor").GetComponent<CoopAStar>();
        map = GameObject.FindGameObjectWithTag("floor").GetComponent<BuildMap>();
        plaque = Random.Range(0, 6);
        assignProf(Random.Range(0, 6));

	}
	
    void assignProf(int prof)
    {
        professor = prof;
        if (checkMemory())
        {
            //target set, head to target
        }
        else
        {
            //target not set, choose a plaque
            choosePlaque();
            //target set as plaque
            headToTarget();
        }
    }

    void choosePlaque()
    {
        int newP = plaque;
        while(newP == plaque)
        {
            newP = Random.Range(0, 6);
        }
        plaque = newP;
        target = map.getPlaque(plaque);
    }

	void raycastOut () {
        //Debug.Log("ray out");
        RaycastHit hit;
        Ray left = new Ray(transform.position, Vector3.left);
        Ray right = new Ray(transform.position, Vector3.right);
        Ray up = new Ray(transform.position, Vector3.up);
        Ray down = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(left, out hit, 0.51f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(right, out hit, 0.51f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(up, out hit, 0.51f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(down, out hit, 0.51f))
        {
            handleHit(hit);
        }
    }

    void handleHit(RaycastHit hit)
    {
        if(hit.collider.tag == "plaque")
        {
            int office = hit.collider.GetComponentInParent<Plaque>().getOffice();
            
            if(office == professor)
            {
                //path to professor
                target = hit.collider.GetComponentInParent<Plaque>().getLoc();
                //store in memory

                //head to target
                headToTarget();
            }
            else
            {
                assignProf(professor);
            }
        }
        if(hit.collider.tag == "prof")
        {
            professor = hit.collider.GetComponentInParent<Professor>().getAdvice();
            int x = Random.Range(0, 2);
            if(x == 1)
            {
                idle();
                return;
            }
            else
            {
                //path to new professor's plaque
                assignProf(professor);
            }
        }
    }

    void addToMemory(Vector3 location, int profNum)
    {

    }

    void idle()
    {
        //go to random spot
        //Debug.Log("IDLE");
        //on arrival
        //wait
        //head to new prof
        assignProf(professor);
    }
    
    bool checkMemory()
    {
        /*for(int i = 0; i < 4; i++)
        {
            if(memory[i] == professor)
            {
                target = locations[i];
                return true;
            }
        }*/
        return false;
    }

    void headToTarget()
    {   
        findPathTo(target);
    }

    void traversePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = end;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        float i = 3/21;
        int j = 1;
        foreach (Node n in path)
        {
            if(j == path.Count)
            {
                StartCoroutine(moveTo(1, n.loc + new Vector3(0.5f, 0.5f), i));
            }
            else{
                StartCoroutine(moveTo(0, n.loc + new Vector3(0.5f, 0.5f), i));
            }
            j++;
            i = i + 3/21f;
            //Debug.Log("path step " + n.loc.x + " " + n.loc.y);
        }
    }

    IEnumerator moveTo(int ray, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = pos;
        if (ray == 1)
        {
            StartCoroutine(moveTo(2, transform.position, 0.5f));
        }
        if (ray == 2)
        {
            raycastOut();
        }
    }

    void findPathTo(Vector3 position) //A*, consulting CAS to find out if any space blocked at a certain point in time
    {
        /*
        OPEN set of nodes to be evaluated
        CLOSED set of nodes already evaluated
        G: dist of a node from current pos
        H: dist of a node from target pos
        F: G+H
        */
        List<Node> OPEN = new List<Node>();
        List<Node> CLOSED = new List<Node>();
        Vector3 currentLoc = transform.position - new Vector3(0.5f, 0.5f, 0);
        Vector3 targetLoc = target - new Vector3(0.5f, 0.5f, 0);
        Node start = new Node(currentLoc);
        start.G = 0;
        start.H = distance(currentLoc, targetLoc);
        start.F = start.H;
        Node current = start;
        OPEN.Add(current);

        /*
        loop
            current = node in OPEN with lowest F
            remove current from OPEN
            add current to CLOSED

            if current is target node
                return
        */
        while (true)
        {
            float lowestF = Mathf.Infinity;
            for (int i = 0; i < OPEN.Count; i++)
            {
                if (OPEN[i].F < lowestF)
                {
                    lowestF = OPEN[i].F;
                    current = OPEN[i];
                }
            }

            OPEN.Remove(current);
            CLOSED.Add(current);

            if (current.loc == targetLoc)
            {
                traversePath(start, current);
                break;
            }

            List<Node> neighbours = new List<Node>();
            //neighbours
            Node up = new Node(current.loc + new Vector3(0, 1), current);
            Node down = new Node(current.loc + new Vector3(0, -1), current);
            Node left = new Node(current.loc + new Vector3(1, 0), current);
            Node right = new Node(current.loc + new Vector3(-1, 0), current);
            Node NW = new Node(current.loc + new Vector3(-1, 1), current);
            Node NE = new Node(current.loc + new Vector3(1, 1), current);
            Node SW = new Node(current.loc + new Vector3(-1, -1), current);
            Node SE = new Node(current.loc + new Vector3(1, -1), current);
            //add appropriate neighbours
            if (currentLoc.x == 0 && currentLoc.y == 0)
            {
                neighbours.Add(up);
                neighbours.Add(left);
                neighbours.Add(NE);
            }
            else if (currentLoc.x == 28 && currentLoc.y == 20)
            {
                neighbours.Add(down);
                neighbours.Add(right);
                neighbours.Add(SW);
            }
            else if (currentLoc.x == 28 && currentLoc.y == 0)
            {
                neighbours.Add(up);
                neighbours.Add(right);
                neighbours.Add(NW);
            }
            else if (currentLoc.x == 0 && currentLoc.y == 20)
            {
                neighbours.Add(down);
                neighbours.Add(left);
                neighbours.Add(SE);
            }
            else if (currentLoc.x == 28)
            {
                neighbours.Add(up);
                neighbours.Add(down);
                neighbours.Add(right);
                neighbours.Add(NW);
                neighbours.Add(SW);
            }
            else if (currentLoc.x == 0)
            {
                neighbours.Add(up);
                neighbours.Add(down);
                neighbours.Add(left);
                neighbours.Add(NE);
                neighbours.Add(SE);
            }
            else if (currentLoc.y == 20)
            {
                neighbours.Add(down);
                neighbours.Add(left);
                neighbours.Add(right);
                neighbours.Add(SW);
                neighbours.Add(SE);
            }
            else if (currentLoc.y == 0)
            {
                neighbours.Add(up);
                neighbours.Add(left);
                neighbours.Add(right);
                neighbours.Add(NW);
                neighbours.Add(NE);
            }
            else
            {
                neighbours.Add(up);
                neighbours.Add(down);
                neighbours.Add(left);
                neighbours.Add(right);
                neighbours.Add(NW);
                neighbours.Add(NE);
                neighbours.Add(SW);
                neighbours.Add(SE);
            }

            /*
            foreach neighbour of current
                if neighbour is not traversable or neighbour is in CLOSED
                    skip to next neighbour
                
                if new path to neighbour is shorter or neighbour is not in OPEN
                    set F of neighbour
                    set parent of neighbour to current
                    if neighbour is not in OPEN
                        add neighbour to OPEN
            */
            foreach (Node n in neighbours){
                if (CAS.isBlocked((int)n.loc.x, (int)n.loc.y, 0) != 0 || CLOSED.Contains(n))
                    continue;

                float newNeighbourCost = current.G + distance(current.loc, n.loc);
                if (OPEN.Contains(n) == false || newNeighbourCost < distance(n.loc, currentLoc))
                {
                    n.G = newNeighbourCost;
                    n.H = distance(n.loc, targetLoc);
                    n.F = n.G + n.H;
                    n.parent = current;

                    if (!OPEN.Contains(n))
                        OPEN.Add(n);
                }
            }
        }
    }

    public static float distance(Vector3 a, Vector3 b)
    {
        int xDif = Mathf.Abs((int)b.x - (int)a.x);
        int yDif = Mathf.Abs((int)b.y - (int)a.y);
        if(yDif > xDif)
        {
            return (14f * xDif) + (10f * (yDif - xDif));
        }
        else
        {
            return (14f * yDif) + (10f * (xDif - yDif));
        }
    }
}
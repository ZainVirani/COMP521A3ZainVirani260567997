using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaque : MonoBehaviour {

    int office = -1;
    Vector3 location;

    public void assignOffice(int i)
    {
        office = i;
        if (i == 0)
            location = new Vector3(2.5f, 10.5f);
        else if (i == 1)
            location = new Vector3(10.5f, 18.5f);
        else if (i == 2)
            location = new Vector3(18.5f, 18.5f);
        else if (i == 3)
            location = new Vector3(26.5f, 10.5f);
        else if (i == 4)
            location = new Vector3(18.5f, 2.5f);
        else
            location = new Vector3(10.5f, 2.5f);
    }

    public int getOffice()
    {
        return office;
    }

    public Vector3 getLoc()
    {
        return location;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaque : MonoBehaviour {

    int office = -1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void assignOffice(int i)
    {
        office = i;
    }

    public int getOffice()
    {
        return office;
    }
}

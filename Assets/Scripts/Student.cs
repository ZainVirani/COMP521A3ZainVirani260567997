using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour {

    Vector3 target;
    int professor;
    int[] memory;
    Vector3[] locations;

    BuildMap map;

	// Use this for initialization
	void Start () {
        professor = Random.Range(0, 5);
        map = GameObject.FindGameObjectWithTag("floor").GetComponent<BuildMap>();
        memory = new int[4];
        locations = new Vector3[4];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void addToMemory(Vector3 location, int profNum)
    {

    }

    void idle()
    {

    }

    void headToTarget()
    {

    }

    void checkMemory()
    {

    }

    bool moveTo(Vector3 position)
    {

        return false;
    }

    void findPathTo(Vector3 position)
    {

    }
}

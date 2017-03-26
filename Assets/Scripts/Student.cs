using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour {

    Vector3 target;
    int professor;
    int[] memory;
    Vector3[] locations;

    CoopAStar CAS;
    BuildMap map;

	// Use this for initialization
	void Start () {
        memory = new int[4];
        locations = new Vector3[4];
        CAS = GameObject.FindGameObjectWithTag("floor").GetComponent<CoopAStar>();
        map = GameObject.FindGameObjectWithTag("floor").GetComponent<BuildMap>();

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
        target = map.getPlaque(Random.Range(0,6));
    }

	void raycastOut () {
        RaycastHit hit;
        Ray left = new Ray(transform.position, Vector3.left);
        Ray right = new Ray(transform.position, Vector3.right);
        Ray up = new Ray(transform.position, Vector3.up);
        Ray down = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(left, out hit, 0.5f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(right, out hit, 0.5f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(up, out hit, 0.5f))
        {
            handleHit(hit);
        }
        if (Physics.Raycast(down, out hit, 0.5f))
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

        //on arrival
        //wait
        //head to new prof
        assignProf(professor);
    }
    
    bool checkMemory()
    {
        for(int i = 0; i < 4; i++)
        {
            if(memory[i] == professor)
            {
                target = locations[i];
                return true;
            }
        }
        return false;
    }

    void headToTarget()
    {
        findPathTo(target);
        //on arrival
        //raycastOut();
    }

    void moveTo(Vector3 position)
    {
        
    }

    void findPathTo(Vector3 position)
    {

    }
}

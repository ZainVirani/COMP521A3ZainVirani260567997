using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopAStar : MonoBehaviour {

    BuildMap map;
    public int[,,] timeGrid; //0 - walkable, 1 - obstacle, 2 - agent
    int[,,] emptyTimeGrid;
    public int reservationWindow = 1;
    int planningWindow;
    GameObject studentObj;
    public int numberOfStudents = 1;
    //public int step = 0;
    bool _pause = false;
    int pausecount = 0;

    public delegate void timeStep();
    public timeStep incrementTimeStep;

    //public delegate void forcePath();
    //public forcePath forcePathfind;

    public void init () {
        if (reservationWindow == 0)
        {
            reservationWindow = 1;
        }
        //planningWindow = reservationWindow + 1;
        planningWindow = 1;
        map = GameObject.FindGameObjectWithTag("floor").GetComponent<BuildMap>();
        timeGrid = new int[29, 21, planningWindow];
        emptyTimeGrid = new int[29, 21, planningWindow];
        for(int i = 0; i < 29; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                for (int k = 0; k < planningWindow; k++)
                {
                    if (map.isBlocked(i, j) == 1){
                        timeGrid[i, j, k] = 1;
                        emptyTimeGrid[i, j, k] = 1;
                    }
                }
            }
        }
        studentObj = Resources.Load("student") as GameObject;
        int x = 0;
        int y = 0;
        for (int i = 0; i < numberOfStudents; i++)
        {
            while (isBlocked(x, y, 0) != 0)
            {
                x = Random.Range(4, 25);
                y = Random.Range(4, 17);
            }
            timeGrid[x, y, 0] = 2;
            GameObject student = Instantiate(studentObj);
            student.transform.position = new Vector3(x + 0.5f, y + 0.5f);
            student.name = "student" + i;
            student.GetComponent<Student>().assignNumber(i);
            incrementTimeStep += student.GetComponent<Student>().incrementStep;
            //forcePathfind += student.GetComponent<Student>().forcePathfind;
        }
        InvokeRepeating("callDelegate", 0, 0.1f);
    }

    public int isBlocked(int i, int j, int k)
    {
        //if (k == -1)
            //k = step;
        //Debug.Log(step + " " + i + " " + j + " " + k);
        return timeGrid[i, j, k];
    }

    public bool reserveSpace(int i, int j, int k)
    {
        //return true;
        if (isBlocked(i, j, k) != 0) //&& isBlocked(i, j, k + 1) != 0)
            return false;
        else
        {
            timeGrid[i, j, k] = 2;
            //timeGrid[i, j, k + 1] = 2;
            return true;
        }
    }
    
    public int getPW()
    {
        return reservationWindow;
    }

    public void pause()
    {
        pausecount++;
        _pause = true;
    }

    public void unpause()
    {
        pausecount--;
        if (pausecount == 0)
            _pause = false;
    }

    public void callDelegate()
    {
        if (!_pause)
        {
            /*step++;
            if (step == planningWindow - 1)
            {
                Debug.Log("time grid reset");
                step = 0;
                for (int i = 0; i < 29; i++)
                {
                    for (int j = 0; j < 21; j++)
                    {
                        for (int k = 0; k < planningWindow; k++)
                        {
                            timeGrid[i, j, k] = emptyTimeGrid[i, j, k];
                        }
                    }
                }

            }*/
            incrementTimeStep();
        }
    }
}

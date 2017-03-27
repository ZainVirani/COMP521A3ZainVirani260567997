using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CoopAStar : MonoBehaviour {

    BuildMap map;
    public int[,,] timeGrid; //0 - walkable, 1 - obstacle, 2 - agent
    public int planningWindow = 1;
    GameObject studentObj;
    public int numberOfStudents = 1;

    public void init () {
        map = GameObject.FindGameObjectWithTag("floor").GetComponent<BuildMap>();
        timeGrid = new int[29, 21, planningWindow];
        for(int i = 0; i < 29; i++)
        {
            for (int j = 0; j < 21; j++)
            {
                for (int k = 0; k < planningWindow; k++)
                {
                    if (map.isBlocked(i, j) == 1){
                        timeGrid[i, j, k] = 1;
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
        }
    }
	
    public int isBlocked(int i, int j, int k)
    {
        return timeGrid[i, j, k];
    }
}

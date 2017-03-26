using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMap : MonoBehaviour {

    TextAsset coordinates;
    GameObject wallObj;
    GameObject wallParent;
    GameObject professorObj;
    GameObject plaqueObj;
    int[,] blocked = new int[29, 21];
    Vector3[] offices = new Vector3[6];
    Vector3[] plaques = new Vector3[6];
    
	void Start () {

        wallObj = Resources.Load("wall") as GameObject;
        wallParent = GameObject.FindGameObjectWithTag("walls");
        coordinates = (TextAsset)Resources.Load("coordinates");
        professorObj = Resources.Load("professor") as GameObject;
        plaqueObj = Resources.Load("plaque") as GameObject;
        string[] lines = coordinates.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
        int x = 0;
        int y = 0;
        offices[0] = new Vector3(2.5f, 9.5f);
        blocked[2, 9] = 1;
        offices[1] = new Vector3(9.5f, 18.5f);
        blocked[9, 18] = 1;
        offices[2] = new Vector3(17.5f, 18.5f);
        blocked[17, 18] = 1;
        offices[3] = new Vector3(26.5f, 11.5f);
        blocked[26, 11] = 1;
        offices[4] = new Vector3(19.5f, 2.5f);
        blocked[19, 2] = 1;
        offices[5] = new Vector3(11.5f, 2.5f);
        blocked[11, 2] = 1;
        plaques[0] = new Vector3(5.3f, 10.5f, 0.01f);
        plaques[1] = new Vector3(10.5f, 15.7f, 0.01f);
        plaques[2] = new Vector3(18.5f, 15.7f, 0.01f);
        plaques[3] = new Vector3(23.7f, 10.5f, 0.01f);
        plaques[4] = new Vector3(18.5f, 5.3f, 0.01f);
        plaques[5] = new Vector3(10.5f, 5.3f, 0.01f);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            int.TryParse(values[1], out x);
            int.TryParse(values[0], out y);
            blocked[x, y] = 1;
            GameObject wall = Instantiate(wallObj);
            wall.transform.position = new Vector3(x + 0.5f, y + 0.5f);
            wall.transform.SetParent(wallParent.transform);
        }

        for (int i = 0; i < 6; i++)
        {
            GameObject professor = Instantiate(professorObj);
            professor.transform.position = offices[i];
            professor.name = "professor" + i;
            professor.GetComponent<Professor>().assignOffice(i);
        }

        for (int i = 0; i < 6; i++)
        {
            GameObject plaque = Instantiate(plaqueObj);
            plaque.transform.position = plaques[i];
            plaque.name = "plaque" + i;
            plaque.GetComponent<Plaque>().assignOffice(i);
        }
        this.GetComponent<CoopAStar>().init();
    }
	
	public int isBlocked(int x, int y)
    {
        //Debug.Log(blocked[x, y] + " " + x + " " + y);
        return blocked[x, y];
    }

    public Vector3 getPlaque(int i)
    {
        if (i == 0)
            return new Vector3(4.5f, 10.5f);
        else if (i == 1)
            return new Vector3(10.5f, 16.5f);
        else if (i == 2)
            return new Vector3(18.5f, 16.5f);
        else if (i == 3)
            return new Vector3(24.5f, 10.5f);
        else if (i == 4)
            return new Vector3(18.5f, 4.5f);
        else
            return new Vector3(10.5f, 4.5f);
    }
}

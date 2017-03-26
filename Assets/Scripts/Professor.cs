using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Professor : MonoBehaviour {

    int office = -1;

    public void assignOffice(int i)
    {
        office = i;
    }

    public int getAdvice()
    {
        int x = Random.Range(0, 6);
        while(x == office)
        {
            x = Random.Range(0, 6);
        }
        return x;
    }
}

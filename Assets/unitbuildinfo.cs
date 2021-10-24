using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitbuildinfo : MonoBehaviour
{
    public int reqtime = 10; //초 단위

    public GameObject resultobj;

    public bool create(int x, int y, int team, unitbuilder builder)
    {
        if(resultobj == null)
        {
            return false;
        }


        GameObject obj = Instantiate(resultobj, new Vector3(x, y, 0), Quaternion.identity);

        Unit u = obj.GetComponent<Unit>();
        if(u != null)
        {
            u.team = team;

        }

        //builder 처리
        if(builder != null)
        {

        }

        return true;
    }

}

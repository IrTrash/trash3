using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitbuildinfo : MonoBehaviour
{
    public int reqtime = 10; //초 단위
    public int cost = 1, tier = 1, buildlimit = 5; //비용, 티어(나도모름), 패턴에서의 최대 생산수

    public GameObject resultobj;

    public List<Unit> builded = new List<Unit>();

    public Unit create(int x, int y, int team, unitbuilder builder)
    {
        if(resultobj == null)
        {
            return null;
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

        return u;
    }

}

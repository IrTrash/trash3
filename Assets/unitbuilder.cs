using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitbuilder : MonoBehaviour
{
    public List<unitbuildinfo> selectlist = new List<unitbuildinfo>(), waiting = new List<unitbuildinfo>();
    public unitbuildinfo current;
    public int currenttime = 0;


    private void Start()
    {
        unitbuildinfo[] ubs = gameObject.GetComponents<unitbuildinfo>();
        if(ubs != null)
        {
            foreach(unitbuildinfo ub in ubs)
            {
                if(!selectlist.Contains(ub))
                {
                    selectlist.Add(ub);
                }
            }
        }


    }


    void FixedUpdate()
    {
        if(current != null)
        {
            if(--currenttime <= 0)
            {
                Unit u = gameObject.GetComponent<Unit>();
                if (u != null)
                {
                    int team = u.team;
                    int x = (int)transform.position.x, y = (int)transform.position.y;
                    current.create(x, y, team, this);
                }
                else
                {
                    //ㅁㄴㅇㄹ
                }

                current = null;
            }
        }
        else
        {
            if(waiting.Count > 0)
            {
                current = waiting[0];
                if(waiting.Count > 1)
                {
                    waiting = new List<unitbuildinfo>(waiting.GetRange(1, waiting.Count - 1));
                    currenttime = current.reqtime;
                }
                else
                {
                    waiting.Clear();
                }
                
            }
        }

    }


    public bool request(int index)
    {
        if(index < 0 || index >= selectlist.Count || !available())
        {
            return false;
        }

        if(waiting.Count < 1)
        {
            if(current == null)
            {                
                current = selectlist[index];
                currenttime = current.reqtime;
            }
            else
            {
                waiting.Add(selectlist[index]);
            }
        }
        else
        {
            waiting.Add(selectlist[index]);
        }

        return true;
    }


    public bool available()
    {
        return selectlist.Count > 0 && waiting.Count < 5;
    }
}

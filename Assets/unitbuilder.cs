using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitbuilder : MonoBehaviour
{
    public List<unitbuildinfo> selectlist = new List<unitbuildinfo>(), waiting = new List<unitbuildinfo>();
    public unitbuildinfo current;
    public int currenttime = 0;

    public int resource = 1, resourcemax = 15;

    public List<Unit> builded = new List<Unit>();
    public int buildlimit = 8;


    public unitpattern up;

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

        system sys = system.getsystem();
        if(sys != null)
        {
            sys.ublist.Add(this);
        }

        if(up == null)
        {
            up = gameObject.GetComponent<unitpattern>();
        }
    }

    private void OnDestroy()
    {
        system sys = system.getsystem();
        if (sys != null)
        {
            sys.ublist.Remove(this);
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

                    Unit ru = current.create(x, y, team, this);
                    if(ru != null)
                    {
                        ru.mybuilder = this;
                        current.builded.Add(ru);
                        ru.mybuildinfo = current;
                        builded.Add(ru);
                        if (up != null)
                        {
                            if (!up.myunits.Contains(ru))
                            {
                                up.myunits.Add(ru);
                            }
                            ru.ownerpattern = up;
                        }
                    }
                    

                    
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


    public int unitcount => builded.Count;

    public bool request(int index)
    {
        if(index < 0 || index >= selectlist.Count || !available() || unitcount >= buildlimit)
        {
            return false;
        }

        if(selectlist[index].builded.Count >= selectlist[index].buildlimit)

        if(resource < selectlist[index].cost)
        {
            return false;
        }
        resource -= selectlist[index].cost;

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


    /* //필요할 때만 만듥자구
    public void cancel(int index)
    {
        
    }

    public void cancelall()
    {

    }
    */



    public bool available()
    {
        return selectlist.Count > 0 && waiting.Count < 5;
    }

    public int countwaiting(unitbuildinfo ubi)
    {
        if(ubi == null)
        {
            return 0;
        }

        int r = 0;
        foreach(unitbuildinfo w in waiting)
        {
            if(w.resultobj == ubi.resultobj)
            {
                r++;
            }

        }
        return r;
    }
}

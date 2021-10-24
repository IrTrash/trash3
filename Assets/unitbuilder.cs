using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitbuilder : MonoBehaviour
{
    public List<unitbuildinfo> selectlist = new List<unitbuildinfo>(), waiting = new List<unitbuildinfo>();
    public unitbuildinfo current;
    


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

        }
        else
        {
            if(waiting.Count > 0)
            {

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

            }
            else
            {

            }
        }
        else
        {

        }
    }


    public bool available()
    {
        return selectlist.Count > 0 && waiting.count < 5;
    }
}

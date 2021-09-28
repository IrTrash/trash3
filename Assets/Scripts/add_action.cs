using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class add_action : MonoBehaviour
{
    public Unit.action._type type;
    public int[] i;
    public float[] f;
    public string[] s;

    public Unit dest;


    public void Start()
    {
        if(dest == null)
        {
            dest = gameObject.GetComponent<Unit>();
        }

        proc();
    }


    private void proc()
    {
        if(dest == null)
        {
            return; 
        }


        dest.addaction(type, i, f, s);
        
        Destroy(this);
    }
}

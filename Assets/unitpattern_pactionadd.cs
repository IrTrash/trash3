using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitpattern_pactionadd : MonoBehaviour
{
    public unitpattern.paction.types type = 0;
    public List<int> i;
    public List<float> f;
    public List<string> s;

    public unitpattern up;
    private void Update()
    {
        if(type != 0)
        {
            if(up == null)
            {
                up = gameObject.GetComponent<unitpattern>();
            }       
            
            if(execute(up))
            {
                Destroy(this);
            }
        }
        else
        {
            Destroy(this);
        }
    }


    public bool execute(unitpattern dest)
    {
        if(dest == null)
        {
            return true;
        }


        return dest.pactionrequest(type, i.ToArray(), f.ToArray(), s.ToArray()); //실패해도 남아서 계속 하게 될거임. 그냥 한번만 하고 그만하게할까        
    }

}

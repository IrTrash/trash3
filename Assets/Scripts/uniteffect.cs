using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uniteffect : MonoBehaviour
{
    public enum _type : int
    {
        damage = 1, stun = 2, heal = 3
    }
    public _type type;
    public bool continuous = false;

    public int[] i;
    public float[] f;

    public void applyonce(Unit dest) //단순하게 1차 적용
    {
        if(dest == null)
        {
            return;
        }

        switch (type)
        {
            case _type.damage:
                {
                    if(i == null)
                    {
                        break;
                    }
                    dest.hp -= i[0];
                }
                break;
        }

    }

    public void applycontinuos(Unit dest)
    {

    }
    
}

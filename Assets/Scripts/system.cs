using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    //tile 사이즈는 1로 통용
    

    public static int gridx(float x)
    {
        float distance = Mathf.Abs(x - (int)x);

        int r;
        if(distance >= 0.5)
        {
            if(x >= 0)
            {
                r = (int)x + 1;
            }
            else
            {
                r = (int)x - 1;
            }
        }
        else
        {
            r = (int)x;
        }

        return r;
    }


    public static int gridy(float y)
    {
        float distance = Mathf.Abs(y - (int)y);

        int r = (int)y;
        if (distance >= 0.5)
        {
            if (y >= 0)
            {
                r += 1;
            }
            else
            {
                r -=  1;
            }
        }
        

        return r;
    }
}

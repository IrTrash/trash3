using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public enum _state : int
    {
        available = 0, charging, cooling
    }
    public _state state = _state.available;
    public int t = 0, charge = 10, casting = 10, cooldown = 10, prange = 3;

    public GameObject resultobj;
    public float direction;

    private void FixedUpdate()
    {
        switch (state)
        {
            case _state.charging:
                if(t > 0)
                {
                    t--;
                }
                else
                {


                    state = _state.cooling;
                    t = cooldown;
                }
                break;

            case _state.cooling:
                if(t > 0)
                {
                    t--;
                }
                else
                {
                    state = _state.available;
                }
                break;
        }
    }
}

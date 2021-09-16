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

    float dx, dy;
    public List<uniteffect> effectlist = new List<uniteffect>();
    public float speed = 0;

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


    public bool request(float dx, float dy) //사용요철
    {
        if(state != _state.available || resultobj == null)
        {
            return false;
        }


        this.dx = dx;
        this.dy = dy;
        state = _state.charging;
        t = charge;

        return true;
    }

    public void createresult(float x, float y, float dx, float dy)
    {
        if(resultobj == null)
        {
            return;
        }

        GameObject obj = Instantiate(resultobj, new Vector3(x, y, 0), Quaternion.identity);
        Projectile pj = obj.GetComponent<Projectile>();

        if(pj != null)
        {
            pj.direction = Mathf.Atan2((dy - y), (dx - x));
            if(speed > 0) //초기값 : 0
            {
                pj.speed = speed;
            }
            Unit u = gameObject.GetComponent<Unit>();
            if(u != null)
            {
                pj.team = u.team;                
            }
        }
    }
}

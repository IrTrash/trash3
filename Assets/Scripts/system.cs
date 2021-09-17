using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    //tile 사이즈는 1로 통용

    public const int left = -50, top = 50, right = 50, bottom = -50;

    Astar astar = new Astar(left, bottom, right, top);

    private void Start() //테스트
    {
    }


    private void FixedUpdate()
    {
        blockupdate();   
    }


    public static system getsystem()
    {
        GameObject systemobj = GameObject.Find("System");
        if(systemobj == null)
        {
            return null;
        }

        return systemobj.GetComponent<system>();
    }


    public static int gridx(float x)
    {
        float distance = Mathf.Abs(x - (int)x);

        int r;
        if (distance >= 0.5)
        {
            if (x >= 0)
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
                r -= 1;
            }
        }


        return r;
    }

    public static Vector2 move(float x, float y, float x2, float y2, float distance)
    {
        if(distance <= 0)
        {
            return new Vector2(x, y);
        }

        float d = Mathf.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));
        if(Mathf.Approximately(d,0))
        {
            return new Vector2(x, y);
        }

        float ratio = distance/d;
        return new Vector2(x + (x2 - x) * ratio, y + (y2 - y) * ratio);
    }

    public static bool isin(int x, int y, int x1, int y1, int x2, int y2) => (x - x1) * (x - x2) <= 0 && (y - y1) * (y - y2) <= 0;
    
    void blockupdate()
    {

    }


    public Unit._direction[] getway(int x, int y, int dx, int dy)
    {
        if (!isin(x, y, left, bottom, right, top) || !isin(dx, dy, left, bottom, right, top))
        {
            return null;
        }


        node[] nodes = astar.getway(x, y, dx, dy);
        if(nodes == null)
        {
            return null;
        }

        List<Unit._direction> r = new List<Unit._direction>();

        for(int i = 0; i < nodes.Length - 1; i++)
        {
            int nx1 = nodes[i].gx, ny1 = nodes[i].gy, nx2 = nodes[i + 1].gx, ny2 = nodes[i + 1].gy;

            if (nx1 != nx2)
            {
                r.Add(nx1 > nx2 ? Unit._direction.left : Unit._direction.right); ;
            }
            else
            {
                r.Add(ny1 > ny2 ? Unit._direction.down : Unit._direction.up); 
            }

        }

        return r.ToArray();
    }
}

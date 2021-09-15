using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float direction = 0, speed = 2, hitsize = 0.5f;
    public int life = 40, team, hitdelay = 1, hitlife = 1;
    public List<uniteffect> effectlist = new List<uniteffect>();
    public List<(Unit, int)> hitlist = new List<(Unit, int)>();


    public Rigidbody2D rb;

    private void Start()
    {
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
    }


    private void FixedUpdate()
    {
        if (life-- <= 0)
        {
            Destroy(gameObject);
        }

        if (speed > 0)
        {
            float deltaspeed = speed * Time.fixedDeltaTime;
            transform.position = new Vector3(transform.position.x + deltaspeed * Mathf.Cos(direction), transform.position.y + deltaspeed * Mathf.Sin(direction), transform.position.z);
            rb.rotation = Mathf.Rad2Deg * direction - 90;
        }


        //피격 리스트 처리
        List<(Unit, int)> remove = new List<(Unit, int)>();
        List<Unit> units = new List<Unit>();
        for (int n = 0; n < hitlist.Count; n++)
        {
            (Unit, int) v = hitlist[n];
            if (v.Item1 == null || --v.Item2 < 1 )
            {
                remove.Add(v);
            }
            else
            {
                units.Add(v.Item1);
            }
        }
        foreach ((Unit, int) v in remove)
        {
            hitlist.Remove(v);
        }


        //피격 판정
        Collider2D[] cds = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - hitsize, transform.position.y - hitsize), new Vector2(transform.position.x + hitsize, transform.position.y + hitsize));
        if(cds != null)
        {
            foreach(Collider2D cd in cds)
            {
                GameObject o = cd.gameObject;
                if(o.tag == "Unit")
                {
                    Unit u = o.GetComponent<Unit>();
                    if(u != null && !units.Contains(u))
                    {
                        if(u.team == team)
                        {
                            continue;
                        }

                        foreach(uniteffect e in effectlist)
                        {
                            if(e.continuous)
                            {
                                e.applycontinuos(u);
                            }
                            else
                            {
                                e.applyonce(u);
                            }
                        }

                        hitlist.Add((u, hitdelay));

                        Debug.Log("hit");
                        if (--hitlife < 1)
                        {
                            Destroy(gameObject);
                            return;
                        }                       
                    }
                    
                }
            }
        }
    }
}

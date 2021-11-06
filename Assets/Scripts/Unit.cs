using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit : MonoBehaviour
{
    public enum _type : int
    {
        unit = 1, structure
    }
    public _type type = _type.unit;

    public int team = 0, maxhp = 10, hp;
    public float speed = 1;

    public enum _direction : int
    {
        left = 1, up, right, down
    }
    public _direction direction = _direction.down;

    public enum _state : int
    {
        idle = 1, move, charge, attack
    }
    public _state state = _state.idle;
    public int statetime = 0;

    public class action
    {
        public action(_type t, int[] ilist, float[] flist, string[] slist)
        {
            type = t;
            if (ilist != null)
            {
                i = (int[])ilist.Clone();
            }
            if (flist != null)
            {
                f = (float[])flist.Clone();
            }
            if (slist != null)
            {
                s = (string[])slist.Clone();
            }
        }

        public enum _type : int
        {
            move_1tile = 1, move_dest, stop, wait, useweapon_pos, useweapon_destunit, approachdest
        }

        public _type type;
        public bool started = false;
        public int[] i;
        public float[] f;
        public string[] s;

        public action pushed;
    };
    action currentaction;
    List<action> actionlist = new List<action>();
    public bool canaction = true;



    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;

        if (gameObject.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D b2d = gameObject.AddComponent<BoxCollider2D>();
            b2d.size = new Vector2(0.5f, 0.5f);
        }

        unitpattern up = gameObject.GetComponent<unitpattern>();
        if (up == null)
        {
            up = gameObject.AddComponent<unitpattern>();

        }
    }


    private void FixedUpdate()
    {
        proc();
    }

    void proc()
    {
        if (hp <= 0)
        {
            Destroy(gameObject); //death. 따로 메소드로 해놔야하나? 
        }

        //행동 가능 여부
        if (state != _state.idle)
        {
            if (statetime-- <= 0)
            {
                state = _state.idle;
            }
        }

        actionproc();


        //이동 관련 처리
        moveproc();
    }

    void actionproc()
    {
        if (!canaction)
        {
            return;
        }

        if (currentaction != null)
        {
            if (executeaction(currentaction))
            {
                currentaction = null;
            }
        }
        else
        {
            if (actionlist.Count > 0)
            {
                currentaction = actionlist[actionlist.Count - 1];
                if (actionlist.Count > 1)
                {
                    actionlist = new List<action>(actionlist.GetRange(0, actionlist.Count - 1));
                }
                else
                {
                    actionlist.Clear();
                }
            }
        }
    }

    void moveproc()
    {
        //이동 가능 여부(아직 미구현)


    }




    bool executeaction(action dest)
    {
        if (dest == null)
        {
            return true;
        }

        if (dest.pushed != null)
        {
            if (executeaction(dest.pushed))
            {
                dest.pushed = null;
            }


            return false;
        }


        bool complete = false; //해당 액션(dest)이 완료되었는지(실행완료했나? 또는 더이상 수행할 이유가 있나없나 등)
        switch (dest.type)
        {
            case action._type.move_1tile: //이런것들은 위치가 강제적으로 변경되거나하면 취소되어야 함
                {
                    if (!dest.started)
                    {
                        if (dest.i == null)
                        {
                            complete = true;
                            break;
                        }

                        int dx = system.gridx(x), dy = system.gridy(y);
                        switch ((_direction)dest.i[0])
                        {
                            case _direction.left:
                                {
                                    dx -= 1;
                                }
                                break;

                            case _direction.right:
                                {
                                    dx += 1;
                                }
                                break;

                            case _direction.up:
                                {
                                    dy += 1;
                                }
                                break;

                            case _direction.down:
                                {
                                    dy -= 1;
                                }
                                break;
                        }


                        dest.i = new int[] { dest.i[0], dx, dy };
                    }
                    else
                    {
                        if (!canmove)
                        {
                            complete = false;
                            break;
                        }

                        float deltaspeed = speed * Time.fixedDeltaTime;
                        float distance = Mathf.Sqrt((dest.i[1] - x) * (dest.i[1] - x) + (dest.i[2] - y) * (dest.i[2] - y));
                        if (distance < deltaspeed)
                        {
                            transform.position = new Vector3(system.gridx(x), system.gridy(y), transform.position.z);
                            complete = true;
                            break;
                        }
                        else
                        {
                            transform.position = system.move(x, y, dest.i[1], dest.i[2], deltaspeed);
                            direction = (_direction)dest.i[0];
                            state = _state.move;
                            statetime = 1;
                        }
                    }
                }
                break;

            case action._type.useweapon_pos:
                {
                    if (!dest.started)
                    {
                        //i : 사용할 wp index
                        //f : 대상 위치
                        if (dest.i == null)
                        {
                            complete = true;
                            break;
                        }

                        if (dest.f == null)
                        {
                            complete = true;
                            break;
                        }
                        else if (dest.f.Length < 2)
                        {
                            complete = true;
                            break;
                        }
                    }
                    else
                    {
                        weapon[] wps = gameObject.GetComponents<weapon>();
                        if (wps == null)
                        {
                            complete = true;
                            break;
                        }
                        else if (wps.Length <= dest.i[0])
                        {
                            complete = true;
                            break;
                        }

                        weapon wp = wps[dest.i[0]];
                        if (wp.request(dest.f[0], dest.f[1]))
                        {
                            complete = true;
                        }
                        else
                        {
                            //그대로 끝낼까 그냥 대기하도록할까....

                        }
                    }
                }
                break;


            case action._type.move_dest:
                {
                    if (!dest.started)
                    {
                        if (dest.i == null)
                        {
                            complete = true;
                            break;
                        }
                        else if (dest.i.Length < 2)
                        {
                            complete = true;
                            break;
                        }


                        system sys = system.getsystem();
                        if (sys == null)
                        {
                            complete = true;
                            break;
                        }
                        _direction[] directions = sys.getway(system.gridx(x), system.gridy(y), dest.i[0], dest.i[1]);
                        if (directions == null)
                        {
                            complete = true;
                            break;
                        }

                        List<int> i2 = new List<int>
                        {
                            directions.Length
                        };
                        foreach (_direction d in directions)
                        {
                            i2.Add((int)d);
                        }

                        dest.i = (int[])i2.ToArray().Clone();
                    }
                    else
                    {
                        if (dest.i[0] <= 0)
                        {
                            complete = true;
                            break;
                        }


                        dest.pushed = new action(action._type.move_1tile, new int[] { dest.i[dest.i.Length - dest.i[0]--] }, null, null);
                        complete = false;
                        break;
                    }
                }
                break;

            case action._type.approachdest:
                {
                    if (!dest.started)
                    {
                        if (dest.i == null)
                        {
                            complete = true;
                            break;
                        }
                        else if (dest.i.Length < 3)
                        {
                            complete = true;
                            break;
                        }


                        system sys = system.getsystem();
                        if (sys == null)
                        {
                            complete = true;
                            break;
                        }
                        _direction[] directions = sys.getway(system.gridx(x), system.gridy(y), dest.i[0], dest.i[1]);
                        if (directions == null)
                        {
                            complete = true;
                            break;
                        }

                        dest.i[2] = Mathf.Min(directions.Length, dest.i[2]);

                        List<int> i2 = new List<int>
                        {
                            dest.i[2]
                        };

                        for (int n = 0; n < dest.i[2]; n++)
                        {
                            i2.Add((int)directions[n]);
                        }

                        dest.i = (int[])i2.ToArray().Clone();
                    }
                    else
                    {
                        if (dest.i[0] <= 0)
                        {
                            complete = true;
                            break;
                        }


                        dest.pushed = new action(action._type.move_1tile, new int[] { dest.i[dest.i.Length - dest.i[0]--] }, null, null);
                        complete = false;
                        break;
                    }
                }
                break;
        }


        if (!dest.started)
        {
            dest.started = true;
        }


        return complete;
    }


    public float x => gameObject.transform.position.x;
    public float y => gameObject.transform.position.y;

    public int gridx => system.gridx(x);
    public int gridy => system.gridx(y);

    public bool canmove => state == _state.idle && speed > 0;



    public bool actionavailable => canaction && currentaction == null && actionlist.Count < 1;



    public bool addaction(action dest)
    {
        if (dest == null)
        {
            return false;
        }


        actionlist.Add(dest); //얕은 복사 해도 되나?

        return true;
    }

    public bool addaction(action._type t, int[] i, float[] f, string[] s) => addaction(new action(t, i, f, s));

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit : MonoBehaviour
{
    public enum _type : int
    { 
        unit =1 , structure
    }
    public _type type = _type.unit;

    public int maxhp = 10, hp;
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

    public class action
    {
        public action(_type t, int[] ilist,float[] flist, string[] slist)
        {
            type = t;
            if(ilist != null)
            {
                i = (int[])ilist.Clone();
            }
            if(flist != null)
            {
                f = (float[])flist.Clone();
            }
            if(slist != null)
            {
                s = (string[])slist.Clone();
            }
        }

        public enum _type : int
        { 
            move_1tile = 1, move_dest , stop, wait, useweapon_pos, useweapon_destunit, 
        }

        public _type type;
        public bool started = false;
        public int[] i;
        public float[] f;
        public string[] s;


    };
    action currentaction;
    List<action> actionlist = new List<action>();
    public bool canaction = true;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxhp;

        Debug.Log(" (4 , 2) -> (1 ,-3) 3만큼 " + system.move(4,2,1,-3,3));


        currentaction = new action(action._type.move_1tile, new int[] {(int)_direction.left }, null, null);
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

        //행동 가능 여부는 먼저 행동가능하다고 해놓고 그 다음 불가능하게 하는 요소들이 있는지 체크 후 처리 하는 식으로
        canaction = true;
        //행동 불가능한 요소들 체크

        actionproc();


        //이동 관련 처리
        moveproc();
    }

    void actionproc()
    {
        if(!canaction)
        {
            return;
        }

        if(currentaction != null)
        {
            if(executeaction(currentaction))
            {
                currentaction = null;
            }
        }
        else
        {
            if(actionlist.Count > 0)
            {
                currentaction = actionlist[0];
                if(actionlist.Count > 1)
                {
                    actionlist = new List<action>(actionlist.GetRange(1, actionlist.Count - 1));
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
        if(dest == null )
        {
            return true;
        }

        bool complete = false; //해당 액션(dest)이 완료되었는지(실행완료했나? 또는 더이상 수행할 이유가 있나없나 등)
        switch (dest.type)
        {
            case action._type.move_1tile : //이런것들은 위치가 강제적으로 변경되거나하면 취소되어야 함
                {
                    if(!dest.started)
                    {
                        if (dest.i == null)
                        {
                            complete = true;
                            break;
                        }
                        dest.i = new int[] { dest.i[0], system.gridx(x), system.gridy(y) };

                    }
                    else
                    {
                        if(!canmove)
                        {
                            complete = false;
                            break;
                        }

                        float deltaspeed = speed * Time.fixedDeltaTime;
                        float gx = system.gridx(x), gy = system.gridy(y);
                        //시작타일과 현재타일이 다르면 이 액션은 유효하지않게됨(1타일만이동하므로)                                                                                                
                        if( (gx != dest.i[1] || gy != dest.i[2])  )
                        {                            
                            if(Mathf.Abs(gx - x) + Mathf.Abs(gy - y) <= deltaspeed)
                            {
                                transform.position = new Vector3(gx, gy, transform.position.z);
                                complete = true;
                            }
                            else
                            {
                                Vector2 v = system.move(x, y, gx, gy, deltaspeed);
                                transform.position = new Vector3(v.x, v.y, transform.position.z);
                            }
                            
                        }
                        moveonce((_direction)dest.i[0]);

                    }
                }
                break;

                
        }


        if(!dest.started)
        {
            dest.started = true;
        }


        return complete;
    }


    public float x => gameObject.transform.position.x;
    public float y => gameObject.transform.position.y;

    public bool canmove => state == _state.idle && speed > 0;

    public bool moveonce(_direction direc)
    {
        //speed로 direc방향으로 한번 이동(deltaspeed). 

        if(!canmove || speed <= 0) //막힌 곳 처리는 나중에
        {
            return false;
        }


        float deltaspeed = speed * Time.fixedDeltaTime;
        float nx = x, ny = y;
        switch (direc)
        {
            case _direction.left:
                {
                    nx -= deltaspeed;
                }
                break;

            case _direction.up:
                {
                    ny += deltaspeed;
                }
                break;

            case _direction.right:
                {
                    nx += deltaspeed;
                }
                break;

            case _direction.down:
                {
                    ny -= deltaspeed;
                }
                break;
        }
        transform.position = new Vector3(nx, ny, transform.position.z);
        Debug.Log(nx + " ," + ny);

        return true;
    }

}

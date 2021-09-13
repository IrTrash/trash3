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

        Debug.Log("11.4 -> " + system.gridy(11.4f));
        Debug.Log("-7.6 -> " + system.gridy(-7.6f));
        Debug.Log("5.8 -> " + system.gridy(5.8f));
        Debug.Log("-3.2 -> " + system.gridy(-3.2f));
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
            case action._type.move_1tile :
                {
                    if(!dest.started)
                    {

                    }
                    else
                    {
                        //이동 가능 여부 체크해야하는데 아직 그런거 구현도안되서 ㅎㅎ(20210913)


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
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitpattern : MonoBehaviour
{
    //자동 action
    public enum _type : int
    {
        normalunit = 1, structure
    }
    public _type type;
    public Unit u;


    private void Start()
    {
        if (u == null)
        {
            u = gameObject.GetComponent<Unit>();
        }

        switch (u.type)
        {
            case Unit._type.unit:
                {
                    type = _type.normalunit;
                }
                break;

            case Unit._type.structure:
                {
                    type = _type.structure;
                }
                break;
        }

    }


    private void FixedUpdate()
    {
        proc();
    }

    void proc()
    {
        if (u == null)
        {
            return;
        }

        switch (type)
        {
            case _type.normalunit:
                {
                    normalunitproc();
                }
                break;

            case _type.structure:
                {

                }
                break;
        }
    }


    public int searchrange = 2, tracerange = 3; //tracerange < searchrange 이면 타겟을 찾아도 바로 유효하지 않아 의미없어질수있음
    public Unit target;
    weapon selectedwp;
    int wpindex = 0;

    void normalunitproc()
    {
        //paction


        if (!u.actionavailable)
        {
            return;
        }


        if (target != null)
        {
            //타겟 유효성
            if (!system.isin(target.x, target.y, u.gridx - tracerange, u.gridy - tracerange, u.gridx + tracerange, u.gridy + tracerange))
            {
                target = null;
            }
        }


        bool canattack = true;
        if (target == null)
        {
            //find enemy
            Unit[] units = system.findunit(system.gridx(u.x) - searchrange, system.gridy(u.y) - searchrange, system.gridx(u.x) + searchrange, system.gridy(u.y) + searchrange);

            List<Unit> enemies = new List<Unit>();
            if (units != null)
            {
                foreach (Unit unit in units)
                {
                    if (unit == u)
                    {
                        continue;
                    }
                    else if (unit.team == u.team) //아군일 경우는 아직 생각한게 없음 ㅠ
                    {
                        continue;
                    }

                    enemies.Add(unit);
                }
            }

            if (enemies.Count > 0)
            {
                target = enemies[UnityEngine.Random.Range(0, enemies.Count)];
            }
            else
            {
                canattack = false;
            }

        }
        else //target exist in range
        {
            //if in attack range
            if (selectedwp == null) // wp is not selected
            {
                //wp select
                weapon[] weapons = gameObject.GetComponents<weapon>();
                if (weapons != null)
                {
                    List<weapon> wplist = new List<weapon>();
                    foreach (weapon wp in weapons)
                    {
                        if (wp.state == weapon._state.available)
                        {
                            wplist.Add(wp);
                        }
                    }

                    if (wplist.Count > 0)
                    {
                        List<int> wppr = new List<int>();
                        int pr = 0;
                        foreach (weapon wp in weapons)
                        {
                            pr += wp.priority;
                            wppr.Add(wp.priority);
                        }

                        pr = UnityEngine.Random.Range(0, pr) + 1;
                        for (int n = 0; n < wplist.Count; n++)
                        {
                            pr -= wplist[n].priority;
                            if (pr <= 0)
                            {
                                //당선
                                selectedwp = wplist[n];
                                wpindex = n;
                                break;
                            }
                        }
                    }
                    else
                    {
                        canattack = false;
                    }
                }
                else
                {
                    canattack = false;
                }

            }
            else
            {
                //사용 후  wp 초기화

                float distance = Mathf.Sqrt((target.x - u.x) * (target.x - u.x) + (target.y - u.y) * (target.y - u.y));
                if (distance <= selectedwp.patternrange)
                {
                    u.addaction(Unit.action._type.useweapon_pos, new int[] { wpindex }, new float[] { target.x, target.y }, null);
                    selectedwp = null;
                    wpindex = -1;
                }
                else
                {
                    //접근
                    u.addaction(Unit.action._type.approachdest, new int[] { target.gridx, target.gridy, 1 }, null, null);
                }
            }
        }

        if (!canattack) //공격할수없거나 할 일이없을때
        {

        }


    }


    //건물용 변수들
    public List<Unit> myunits, defenders, attackers, enemies;
    public int defendrange = 1;
    void structureproc()
    {
        if (!u.actionavailable)
        {
            return;
        }


        if(target == null)
        {
            //find target
        }
        else
        {
            //target vertification
            if (!system.isin(target.x, target.y, u.gridx - tracerange, u.gridy - tracerange, u.gridx + tracerange, u.gridy + tracerange))
            {
                target = null;
            }
        }

        //자기유닛들 관리
        List<Unit> removelist = new List<Unit>();
        foreach(Unit myu in myunits)
        {

        }
        foreach(Unit reu in removelist)
        {
            myunits.Remove(reu);
        }
        removelist.Clear();

        //attackers
        foreach(Unit atu in attackers)
        {

        }
        foreach(Unit reu in removelist)
        {
            attackers.Remove(reu);
        }
        removelist.Clear();

        //defenders
        foreach(Unit dfu in defenders)
        {

        }
        foreach(Unit reu in removelist)
        {
            defenders.Remove(reu);
        }
        // removelist.Clear();

        if (target != null)
        {
            //attackers에 공격 명령 할당

            adsgsdgvgwewer`1231231234552352
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这仅仅只是静止状态的行为
public class IdleState : IState
{
    private FSM manager;//添加对状态机的引用
    private Parameter parameter;//设置属性

    //计时器
    private float timer;

    //在构造函数中获取到状态机的属性，再由其属性改变状态机的对象
    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //在实现接口后必须实现这个接口中的所有方法，否则会报错。
    public void OnEnter()
    {
        parameter.anim.Play("Idle");//补考条件直接播放动画
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        //加入发现了玩家进入反应状态
        if(parameter.target !=null && parameter.target.position.x>=parameter.chasePoints[0].position.x &&parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }

        //巡逻到目标地点先停一下，然后再巡逻
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;//结束时清空计时器
    }
}

/// /巡逻状态
public class PatrolState : IState
{
    private FSM manager;//添加对状态机的引用
    private Parameter parameter;//设置属性

    private int patrolPosition;//查找巡逻点

    //在构造函数中获取到状态机的属性，再由其属性改变状态机的对象
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //在实现接口后必须实现这个接口中的所有方法，否则会报错。
    public void OnEnter()
    {
        parameter.anim.Play("Run");
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);//让敌人持续朝向巡逻方向

        manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);//移动到目标点

        //加入发现了玩家进入反应状态
        if (parameter.target != null && parameter.target.position.x >= parameter.chasePoints[0].position.x && parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) <= 1)//当两者目标相近时判断已经到达位置
        {
            manager.TransitionState(StateType.Idle);

        }
    }

    public void OnExit()
    {
        patrolPosition++;

        if (patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;//如果寻路大数字大于器数组范围则从0开始巡逻。
        }
    }
}

//追击状态
public class ChaseState : IState
{
    private FSM manager;//添加对状态机的引用
    private Parameter parameter;//设置属性

    //在构造函数中获取到状态机的属性，再由其属性改变状态机的对象
    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //在实现接口后必须实现这个接口中的所有方法，否则会报错。
    public void OnEnter()
    {
        parameter.anim.Play("Run");
    }

    public void OnUpdate()
    {
        //始终让敌人朝向玩家移动
        manager.FlipTo(parameter.target);
        if (parameter.target)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.target.position, parameter.chaseSpeed * Time.deltaTime);

        //如果追丢目标停止巡逻
        if(parameter.target ==null || manager.transform.position.x < parameter.chasePoints[0].position.x || manager.transform.position.y > parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.Idle);
        }
        if (Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.tagPlayer))
        {
            manager.TransitionState(StateType.Attack);
        }

    }

    public void OnExit()
    {

    }
}

//反应状态代码
public class ReactState : IState
{
    private FSM manager;//添加对状态机的引用
    private Parameter parameter;//设置属性

    private AnimatorStateInfo info;//获取动画的播放进度

    //在构造函数中获取到状态机的属性，再由其属性改变状态机的对象
    public ReactState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //在实现接口后必须实现这个接口中的所有方法，否则会报错。
    public void OnEnter()
    {
        parameter.anim.Play("Dash-Attack");
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);//获取动画进度并保持朝向

        if (info.normalizedTime >= 0.95f)//当动画快播完时进入追击状态
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        
    }
}

public class AttackState : IState
{
    private FSM manager;//添加对状态机的引用
    private Parameter parameter;//设置属性

    private AnimatorStateInfo info;//获取动画的播放进度

    //在构造函数中获取到状态机的属性，再由其属性改变状态机的对象
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //在实现接口后必须实现这个接口中的所有方法，否则会报错。
    public void OnEnter()
    {
        parameter.anim.Play("Attack");
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);//获取动画进度并保持朝向

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }


    }

    public void OnExit()
    {

    }
}

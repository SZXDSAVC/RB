using System;//序列化这篇代码
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//创建枚举方便查找状态
public enum StateType
{
    Idle,Patrol,Chase,React,Attack
}

[Serializable]
public class Parameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;//停止时间
    public Transform[] patrolPoints;//巡逻范围
    public Transform[] chasePoints;//追击范围
    public Transform target;
    public LayerMask tagPlayer;
    public Transform attackPoint;
    public float attackArea;
    public Animator anim;
}

//优先状态机脚本
public class FSM : MonoBehaviour
{
    public Parameter parameter;//获取上面那个类力的基本参数

    private IState currentState;//要先选定一个状态
    //字典集合，通过前面的key找到后面的值
    private IDictionary<StateType, IState> states = new Dictionary<StateType,IState>();

    void Start()
    {
        parameter.anim = GetComponent<Animator>();

        //注册所有状态顺带把自己的引用传给状态
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));

        TransitionState(StateType.Idle);//设置初始状态是Idle
    }

 
    void Update()
    {
        currentState.OnUpdate();
    }

    //切换状态方法
    public void TransitionState(StateType type)
    {
        if (currentState !!= null)
            currentState.OnExit();
        //转换状态是先取消上一阶段的状态
        currentState = states[type];//根据状态的词条在字典中查找状态来表示切换为什么状态。
        currentState.OnEnter();//执行新状态的onEnter函数
    }

    //敌人朝向方法
    public void FlipTo(Transform target)
    {
        if (target != null)//如果目标不为空
        {
            if (transform.position.x > target.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            if(transform.position.x<target.position.x)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }

    //画圆方法
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}

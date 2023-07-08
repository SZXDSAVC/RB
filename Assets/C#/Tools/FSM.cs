using System;//���л���ƪ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ö�ٷ������״̬
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
    public float idleTime;//ֹͣʱ��
    public Transform[] patrolPoints;//Ѳ�߷�Χ
    public Transform[] chasePoints;//׷����Χ
    public Transform target;
    public LayerMask tagPlayer;
    public Transform attackPoint;
    public float attackArea;
    public Animator anim;
}

//����״̬���ű�
public class FSM : MonoBehaviour
{
    public Parameter parameter;//��ȡ�����Ǹ������Ļ�������

    private IState currentState;//Ҫ��ѡ��һ��״̬
    //�ֵ伯�ϣ�ͨ��ǰ���key�ҵ������ֵ
    private IDictionary<StateType, IState> states = new Dictionary<StateType,IState>();

    void Start()
    {
        parameter.anim = GetComponent<Animator>();

        //ע������״̬˳�����Լ������ô���״̬
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));

        TransitionState(StateType.Idle);//���ó�ʼ״̬��Idle
    }

 
    void Update()
    {
        currentState.OnUpdate();
    }

    //�л�״̬����
    public void TransitionState(StateType type)
    {
        if (currentState !!= null)
            currentState.OnExit();
        //ת��״̬����ȡ����һ�׶ε�״̬
        currentState = states[type];//����״̬�Ĵ������ֵ��в���״̬����ʾ�л�Ϊʲô״̬��
        currentState.OnEnter();//ִ����״̬��onEnter����
    }

    //���˳��򷽷�
    public void FlipTo(Transform target)
    {
        if (target != null)//���Ŀ�겻Ϊ��
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

    //��Բ����
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}

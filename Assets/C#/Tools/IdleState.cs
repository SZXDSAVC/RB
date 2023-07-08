using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ֻ�Ǿ�ֹ״̬����Ϊ
public class IdleState : IState
{
    private FSM manager;//��Ӷ�״̬��������
    private Parameter parameter;//��������

    //��ʱ��
    private float timer;

    //�ڹ��캯���л�ȡ��״̬�������ԣ����������Ըı�״̬���Ķ���
    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //��ʵ�ֽӿں����ʵ������ӿ��е����з���������ᱨ��
    public void OnEnter()
    {
        parameter.anim.Play("Idle");//��������ֱ�Ӳ��Ŷ���
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        //���뷢������ҽ��뷴Ӧ״̬
        if(parameter.target !=null && parameter.target.position.x>=parameter.chasePoints[0].position.x &&parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }

        //Ѳ�ߵ�Ŀ��ص���ͣһ�£�Ȼ����Ѳ��
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        timer = 0;//����ʱ��ռ�ʱ��
    }
}

/// /Ѳ��״̬
public class PatrolState : IState
{
    private FSM manager;//��Ӷ�״̬��������
    private Parameter parameter;//��������

    private int patrolPosition;//����Ѳ�ߵ�

    //�ڹ��캯���л�ȡ��״̬�������ԣ����������Ըı�״̬���Ķ���
    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //��ʵ�ֽӿں����ʵ������ӿ��е����з���������ᱨ��
    public void OnEnter()
    {
        parameter.anim.Play("Run");
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);//�õ��˳�������Ѳ�߷���

        manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);//�ƶ���Ŀ���

        //���뷢������ҽ��뷴Ӧ״̬
        if (parameter.target != null && parameter.target.position.x >= parameter.chasePoints[0].position.x && parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(StateType.React);
        }

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) <= 1)//������Ŀ�����ʱ�ж��Ѿ�����λ��
        {
            manager.TransitionState(StateType.Idle);

        }
    }

    public void OnExit()
    {
        patrolPosition++;

        if (patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;//���Ѱ·�����ִ��������鷶Χ���0��ʼѲ�ߡ�
        }
    }
}

//׷��״̬
public class ChaseState : IState
{
    private FSM manager;//��Ӷ�״̬��������
    private Parameter parameter;//��������

    //�ڹ��캯���л�ȡ��״̬�������ԣ����������Ըı�״̬���Ķ���
    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //��ʵ�ֽӿں����ʵ������ӿ��е����з���������ᱨ��
    public void OnEnter()
    {
        parameter.anim.Play("Run");
    }

    public void OnUpdate()
    {
        //ʼ���õ��˳�������ƶ�
        manager.FlipTo(parameter.target);
        if (parameter.target)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.target.position, parameter.chaseSpeed * Time.deltaTime);

        //���׷��Ŀ��ֹͣѲ��
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

//��Ӧ״̬����
public class ReactState : IState
{
    private FSM manager;//��Ӷ�״̬��������
    private Parameter parameter;//��������

    private AnimatorStateInfo info;//��ȡ�����Ĳ��Ž���

    //�ڹ��캯���л�ȡ��״̬�������ԣ����������Ըı�״̬���Ķ���
    public ReactState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //��ʵ�ֽӿں����ʵ������ӿ��е����з���������ᱨ��
    public void OnEnter()
    {
        parameter.anim.Play("Dash-Attack");
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);//��ȡ�������Ȳ����ֳ���

        if (info.normalizedTime >= 0.95f)//�������첥��ʱ����׷��״̬
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
    private FSM manager;//��Ӷ�״̬��������
    private Parameter parameter;//��������

    private AnimatorStateInfo info;//��ȡ�����Ĳ��Ž���

    //�ڹ��캯���л�ȡ��״̬�������ԣ����������Ըı�״̬���Ķ���
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    //��ʵ�ֽӿں����ʵ������ӿ��е����з���������ᱨ��
    public void OnEnter()
    {
        parameter.anim.Play("Attack");
    }

    public void OnUpdate()
    {
        info = parameter.anim.GetCurrentAnimatorStateInfo(0);//��ȡ�������Ȳ����ֳ���

        if (info.normalizedTime >= 0.95f)
        {
            manager.TransitionState(StateType.Chase);
        }


    }

    public void OnExit()
    {

    }
}

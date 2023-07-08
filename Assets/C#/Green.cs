using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GreenStates {SLEEP,WAKE,RUN,IDLE,WIN}
public class Green : MonoBehaviour
{
    public int speed = 10;

    //获取枚举规划敌人状态
    private GreenStates greenStates;

    //获取组件
    private Animator anim;
    private Rigidbody2D rb;
    public BoxCollider2D sleepColl;
    private CapsuleCollider2D wakeColl;
    private PlayerContent player;
    private Enemy enemy;
    public Transform target;//获取主角的位置
    public GameObject greenTalkUI;
    public Transform runPoint;
    public Transform EndPoint;
    public GameObject End;

    //状态
    bool isWake;
    bool isRun;
    public static bool textIsEnd;
    bool isRight;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContent>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        sleepColl = GetComponent<BoxCollider2D>();
        wakeColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    //状态
    void Start()
    {
        greenStates = GreenStates.SLEEP;
        isWake = false;
        textIsEnd = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchStates();
        Anim();
    }

    void SwitchStates()
    {
        switch (greenStates)
        {
            case GreenStates.SLEEP://睡眠状态
                anim.Play("sleep");
                if (target != null && isWake == false)//如果发现玩家并且每苏醒就转化为苏醒状态
                    greenStates = GreenStates.WAKE;
                break;
            case GreenStates.WAKE:
                isWake = true;
                anim.SetTrigger("IsWake");
                //sleepColl.enabled = false;
                //wakeColl.enabled = true;
                break;
            case GreenStates.RUN:
                isRun = true;
                transform.position = Vector3.MoveTowards(transform.position,runPoint.position,speed*Time.deltaTime);
                if (Vector2.Distance(transform.position, runPoint.position) <= 1)
                {
                    transform.position = runPoint.position;
                    isRun = false;
                    Invoke("RunToIdle",1);
                }
                break;
            case GreenStates.IDLE:
                if (textIsEnd == true)
                {
                    Debug.Log("444");
                    greenStates = GreenStates.WIN;
                }
                break;
            case GreenStates.WIN:
                CameraFollow.Instence.target = EndPoint;
                End.SetActive(true);
                break;
        }
    }

    //检测玩家进入boss的视线。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //沉睡时先获取主角位置
        if (collision.CompareTag("Player") && isWake == false)
        {
            target = collision.transform;
        }else if (collision.CompareTag("Player") && isWake == true&&greenStates==GreenStates.IDLE)
        {
            enemy.isStop = true;
            greenTalkUI.SetActive(true);
        }
    }

    void Anim()
    {
        anim.SetBool("IsRun", isRun);
    }

    void AnimWakeToRun()
    {
        greenStates = GreenStates.RUN;
        sleepColl.enabled = false;
        wakeColl.enabled = true;
    }

    void RunToIdle()
    {
        greenStates = GreenStates.IDLE;
    }
}

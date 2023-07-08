using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// boss专用敌人脚本（未改通用）
/// </summary>

//枚举事件（沉睡,唤醒，攻击，失败）
public enum EnemyStates { SLEEP, WAKE, ATTACK, DEAD, WIN,CATCH }

public class Enemy : MonoBehaviour
{
    //获取枚举规划敌人状态
    private EnemyStates enenmyStates;

    //参数设置
    public Transform target;//获取主角的位置
    public LayerMask tagPlayer;//检测主角的图层
    public Transform attackPoint;//攻击范围的圆心点
    public float attackArea;//攻击范围的半径
    public float targeToEnemy;//敌人和玩家间的距离
    [Header("攻击设置")]
    public float catchSpeed;
    public float catchTime;//追击时间。
    public float slideSpeed;
    public float begTime;//记录大招时间
    public float nextAttack;//下一次攻击的时间


    //状态判断
    bool isWake;//是否已经被唤醒
    bool isCroush;
    public static bool isSpeech;//是否在对话
    public bool isCatch;//是否在追击状态
    private bool isReadNextAttack;//是否准备开始下次攻击
    private bool isAttack;
    private bool isSlide;
    public bool isStop;

    //组件获取
    private AnimatorStateInfo info;//获取动画当前状态
    private PlayerContent player;
    private CapsuleCollider2D bossColl;
    private Rigidbody2D bossRb;
    private Animator bossAnim;
    public GameObject talkUI;
    public GameObject hurtWalk;
    public Transform bagPoint;
    public Text talkUIText;
    public GameObject enemyWinMain;//结算界面
    public GameObject playerWinMain;

    //可改参数
    int WakeAttack;//防止苏醒后不停的攻击

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContent>();
        bossColl = GetComponent<CapsuleCollider2D>();
        bossRb = GetComponent<Rigidbody2D>();
        bossAnim = GetComponent<Animator>();
    }

    void Start()
    {
        enenmyStates = EnemyStates.SLEEP;
        isWake = false;
        WakeAttack = 0;
        isReadNextAttack = true;
        isAttack = false;
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
            if (target != null)//时刻计算玩家和敌人间的距离
            {
                targeToEnemy = Vector3.Distance(transform.position, target.transform.position);
            }
        if (isStop == true)
        {
            enenmyStates = EnemyStates.SLEEP;
            isCatch = false;
            StopAllCoroutines();
        }

            SwitchStates();
            Animtor();
    }

    void SwitchStates()
    {
        switch (enenmyStates)
        {
            case EnemyStates.SLEEP://睡眠状态
                bossAnim.Play("Idle");
                if (target != null && isWake == false)//如果发现玩家并且每苏醒就转化为苏醒状态
                    enenmyStates = EnemyStates.WAKE;
                break;

            case EnemyStates.WAKE://苏醒激活
                //Debug.Log("WAKE");
                //主角不能移动
                isWake = true;
                player.isGameAlive = false;
                player.StopPlayerFly();
                //摄像机视角
                CameraFollow.Instence.target = transform;
                //文本显示
                info = bossAnim.GetCurrentAnimatorStateInfo(0);//获取动画进度并保持朝向
                if (info.normalizedTime >= 0.95&& WakeAttack < 1)
                {
                    talkUI.SetActive(true);
                    WakeAttack++;
                    isSpeech = true;
                }
                //放激活动画
                if (WakeAttack == 1 && isSpeech == false)
                {
                    isCroush = true;
                }
                //拉起钉墙(动画事件了)
                //转换为攻击状态(动画事件了)
                break;

            case EnemyStates.CATCH:
                //敌人胜利模式和失败模式
                if (player.Hp <= 0)
                    enenmyStates = EnemyStates.WIN;
                if (player.playerWin)
                {
                    StopAllCoroutines();
                    isCatch = false;
                    enenmyStates = EnemyStates.DEAD;
                }
                //正常追击判断
                FlipTo(target);          
                if(target!=null&&!isAttack)
                    transform.position=Vector3.MoveTowards(transform.position, target.position, catchSpeed * Time.deltaTime);
                if (Time.time > nextAttack)
                    isReadNextAttack = true;
                if (isReadNextAttack&& Physics2D.OverlapCircle(attackPoint.position,attackArea,tagPlayer)&&target!=null&& targeToEnemy <= 8)//近战跑过去砍
                {
                    isAttack = true;
                    isCatch = false;
                    isReadNextAttack = false;
                    bossAnim.SetTrigger("Attack");
                    nextAttack = Time.time + catchTime;
                    Invoke("ReturnCatch", 2f);
                }
                else if(isReadNextAttack && target != null && targeToEnemy >8)//距离远就滑铲
                {
                    enenmyStates = EnemyStates.ATTACK;
                    StartCoroutine(BagAttack());//滑铲成功接近敌人则会取消该携程，否则直接瞬移一砍
                }
                    break;

            case EnemyStates.ATTACK:   //写作攻击实则滑铲
                isSlide = true;
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(target.position.x,transform.position.y), slideSpeed * Time.deltaTime);               
                if(Vector3.Distance(transform.position, target.transform.position) < 2&&isSlide)
                {
                   StopAllCoroutines();
                   isSlide = false;
                   enenmyStates = EnemyStates.CATCH;
                }
                break;

            case EnemyStates.DEAD://玩家胜利
                isCatch = false;
                target = null;
                talkUI.SetActive(true);
                talkUIText.text = "哇~你居然做到了！加油小家伙，我相信你一定可以达到夫人的要求的。";
                //胜利界面（延时）
                Invoke("StartDeadMain", 3);
                break;

            case EnemyStates.WIN://敌人胜利
                //主角被杀后播放动画
                isCatch = false;
                target = null;
                talkUI.SetActive(true);
                talkUIText.text = "额...下手太重了...我都被削了好几次了吖...emmmm";
                //TODO：失败界面（延时）
                Invoke("StartWinMain", 3);
                break;
        }
    }

    //敌人胜利结算界面
    void StartWinMain()
    {
      enemyWinMain.SetActive(true);
    }
    //玩家胜利结算界面
    void StartDeadMain()
    {
      playerWinMain.SetActive(true);
    }

    //大招携程
    IEnumerator BagAttack()
    {
        yield return new WaitForSeconds(begTime);
        isSlide = false;
        isCatch = true;
        bossAnim.SetTrigger("Dash");
        bossRb.mass = 0;
        FlipTo(target);      
        transform.position = bagPoint.position;
        //TODO:加个瞬移特效？
    }

    //后滑转攻击（动画事件）
    void DashToAttack()
    {
        FlipTo(target);
        bossAnim.SetTrigger("Attack");
    }

    //恢复追击敌人
    void ReturnCatch()
    {
        isAttack = false;
        isCatch = true;
        enenmyStates = EnemyStates.CATCH;
    }

    //敌人朝向方法
    public void FlipTo(Transform target)
    {
        if (target != null)//如果目标不为空
        {
            if (transform.position.x > target.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x < target.position.x)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }



    //检测玩家进入boss的视线。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //沉睡时先获取主角位置
        if (collision.CompareTag("Player")&&isWake==false)
        {
            target = collision.transform;
            return;
        }
    }

    //动画管理
    void Animtor()
    {
        bossAnim.SetBool("Croush", isCroush);
        bossAnim.SetBool("IsCatch", isCatch);
        bossAnim.SetBool("Slide", isSlide);
    }

    //画圆方法
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackArea);
    }

    //动画器控制拉起高墙
    void HurtWalkIsBorn()
    {
        HurtWlak.isBorn = true;
    }

    //由下蹲切换到攻击状态
    void FromWakeToAttack()
    {
        isCroush = false;
        bossAnim.SetTrigger("Attack");
        enenmyStates = EnemyStates.CATCH;
        player.isGameAlive = true;
        HurtWlak.isCatch = true;
        CameraFollow.Instence.target = target;
        isReadNextAttack = true;
    }
}

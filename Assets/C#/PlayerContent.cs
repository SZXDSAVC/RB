using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//控制主角
public class PlayerContent : MonoBehaviour
{
    //数值列表
    [Header("人物基础数值")]
    public int speed = 5;
    public int jumpHigh = 5;
    public float DashUp = 3;
    public int Hp;
    public float returnTime;//恢复正常的时间
    public float NBTime;//无敌时间
    private Vector2 direction;//被击退的向量

    [Header("冲参数")]
    public float dashTime;//冲锋时间
    private float dashTimeLeft;//冲锋剩余时间
    public float dashSpeed;//冲锋速度
    private float lastDash = -10;//上一次冲锋开始的时间点 （设置-10以确保游戏一开始能用冲锋，同理CD不能超过10面，建议设置必CD略短）
    public float dashCoolDown;//冲锋技能的CD
    [Header("CD的UI组件")]
    public Image cdImage;


    //状态的判断
    public bool isOnGround, isDashing;
    public bool isGameAlive = true;//判断主角是否能移动
    public bool isHurt;//受伤啦
    public bool playerWin;

    //组件获取
    private CapsuleCollider2D coll;
    private BoxCollider2D footColl;
    private Animator anim;
    private Rigidbody2D rb;
    public LayerMask ground;
    private SpriteRenderer sr;//获取图像组件
    private Color beginColor;//获取一开始的颜色
    public Transform playerReadPoint;

    [Header("管理角色材质的")]
    public PhysicsMaterial2D haveFrc;
    public PhysicsMaterial2D notFrc;


    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        footColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        beginColor = sr.color;
    }

    void Start()
    {
        isOnGround = true;
    }

    void Update()
    {
        if (isGameAlive)//如果主角还能移动
        {
            Movement();
            Jump();
            Anim();
            PlayerHurt();
            //按下冲锋键
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= (lastDash + dashCoolDown)) //游戏时间大于上一次开始冲锋的时间+冲锋CD
                {
                    //可执行冲锋
                    ReadToDash();
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
                transform.position = playerReadPoint.position;
            if (Input.GetKeyDown(KeyCode.L))
                Hp += 30;


            cdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;//实时剪掉技能CD完成技能回转
        }

    }

    private void FixedUpdate()
    {
        if (isGameAlive)
        {
            isOnGround = footColl.IsTouchingLayers(ground);

            Dash();

            if (isDashing)
            {
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 1;
            }
        }
    }

    //移动代码
    void Movement()
    {
        float moveDir = Input.GetAxis("Horizontal");//获取按键输入值
        float faceDircetion = Input.GetAxisRaw("Horizontal");//根据按键输入值获取方向
        if (moveDir != 0)
        {
            rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
            anim.SetFloat("Walk", Mathf.Abs(faceDircetion));
        }
        if (faceDircetion != 0)
        {
            transform.localScale = new Vector3(faceDircetion, 1, 1);
        }
    }

    //跳跃代码
    void Jump()
    {
        if (Input.GetButtonDown("Jump")&& isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHigh);
        }
        if (Input.GetKeyDown(KeyCode.W) && isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHigh);
        }
    }

    //控制动画
    void Anim()
    {
        anim.SetFloat("Y.vel", rb.velocity.y);
        anim.SetBool("IsOnGround", isOnGround);
        anim.SetBool("Icon", isDashing);

        //在空中和地面不用一种材质避免卡墙
        if (!isOnGround)
        {
            rb.sharedMaterial = notFrc;
        }
        else
        {
            rb.sharedMaterial = haveFrc;
        }
    }

    //冲锋准备
    void ReadToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;//冲锋状态的总时长=冲锋的旧时长。冲锋剩余时间减少

        lastDash = Time.time;//记录结束冲锋的时间

        cdImage.fillAmount = 1;//技能黑幕
    }

    //冲锋速度方法
    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)//如果冲锋时间还没结束
            {
                if (rb.velocity.y > 0 && !isOnGround)//如果是在空中冲锋则会斜上冲锋
                {
                    rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, DashUp);
                }

                rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isOnGround)
                {
                    rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, DashUp);//如果冲锋结束还没碰地则会小跳一下
                }
            }
        }
    }

    //取消红色恢复正常画面携程
    IEnumerator RedPlayer(float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        sr.color = beginColor;
    }

    //受伤无敌时间
    IEnumerator NotHurtTime(float NBTime)
    {
        yield return new WaitForSeconds(NBTime);
        gameObject.tag = ("Player");
        isHurt = false;
        speed -= 1;
        jumpHigh -= 1;
    }


    //防止停止操控后主角乱飞代码
    public void StopPlayerFly()
    {
        if (!isGameAlive)
        {
            rb.Sleep();
            
            //rb.transform.position = Vector2.MoveTowards(transform.position, staySee, speed * Time.deltaTime);
        }
    }

    public void PlayerHurt()
    {
        if (isHurt)
        {
            if (Hp < 0)
                Hp = 0;
            //如果角色死亡
            if (Hp <= 0)
            {
                isGameAlive = false;//主角不可移动激活
                anim.SetBool("Die", true);//转换位死亡动画
                gameObject.tag = "Die";
            }
            speed +=1;
            jumpHigh += 1;
            gameObject.tag = ("Finish");
            sr.color = Color.red;
            StartCoroutine(RedPlayer(returnTime));
            StartCoroutine(NotHurtTime(NBTime));
            //受伤无敌等等等等，以及恢复有敌
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Diebox")
            {
                Destroy(gameObject);
            }       
    }
}


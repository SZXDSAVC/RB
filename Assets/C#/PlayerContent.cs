using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��������
public class PlayerContent : MonoBehaviour
{
    //��ֵ�б�
    [Header("���������ֵ")]
    public int speed = 5;
    public int jumpHigh = 5;
    public float DashUp = 3;
    public int Hp;
    public float returnTime;//�ָ�������ʱ��
    public float NBTime;//�޵�ʱ��
    private Vector2 direction;//�����˵�����

    [Header("�����")]
    public float dashTime;//���ʱ��
    private float dashTimeLeft;//���ʣ��ʱ��
    public float dashSpeed;//����ٶ�
    private float lastDash = -10;//��һ�γ�濪ʼ��ʱ��� ������-10��ȷ����Ϸһ��ʼ���ó�棬ͬ��CD���ܳ���10�棬�������ñ�CD�Զ̣�
    public float dashCoolDown;//��漼�ܵ�CD
    [Header("CD��UI���")]
    public Image cdImage;


    //״̬���ж�
    public bool isOnGround, isDashing;
    public bool isGameAlive = true;//�ж������Ƿ����ƶ�
    public bool isHurt;//������
    public bool playerWin;

    //�����ȡ
    private CapsuleCollider2D coll;
    private BoxCollider2D footColl;
    private Animator anim;
    private Rigidbody2D rb;
    public LayerMask ground;
    private SpriteRenderer sr;//��ȡͼ�����
    private Color beginColor;//��ȡһ��ʼ����ɫ
    public Transform playerReadPoint;

    [Header("�����ɫ���ʵ�")]
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
        if (isGameAlive)//������ǻ����ƶ�
        {
            Movement();
            Jump();
            Anim();
            PlayerHurt();
            //���³���
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= (lastDash + dashCoolDown)) //��Ϸʱ�������һ�ο�ʼ����ʱ��+���CD
                {
                    //��ִ�г��
                    ReadToDash();
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
                transform.position = playerReadPoint.position;
            if (Input.GetKeyDown(KeyCode.L))
                Hp += 30;


            cdImage.fillAmount -= 1.0f / dashCoolDown * Time.deltaTime;//ʵʱ��������CD��ɼ��ܻ�ת
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

    //�ƶ�����
    void Movement()
    {
        float moveDir = Input.GetAxis("Horizontal");//��ȡ��������ֵ
        float faceDircetion = Input.GetAxisRaw("Horizontal");//���ݰ�������ֵ��ȡ����
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

    //��Ծ����
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

    //���ƶ���
    void Anim()
    {
        anim.SetFloat("Y.vel", rb.velocity.y);
        anim.SetBool("IsOnGround", isOnGround);
        anim.SetBool("Icon", isDashing);

        //�ڿ��к͵��治��һ�ֲ��ʱ��⿨ǽ
        if (!isOnGround)
        {
            rb.sharedMaterial = notFrc;
        }
        else
        {
            rb.sharedMaterial = haveFrc;
        }
    }

    //���׼��
    void ReadToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;//���״̬����ʱ��=���ľ�ʱ�������ʣ��ʱ�����

        lastDash = Time.time;//��¼��������ʱ��

        cdImage.fillAmount = 1;//���ܺ�Ļ
    }

    //����ٶȷ���
    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)//������ʱ�仹û����
            {
                if (rb.velocity.y > 0 && !isOnGround)//������ڿ��г�����б�ϳ��
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
                    rb.velocity = new Vector2(dashSpeed * gameObject.transform.localScale.x, DashUp);//�����������û�������С��һ��
                }
            }
        }
    }

    //ȡ����ɫ�ָ���������Я��
    IEnumerator RedPlayer(float returnTime)
    {
        yield return new WaitForSeconds(returnTime);
        sr.color = beginColor;
    }

    //�����޵�ʱ��
    IEnumerator NotHurtTime(float NBTime)
    {
        yield return new WaitForSeconds(NBTime);
        gameObject.tag = ("Player");
        isHurt = false;
        speed -= 1;
        jumpHigh -= 1;
    }


    //��ֹֹͣ�ٿغ������ҷɴ���
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
            //�����ɫ����
            if (Hp <= 0)
            {
                isGameAlive = false;//���ǲ����ƶ�����
                anim.SetBool("Die", true);//ת��λ��������
                gameObject.tag = "Die";
            }
            speed +=1;
            jumpHigh += 1;
            gameObject.tag = ("Finish");
            sr.color = Color.red;
            StartCoroutine(RedPlayer(returnTime));
            StartCoroutine(NotHurtTime(NBTime));
            //�����޵еȵȵȵȣ��Լ��ָ��е�
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


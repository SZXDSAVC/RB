using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// bossר�õ��˽ű���δ��ͨ�ã�
/// </summary>

//ö���¼�����˯,���ѣ�������ʧ�ܣ�
public enum EnemyStates { SLEEP, WAKE, ATTACK, DEAD, WIN,CATCH }

public class Enemy : MonoBehaviour
{
    //��ȡö�ٹ滮����״̬
    private EnemyStates enenmyStates;

    //��������
    public Transform target;//��ȡ���ǵ�λ��
    public LayerMask tagPlayer;//������ǵ�ͼ��
    public Transform attackPoint;//������Χ��Բ�ĵ�
    public float attackArea;//������Χ�İ뾶
    public float targeToEnemy;//���˺���Ҽ�ľ���
    [Header("��������")]
    public float catchSpeed;
    public float catchTime;//׷��ʱ�䡣
    public float slideSpeed;
    public float begTime;//��¼����ʱ��
    public float nextAttack;//��һ�ι�����ʱ��


    //״̬�ж�
    bool isWake;//�Ƿ��Ѿ�������
    bool isCroush;
    public static bool isSpeech;//�Ƿ��ڶԻ�
    public bool isCatch;//�Ƿ���׷��״̬
    private bool isReadNextAttack;//�Ƿ�׼����ʼ�´ι���
    private bool isAttack;
    private bool isSlide;
    public bool isStop;

    //�����ȡ
    private AnimatorStateInfo info;//��ȡ������ǰ״̬
    private PlayerContent player;
    private CapsuleCollider2D bossColl;
    private Rigidbody2D bossRb;
    private Animator bossAnim;
    public GameObject talkUI;
    public GameObject hurtWalk;
    public Transform bagPoint;
    public Text talkUIText;
    public GameObject enemyWinMain;//�������
    public GameObject playerWinMain;

    //�ɸĲ���
    int WakeAttack;//��ֹ���Ѻ�ͣ�Ĺ���

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
            if (target != null)//ʱ�̼�����Һ͵��˼�ľ���
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
            case EnemyStates.SLEEP://˯��״̬
                bossAnim.Play("Idle");
                if (target != null && isWake == false)//���������Ҳ���ÿ���Ѿ�ת��Ϊ����״̬
                    enenmyStates = EnemyStates.WAKE;
                break;

            case EnemyStates.WAKE://���Ѽ���
                //Debug.Log("WAKE");
                //���ǲ����ƶ�
                isWake = true;
                player.isGameAlive = false;
                player.StopPlayerFly();
                //������ӽ�
                CameraFollow.Instence.target = transform;
                //�ı���ʾ
                info = bossAnim.GetCurrentAnimatorStateInfo(0);//��ȡ�������Ȳ����ֳ���
                if (info.normalizedTime >= 0.95&& WakeAttack < 1)
                {
                    talkUI.SetActive(true);
                    WakeAttack++;
                    isSpeech = true;
                }
                //�ż����
                if (WakeAttack == 1 && isSpeech == false)
                {
                    isCroush = true;
                }
                //����ǽ(�����¼���)
                //ת��Ϊ����״̬(�����¼���)
                break;

            case EnemyStates.CATCH:
                //����ʤ��ģʽ��ʧ��ģʽ
                if (player.Hp <= 0)
                    enenmyStates = EnemyStates.WIN;
                if (player.playerWin)
                {
                    StopAllCoroutines();
                    isCatch = false;
                    enenmyStates = EnemyStates.DEAD;
                }
                //����׷���ж�
                FlipTo(target);          
                if(target!=null&&!isAttack)
                    transform.position=Vector3.MoveTowards(transform.position, target.position, catchSpeed * Time.deltaTime);
                if (Time.time > nextAttack)
                    isReadNextAttack = true;
                if (isReadNextAttack&& Physics2D.OverlapCircle(attackPoint.position,attackArea,tagPlayer)&&target!=null&& targeToEnemy <= 8)//��ս�ܹ�ȥ��
                {
                    isAttack = true;
                    isCatch = false;
                    isReadNextAttack = false;
                    bossAnim.SetTrigger("Attack");
                    nextAttack = Time.time + catchTime;
                    Invoke("ReturnCatch", 2f);
                }
                else if(isReadNextAttack && target != null && targeToEnemy >8)//����Զ�ͻ���
                {
                    enenmyStates = EnemyStates.ATTACK;
                    StartCoroutine(BagAttack());//�����ɹ��ӽ��������ȡ����Я�̣�����ֱ��˲��һ��
                }
                    break;

            case EnemyStates.ATTACK:   //д������ʵ�򻬲�
                isSlide = true;
                transform.position = Vector2.MoveTowards(transform.position,new Vector2(target.position.x,transform.position.y), slideSpeed * Time.deltaTime);               
                if(Vector3.Distance(transform.position, target.transform.position) < 2&&isSlide)
                {
                   StopAllCoroutines();
                   isSlide = false;
                   enenmyStates = EnemyStates.CATCH;
                }
                break;

            case EnemyStates.DEAD://���ʤ��
                isCatch = false;
                target = null;
                talkUI.SetActive(true);
                talkUIText.text = "��~���Ȼ�����ˣ�����С�һ��������һ�����Դﵽ���˵�Ҫ��ġ�";
                //ʤ�����棨��ʱ��
                Invoke("StartDeadMain", 3);
                break;

            case EnemyStates.WIN://����ʤ��
                //���Ǳ�ɱ�󲥷Ŷ���
                isCatch = false;
                target = null;
                talkUI.SetActive(true);
                talkUIText.text = "��...����̫����...�Ҷ������˺ü�����߹...emmmm";
                //TODO��ʧ�ܽ��棨��ʱ��
                Invoke("StartWinMain", 3);
                break;
        }
    }

    //����ʤ���������
    void StartWinMain()
    {
      enemyWinMain.SetActive(true);
    }
    //���ʤ���������
    void StartDeadMain()
    {
      playerWinMain.SetActive(true);
    }

    //����Я��
    IEnumerator BagAttack()
    {
        yield return new WaitForSeconds(begTime);
        isSlide = false;
        isCatch = true;
        bossAnim.SetTrigger("Dash");
        bossRb.mass = 0;
        FlipTo(target);      
        transform.position = bagPoint.position;
        //TODO:�Ӹ�˲����Ч��
    }

    //��ת�����������¼���
    void DashToAttack()
    {
        FlipTo(target);
        bossAnim.SetTrigger("Attack");
    }

    //�ָ�׷������
    void ReturnCatch()
    {
        isAttack = false;
        isCatch = true;
        enenmyStates = EnemyStates.CATCH;
    }

    //���˳��򷽷�
    public void FlipTo(Transform target)
    {
        if (target != null)//���Ŀ�겻Ϊ��
        {
            if (transform.position.x > target.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            if (transform.position.x < target.position.x)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }



    //�����ҽ���boss�����ߡ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��˯ʱ�Ȼ�ȡ����λ��
        if (collision.CompareTag("Player")&&isWake==false)
        {
            target = collision.transform;
            return;
        }
    }

    //��������
    void Animtor()
    {
        bossAnim.SetBool("Croush", isCroush);
        bossAnim.SetBool("IsCatch", isCatch);
        bossAnim.SetBool("Slide", isSlide);
    }

    //��Բ����
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackArea);
    }

    //���������������ǽ
    void HurtWalkIsBorn()
    {
        HurtWlak.isBorn = true;
    }

    //���¶��л�������״̬
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

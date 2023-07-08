using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ǽ�ƶ�
public class HurtWlak : MonoBehaviour
{
    //��ֵ����
    public float bornSpeed;
    public float catachSpeed;

    public Transform bornPoint;//���ó�����
    public Transform catchPoint;//׷���㡣
    private BoxCollider2D walkColl;

    //״̬
    public static bool isBorn;
    public static bool isCatch;

    // Start is called before the first frame update
    void Start()
    {
        walkColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBorn)
        {
            transform.position = Vector2.MoveTowards(transform.position, bornPoint.position, bornSpeed * Time.deltaTime);//�ƶ���Ŀ���
            walkColl.enabled = false;
        }
        if (Vector2.Distance(transform.position, bornPoint.position) <= 1)
        {
            isBorn = false;
            walkColl.enabled = true;
        }

        if (isCatch)
        {
            transform.position = Vector2.MoveTowards(transform.position, catchPoint.position, catachSpeed * Time.deltaTime);//�ƶ���Ŀ���
        }
        if (Vector2.Distance(transform.position, catchPoint.position) <= 1)
            isBorn = false;
    }
}

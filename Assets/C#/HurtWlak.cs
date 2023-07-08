using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//钉墙移动
public class HurtWlak : MonoBehaviour
{
    //数值设置
    public float bornSpeed;
    public float catachSpeed;

    public Transform bornPoint;//设置出生点
    public Transform catchPoint;//追击点。
    private BoxCollider2D walkColl;

    //状态
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
            transform.position = Vector2.MoveTowards(transform.position, bornPoint.position, bornSpeed * Time.deltaTime);//移动到目标点
            walkColl.enabled = false;
        }
        if (Vector2.Distance(transform.position, bornPoint.position) <= 1)
        {
            isBorn = false;
            walkColl.enabled = true;
        }

        if (isCatch)
        {
            transform.position = Vector2.MoveTowards(transform.position, catchPoint.position, catachSpeed * Time.deltaTime);//移动到目标点
        }
        if (Vector2.Distance(transform.position, catchPoint.position) <= 1)
            isBorn = false;
    }
}

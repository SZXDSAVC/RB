using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject gunshi;
    public Transform gunshiBorn;

    bool isOpen;

    private void Start()
    {
        isOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)//�����ⷽ��
    {
        if (collision.tag == "Player"&&isOpen==true)//����⵽ͼ��ΪPlayer����������ʱ
        {
            var pre = Instantiate(gunshi,gunshiBorn.position, Quaternion.identity);
            isOpen = false;
        }
    }
}

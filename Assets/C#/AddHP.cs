using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHP : MonoBehaviour
{
    private PlayerContent player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContent>();
    }

    private void OnTriggerEnter2D(Collider2D collision)//�����ⷽ��
    {
        if (collision.tag == "Player")//����⵽ͼ��ΪPlayer����������ʱ
        {
            player.Hp += 1;
            Destroy(gameObject);
        }
    }
}

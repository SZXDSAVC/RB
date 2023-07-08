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

    private void OnTriggerEnter2D(Collider2D collision)//物体检测方法
    {
        if (collision.tag == "Player")//当检测到图层为Player的玩意碰到时
        {
            player.Hp += 1;
            Destroy(gameObject);
        }
    }
}

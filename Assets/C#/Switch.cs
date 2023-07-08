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

    private void OnTriggerEnter2D(Collider2D collision)//物体检测方法
    {
        if (collision.tag == "Player"&&isOpen==true)//当检测到图层为Player的玩意碰到时
        {
            var pre = Instantiate(gunshi,gunshiBorn.position, Quaternion.identity);
            isOpen = false;
        }
    }
}

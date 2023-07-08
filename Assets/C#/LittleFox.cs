using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleFox : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    bool Ishurt;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Ishurt = false;
    }


    void Update()
    {
        Animtor();
    }

    void Animtor()
    {
        anim.SetBool("ishurt", Ishurt);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="GunShi" && Ishurt == false)//当检测到图层为Player的玩意碰到时
        {
            anim.Play("Hurt");
        }
        if(collision.tag=="Diebox")
        {
            Destroy(gameObject);
        }
    }
}

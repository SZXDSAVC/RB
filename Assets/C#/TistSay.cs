using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///框框对话框代码
///</summary>
public class TistSay: MonoBehaviour
{
    public GameObject enterDialog;//获取要开启的对话框

    private void OnTriggerEnter2D(Collider2D collision)//物体检测方法
    {
        if (collision.tag == "Player"|| collision.tag == "Finish")//当检测到图层为Player的玩意碰到时
        {
            enterDialog.SetActive(true);//启用对话框
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//当物体离开碰撞器时
    {
        if (collision.tag == "Player" || collision.tag == "Finish")//当检测到图层为Player的玩意碰到时
        {
            enterDialog.SetActive(false);//禁用对话框
        }
    }

}

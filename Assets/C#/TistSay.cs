using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///���Ի������
///</summary>
public class TistSay: MonoBehaviour
{
    public GameObject enterDialog;//��ȡҪ�����ĶԻ���

    private void OnTriggerEnter2D(Collider2D collision)//�����ⷽ��
    {
        if (collision.tag == "Player"|| collision.tag == "Finish")//����⵽ͼ��ΪPlayer����������ʱ
        {
            enterDialog.SetActive(true);//���öԻ���
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//�������뿪��ײ��ʱ
    {
        if (collision.tag == "Player" || collision.tag == "Finish")//����⵽ͼ��ΪPlayer����������ʱ
        {
            enterDialog.SetActive(false);//���öԻ���
        }
    }

}

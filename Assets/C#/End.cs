using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public float spwanTime;//ÿ�����ɼ��ʱ��
    private float countTime;//��¼ʱ��
    private Vector3 spwanPosition;//����λ��

    public GameObject GreenMode;
    public GameObject OtherMode;
    public GameObject KingMode;
    public GameObject winUI;
    public GameObject endTack;
    private int Green;
    private int King;

    bool isGreenStart;
    public static bool isOverSay;

    private void Start()
    {
        isGreenStart = false;
        King = 1;
        Green = 0;
    }
    void Update()
    {
        
        if (isGreenStart ==false&&Green<=1)
        {
            GameObject newPlatform = Instantiate(GreenMode, transform.position, Quaternion.identity);//������������ƽ̨��������ĵ����Ǹ����λ�������ǶȲ��䡣
            isGreenStart = true;
            Green++;
            Invoke("Win", 4);
        }
        if (isOverSay == true && King <= 1&&isGreenStart==true)
        {
            GameObject newPlatform = Instantiate(KingMode, transform.position, Quaternion.identity);//������������ƽ̨��������ĵ����Ǹ����λ�������ǶȲ��䡣
            winUI.SetActive(true);
            King++;
        }
        else if (isOverSay == true && King >= 1 && isGreenStart == true)
        {
            SpwanPlatform();
        }
    }

    void Win()
    {
        endTack.SetActive(true);
    }

    //ƽ̨���ɷ�Χ�����ɼ��
    public void SpwanPlatform()
    {
        countTime += Time.deltaTime;//��¼ʱ��
        spwanPosition = transform.position;//���ɵ�λ�þ͵���������λ��
        spwanPosition.x = Random.Range(-2.5f, 2.5f);//���ɵĵط������������ѡ��

        if (countTime >= spwanTime)
        {
            CreatePlatform();
            countTime = 0;//����һ�κ��������0
        }
    }

    //����ƽ̨����
    public void CreatePlatform()
    {
        GameObject newPlatform = Instantiate(OtherMode, transform.position, Quaternion.identity);//������������ƽ̨��������ĵ����Ǹ����λ�������ǶȲ��䡣
        newPlatform.transform.SetParent(this.gameObject.transform);//�����ɵĶ���ȫ�������Ӽ���
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public float spwanTime;//每次生成间隔时间
    private float countTime;//记录时间
    private Vector3 spwanPosition;//生成位置

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
            GameObject newPlatform = Instantiate(GreenMode, transform.position, Quaternion.identity);//生成这个号码的平台，在上面的到的那个随机位置力，角度不变。
            isGreenStart = true;
            Green++;
            Invoke("Win", 4);
        }
        if (isOverSay == true && King <= 1&&isGreenStart==true)
        {
            GameObject newPlatform = Instantiate(KingMode, transform.position, Quaternion.identity);//生成这个号码的平台，在上面的到的那个随机位置力，角度不变。
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

    //平台生成范围与生成间隔
    public void SpwanPlatform()
    {
        countTime += Time.deltaTime;//记录时间
        spwanPosition = transform.position;//生成的位置就等与该组件的位置
        spwanPosition.x = Random.Range(-2.5f, 2.5f);//生成的地方在这个区间内选择。

        if (countTime >= spwanTime)
        {
            CreatePlatform();
            countTime = 0;//生成一次后计数器清0
        }
    }

    //生成平台方法
    public void CreatePlatform()
    {
        GameObject newPlatform = Instantiate(OtherMode, transform.position, Quaternion.identity);//生成这个号码的平台，在上面的到的那个随机位置力，角度不变。
        newPlatform.transform.SetParent(this.gameObject.transform);//让生成的对象全部放在子集里
    }
}

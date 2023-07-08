using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话逻辑方法
/// </summary>
public class DialogSystem1 : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;//获取文本框内的信息
    public Image faceImage;//获取图像
    private int textNum;//该播放几号文本

    [Header("文本文件")]
    public TextAsset textFile;//获取文本文档
    public int index;//隔壁文档的编号
    public float textSpeed;//文字显示速度

    [Header("头像")]
    public Sprite face01,face02,face03;//获取头像

    bool textFinished;//文字输出完没？

    List<string> textList = new List<string>();//把隔壁文档的代码拆分成一行一行放入列表之中。

    void Awake()//因为OnEnable会在Start之前被调用，所以换成Awake。不然读取不了文本
    {
        GetTextFormFile(textFile);//一开始获取文本文字
    }

    //正常情况下游戏一开始列表内容是为空，故在此把列表变为1，使其读取第一行的内容
    private void OnEnable()//OnEnable()每次开启都会被调用，并且会在Start之前被调用
    {
        //textLabel.text = textList[index];
        //index++;

        StartCoroutine(SetTextUI());//文字一顿一显示的携程
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)&&index == textList.Count)//如果按下R时已经时列表中的最后一句话了
        {
            gameObject.SetActive(false);//那就把他关闭
            Enemy.isSpeech = false;
            index = 0;//对话列数归0以便下次重新开始对话。
            textNum++;
            this.gameObject.SetActive(false);
            return;//不用显示下一句直接结束该方法
        }

        if (Input.GetKeyDown(KeyCode.R)&&textFinished)
        {
            StartCoroutine(SetTextUI());//文字一顿一显示的携程
        }else if(Input.GetKeyDown(KeyCode.R) && !textFinished)//连按两次直接输出所有文本
        {
            textLabel.text = textList[index];
            index++;
            textFinished = true;
        }
    }
    
    //把文本内容打碎成行转换为string【】组塞入列表中
    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();//每次触发前都要把列表清空，不然列表里的语句会不同的积累。
        index = 0;

        var lineDate=file.text.Split('\n');//此意为把文本按行切割,并把其存入字符型的数组之中。

        foreach (var line in lineDate)//把文本的每一行都放进列表之中
        {
            textList.Add(line);
        }
    }

    //文字一字一字出来的携程
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";//列表清空，然后再输出下一句

        ChangeFace();

        for (int i = 0; i < textList[index].Length; i++)//此文本行有多少字就循环多少次
        {
            textLabel.text += textList[index][i];//输出该行的第几个字

            yield return new WaitForSeconds(textSpeed);
        }

        if(textFinished != true)//如果显示全部文本那就不加了
        {
            textFinished = true;
            index++;
        }
    }

    //换头像方法
    void ChangeFace()
    {

        switch (textList[index].Trim().ToString())//用A,B再文档标注换头像，换完后跳过该行
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;
            case "C":
                faceImage.sprite = face03;
                index++;
                break;
            case "End":
                End.isOverSay = true;
                gameObject.SetActive(false);
                
                break;
        }
    }
}

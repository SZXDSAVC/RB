using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Ի��߼�����
/// </summary>
public class DialogSystem1 : MonoBehaviour
{
    [Header("UI���")]
    public Text textLabel;//��ȡ�ı����ڵ���Ϣ
    public Image faceImage;//��ȡͼ��
    private int textNum;//�ò��ż����ı�

    [Header("�ı��ļ�")]
    public TextAsset textFile;//��ȡ�ı��ĵ�
    public int index;//�����ĵ��ı��
    public float textSpeed;//������ʾ�ٶ�

    [Header("ͷ��")]
    public Sprite face01,face02,face03;//��ȡͷ��

    bool textFinished;//���������û��

    List<string> textList = new List<string>();//�Ѹ����ĵ��Ĵ����ֳ�һ��һ�з����б�֮�С�

    void Awake()//��ΪOnEnable����Start֮ǰ�����ã����Ի���Awake����Ȼ��ȡ�����ı�
    {
        GetTextFormFile(textFile);//һ��ʼ��ȡ�ı�����
    }

    //�����������Ϸһ��ʼ�б�������Ϊ�գ����ڴ˰��б��Ϊ1��ʹ���ȡ��һ�е�����
    private void OnEnable()//OnEnable()ÿ�ο������ᱻ���ã����һ���Start֮ǰ������
    {
        //textLabel.text = textList[index];
        //index++;

        StartCoroutine(SetTextUI());//����һ��һ��ʾ��Я��
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)&&index == textList.Count)//�������Rʱ�Ѿ�ʱ�б��е����һ�仰��
        {
            gameObject.SetActive(false);//�ǾͰ����ر�
            Enemy.isSpeech = false;
            index = 0;//�Ի�������0�Ա��´����¿�ʼ�Ի���
            textNum++;
            this.gameObject.SetActive(false);
            return;//������ʾ��һ��ֱ�ӽ����÷���
        }

        if (Input.GetKeyDown(KeyCode.R)&&textFinished)
        {
            StartCoroutine(SetTextUI());//����һ��һ��ʾ��Я��
        }else if(Input.GetKeyDown(KeyCode.R) && !textFinished)//��������ֱ����������ı�
        {
            textLabel.text = textList[index];
            index++;
            textFinished = true;
        }
    }
    
    //���ı����ݴ������ת��Ϊstring�����������б���
    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();//ÿ�δ���ǰ��Ҫ���б���գ���Ȼ�б�������᲻ͬ�Ļ��ۡ�
        index = 0;

        var lineDate=file.text.Split('\n');//����Ϊ���ı������и�,����������ַ��͵�����֮�С�

        foreach (var line in lineDate)//���ı���ÿһ�ж��Ž��б�֮��
        {
            textList.Add(line);
        }
    }

    //����һ��һ�ֳ�����Я��
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";//�б���գ�Ȼ���������һ��

        ChangeFace();

        for (int i = 0; i < textList[index].Length; i++)//���ı����ж����־�ѭ�����ٴ�
        {
            textLabel.text += textList[index][i];//������еĵڼ�����

            yield return new WaitForSeconds(textSpeed);
        }

        if(textFinished != true)//�����ʾȫ���ı��ǾͲ�����
        {
            textFinished = true;
            index++;
        }
    }

    //��ͷ�񷽷�
    void ChangeFace()
    {

        switch (textList[index].Trim().ToString())//��A,B���ĵ���ע��ͷ�񣬻������������
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ÑªÌõ´úÂë
public class HealthBar : MonoBehaviour
{
    public Text healthText;
    public Image life1, life2, life3, life4, life5;
    private int barHp;
    private PlayerContent player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            barHp = player.Hp;
            if (barHp <= 0)
                barHp = 0;
            switch (barHp)
            {
                case 0:
                    life1.enabled = false;
                    life2.enabled = false;
                    life3.enabled = false;
                    life4.enabled = false;
                    life5.enabled = false;
                    healthText.enabled = false;
                    break;
                case 1:
                    life1.enabled = true;
                    life2.enabled = false;
                    life3.enabled = false;
                    life4.enabled = false;
                    life5.enabled = false;
                    healthText.enabled = false;
                    break;
                case 2:
                    life1.enabled = true;
                    life2.enabled = true;
                    life3.enabled = false;
                    life4.enabled = false;
                    life5.enabled = false;
                    healthText.enabled = false;
                    break;
                case 3:
                    life1.enabled = true;
                    life2.enabled = true;
                    life3.enabled = true;
                    life4.enabled = false;
                    life5.enabled = false;
                    healthText.enabled = false;
                    break;
                case 4:
                    life1.enabled = true;
                    life2.enabled = true;
                    life3.enabled = true;
                    life4.enabled = true;
                    life5.enabled = false;
                    healthText.enabled = false;
                    break;
                case 5:
                    life1.enabled = true;
                    life2.enabled = true;
                    life3.enabled = true;
                    life4.enabled = true;
                    life5.enabled = true;
                    healthText.enabled = false;
                    break;
                default:
                    life1.enabled = true;
                    life2.enabled = true;
                    life3.enabled = true;
                    life4.enabled = true;
                    life5.enabled = true;
                    healthText.enabled = true;
                    healthText.text = "+" + (barHp - 5);
                    break;
            }
        }
    }
}

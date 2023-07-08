using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoint : MonoBehaviour
{
    private BoxCollider2D box2D;
    private PlayerContent player;
    void Start()
    {
        box2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //¹¥»÷¼ì²â
        if (collision.CompareTag("Player") && player.playerWin==false&&player.isGameAlive!=false)
        {
            player.playerWin = true;
            player.isGameAlive = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShi : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Diebox")
        {
            Destroy(gameObject);
        }
    }
}

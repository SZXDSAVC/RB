using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenMode : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 720 * Time.deltaTime,Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Diebox")
        {
            Destroy(gameObject);
        }
    }
}

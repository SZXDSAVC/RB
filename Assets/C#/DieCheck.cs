using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCheck : MonoBehaviour
{
    private BoxCollider2D coll;
    private Transform PLayer;
    public GameObject EndUI;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        PLayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PLayer == null)
        {
            EndUI.SetActive(true);
        }
    }
}

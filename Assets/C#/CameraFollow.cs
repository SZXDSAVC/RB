using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ïà»ú´úÂë
public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instence;
    public Transform target;
    //public float smoothing;

    public Vector2 minPostion, maxPostion;

    private void Awake()
    {
        Instence = this;
    }

    void Start()
    {

    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if(transform.position != target.position)
            {

                Vector3 targetPos = target.transform.position;
                targetPos.x = Mathf.Clamp(targetPos.x, minPostion.x, maxPostion.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPostion.y, maxPostion.y);
                //    transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
                transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            }
        }
    }

}

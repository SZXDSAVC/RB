using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAgent : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;//动画播放时角色时不能移动的
    }

 
}

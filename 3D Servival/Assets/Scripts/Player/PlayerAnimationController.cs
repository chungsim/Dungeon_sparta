using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Awake()
    {
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }

    public void RunAnimWalk(bool tf)
    {
        animator.SetBool("IsWalking", tf);
    }

    public void RunAnimJump(bool tf)
    {
        animator.SetBool("IsJumping", tf);
    }

    public void RunAnimClimb(ClimbState climbState)
    {
        
    }
    
}

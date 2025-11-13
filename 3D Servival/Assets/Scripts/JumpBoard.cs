using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoard : MonoBehaviour
{
    public float jumpForce;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            pc.OnJumpBoard(jumpForce);
        }
    }
}

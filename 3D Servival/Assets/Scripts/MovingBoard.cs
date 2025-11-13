using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoard : MonoBehaviour
{
    public Rigidbody _rigidbody;

    public float distance;
    public Vector3 dirV;
    public float time;

    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 previousPosition;
    private Vector3 curVelocity;

    [SerializeField]private Rigidbody playerRigidbody;
    private bool playerOn;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        targetPos = startPos + dirV * distance;

        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        Debug.Log("Go");
        float t = 0f;

        while (t < time)
        {
            t += Time.fixedDeltaTime;
            float per = Mathf.Clamp01(t / time);   // Lerp 범위에 맞추기 위한 정규화
            previousPosition = transform.position;
            transform.position = Vector3.Lerp(startPos, targetPos, per);
            Vector3 vel = (transform.position - previousPosition) / Time.fixedDeltaTime;

            if (playerOn)
            {
                playerRigidbody.MovePosition(playerRigidbody.position + vel*Time.deltaTime);
                Debug.Log(vel);
            }

            yield return new WaitForFixedUpdate();
            
        }

        yield return StartCoroutine(GoBack());
    }

    IEnumerator GoBack()
    {
        float t = 0f;

        while (t < time)
        {

            t += Time.fixedDeltaTime;
            float per = Mathf.Clamp01(t / time);   // Lerp 범위에 맞추기 위한 정규화
            previousPosition = transform.position;
            transform.position = Vector3.Lerp(targetPos, startPos, per);
            Vector3 vel = (transform.position - previousPosition) / Time.fixedDeltaTime;

            if (playerOn)
            {
                playerRigidbody.MovePosition(playerRigidbody.position + vel*Time.deltaTime);
                Debug.Log(vel);
            }
            
            yield return new WaitForFixedUpdate();
            
        }

        yield return StartCoroutine(Go());
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 collisionDirection = -contact.normal;

        Debug.Log($"충돌 방향: {collisionDirection}");

        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc) && collisionDirection.y > 0.9)
        {
            Debug.Log("player on the board");
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            playerOn = true;

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            Debug.Log("player out of board");
            playerRigidbody = null;
            playerOn = false;
        }
    }
}

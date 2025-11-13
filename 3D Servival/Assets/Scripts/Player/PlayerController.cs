using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ClimbState{None, Hold, TopEnd, BottomEnd, LeftEnd, RightEnd}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float passiveSpeed = 0;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;
    public LayerMask WallLayerMask;

    [Header("Look")]
    public Transform cameraContainerF1;
    public Transform cameraContainerF3;
    private Transform curCamera;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;
    public Action inventory;

    [Header("Anim")]
    private PlayerAnimationController animConrol;

    [Header("States")]
    bool canWalking = true;
    public bool isFp = true;

    [SerializeField] private bool _isClimbing;
    [SerializeField] private ClimbState ClimbState = ClimbState.None;
    [SerializeField] bool islaunching = false;



    private Rigidbody _rigidbody;
    // Start is called before the first frame update

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animConrol = GetComponent<PlayerAnimationController>();
    }
    void Start()
    {
        // 마우스 커서 고정 (Locked 모드는 중앙 고정)
        Cursor.lockState = CursorLockMode.Locked;
        curCamera = cameraContainerF1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        if (_isClimbing)
        {
            CheckClimbingState();
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir;
        if (_isClimbing)
        {
            dir = transform.up * curMovementInput.y + transform.right * curMovementInput.x;
            dir *= moveSpeed;
            dir.z = 0.1f;

            _rigidbody.velocity = dir;
            return;
        }

        if (islaunching)
        {
            return;
        }

        dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed + passiveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        
        if (!_isClimbing)
        {
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
            curCamera.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        }
        else
        {
            curCamera.localEulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
            curCamera.localEulerAngles = new Vector3(-camCurXRot, curCamera.localEulerAngles.y, 0);
        }
    }

    public void OnCameraChange(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CameraChange();
        }
    }

    void CameraChange()
    {
        if (cameraContainerF3.gameObject.activeInHierarchy)
        {
            cameraContainerF1.gameObject.SetActive(true);
            cameraContainerF3.gameObject.SetActive(false);
            curCamera = cameraContainerF1;
            isFp = true;
        }
        else
        {
            cameraContainerF1.gameObject.SetActive(false);
            cameraContainerF3.gameObject.SetActive(true);
            curCamera = cameraContainerF3;
            isFp = false;
        } 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && canWalking)
        {
            curMovementInput = context.ReadValue<Vector2>();
            animConrol.RunAnimWalk(true);
        }
        else if (context.phase == InputActionPhase.Canceled || !canWalking)
        {
            curMovementInput = Vector2.zero;
            animConrol.RunAnimWalk(false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_isClimbing)
        {
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(-Vector3.forward * 0.2f, ForceMode.Impulse);
            _isClimbing = false;
            if (cameraContainerF3.gameObject.activeInHierarchy)
            {
                CameraChange();
            }
            return;
        }

        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
        animConrol.RunAnimJump(true);

        if (climbingState() != ClimbState.None)
        {
            //animConrol.RunAnimClimb;
            _isClimbing = true;
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;

            if (cameraContainerF1.gameObject.activeInHierarchy)
            {
                CameraChange();
            }

            return;
        }

        StartCoroutine(JumpEndCheck());
    }

    public void OnJumpBoard(float force)
    {
        _rigidbody.AddForce(Vector2.up * force, ForceMode.Impulse);
        animConrol.RunAnimJump(true);
        StartCoroutine(JumpEndCheck());
    }

    public void OnLauncher(float force, Vector3 dir)
    {
        _rigidbody.transform.position += Vector3.up * 0.01f;
        _rigidbody.AddForce(dir * force, ForceMode.Impulse);
        animConrol.RunAnimJump(true);
        islaunching = true;
        StartCoroutine(JumpEndCheck());
    }

    IEnumerator JumpEndCheck()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (isGrounded())
            {
                islaunching = false;
                animConrol.RunAnimJump(false);
                canWalking = false;
                yield return new WaitForSeconds(0.2f);
                canWalking = true;
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right *0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right *0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    ClimbState climbingState()
    {
        bool[] state = new bool[4];
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 0.01f), Vector3.forward),
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 1.7f), Vector3.forward),
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 0.85f) + (transform.right * 0.2f), Vector3.forward),
            new Ray(transform.position + (transform.forward *0.2f) + (transform.up * 0.85f) - (transform.right * 0.2f), Vector3.forward)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, WallLayerMask))
            {
                state[i] = true;
            }
            else
            {
                state[i] = false;
            }
        }

        if (isGrounded())
        {
            return ClimbState.None;
        }

        if (!(state[0] || state[1] || state[2] || state[3]))
        {
            return ClimbState.None;
        }

        if (state[0] && state[1] && state[2] && state[3])
        {
            return ClimbState.Hold;
        }
        else if (state[0] && state[1])
        {
            if (state[2])
            {
                return ClimbState.LeftEnd;
            }
            else if (state[3])
            {
                return ClimbState.RightEnd;
            }

            return ClimbState.Hold;
        }
        else if (state[0] && !state[1])
        {
            return ClimbState.TopEnd;
        }
        else
        {
            return ClimbState.Hold;
        }
    }
    
    void CheckClimbingState()
    {
        if (climbingState() == ClimbState.None)
        {
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(-Vector3.forward * 0.2f, ForceMode.Impulse);
            _isClimbing = false;
            StartCoroutine(JumpEndCheck());
            return;
        }
        
        if(climbingState() == ClimbState.BottomEnd)
        {
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(-Vector3.forward * 0.2f, ForceMode.Impulse);
            _isClimbing = false;
            StartCoroutine(JumpEndCheck());
            return;
        }   
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}

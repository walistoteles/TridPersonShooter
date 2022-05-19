using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterController controller;
    public float walkSpeed = 3.5f, runSpeed = 5f, slowWalkSpeed = 2;
    public float rotateSpeed;
    public float turnSmoothTime = 0.1f;
    float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float gravity = 6;
    Transform cam;
    Animator anim;
    public bool slowWalk;
    public float distanceCheck;
    public float offset;
    bool Crouch;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleCrouch();

        slowWalk = Input.GetKey(KeyCode.LeftControl);
        anim.SetBool("isGround", Ground());
        anim.SetBool("Crouch", Crouch);
    }

    bool Ground()
    {
        bool r = false;

        Vector3 dis = transform.position;
        dis.y += offset;
        RaycastHit hit;
        if (Physics.Raycast(dis, -transform.up * distanceCheck, out hit))
        {
            if(hit.distance < distanceCheck)
            {
                r = true;
            }
            else
            {
                r = false;
            }


        }

        Debug.DrawRay(dis, -transform.up * distanceCheck, Color.red);
        return r;
    }


    void HandleCrouch()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch = !Crouch;
        }
    }

  
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 v = vertical * cam.transform.forward;
        Vector3 h = horizontal * cam.transform.right;

        Vector3 moveDir = (v + h).normalized;

        float m = Mathf.Abs((v).magnitude) + Mathf.Abs((h).magnitude);
        float moveAmount = Mathf.Clamp01(m);
        Vector3 gravityVector = Vector3.zero;

        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        if (moveDir != Vector3.zero)
        {
            Vector3 targertDir = moveDir;
            targertDir.y = 0;
            if (targertDir == Vector3.zero)
                targertDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targertDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;
        }

        // Vector3 offset = transform.position;

        // offset.y = 1.5f;

        // Debug.DrawRay(offset, moveDir * (3 * moveAmount), Color.white);
        float targetSpeed = 0;
        if (slowWalk)
        {
            targetSpeed = (Input.GetKey(KeyCode.LeftShift) && Crouch == true) ? walkSpeed * moveAmount : slowWalkSpeed * moveAmount;

        }
        else
        {
            if (Crouch)
            {
                targetSpeed = slowWalkSpeed * moveAmount;
            }
            else
            {
                targetSpeed = (Input.GetKey(KeyCode.LeftShift) && Crouch == false) ? runSpeed * moveAmount : walkSpeed * moveAmount;

            }
        }
        

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(moveDir * currentSpeed * Time.deltaTime);
        controller.Move(gravityVector * Time.deltaTime);

        if (slowWalk)
        {
            anim.SetFloat("Speed", 0.2f * moveAmount, speedSmoothTime, Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("Speed", 3f * moveAmount, 0.4f, Time.deltaTime);

            }

        }
        else
        {
            anim.SetFloat("Speed", 0.5f * moveAmount, speedSmoothTime, Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("Speed", 2.3f * moveAmount, 0.4f, Time.deltaTime);

            }

        }

    }
}

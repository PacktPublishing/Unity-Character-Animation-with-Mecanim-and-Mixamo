using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInput : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 5f;
    public float speed;
    //private float rotationSpeed = 1f;

    //public float ySpeed;
    public float jumpSpeed = 15f;
    public float gravity = -9.81f;
    public Vector3 velocity;
    

    public Transform cam;
    public float mouseSensitivity = 2f;
    public float upLimit = -50f;
    public float downLimit = 50f;

    public Animator anim;
    public bool grounded;
    public bool isJumping;

    public float idleTimer;
    public Vector2 idleTimeMinMax = new Vector2(4f, 8f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 movementDirection = transform.forward * v + transform.right * h;
        //Vector3 movementDirection = new Vector3(h, -4f, v);
        controller.Move(movementDirection * walkSpeed * Time.deltaTime);
        
        if (v > 0.1 || v < -0.1f)
        {
            anim.SetFloat("Speed", v);
            
        }
        else {
            speed = 0f;
            anim.SetFloat("Speed", 0);


            //start the timer
            idleTimer += Time.deltaTime;
        }

        //ySpeed += Physics.gravity.y * Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded)
        {
            grounded = true;//just for debugging
            if(isJumping)//if already jumping when it becomes grounded, trigger the land animation
            {
                isJumping = false;
                anim.SetTrigger("Landing");
            }
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Player is jumping");
                velocity.y = jumpSpeed;
                anim.SetTrigger("Jumping");
                Jumping();
                //movementDirection.y = jumpSpeed;
            }
            
        }
        else
        {
            grounded = false;
            //isJumping = true;//want this to be falling
        }
         //   velocity.y = -0.5f;

        float hRotation = Input.GetAxis("Mouse X");
        float vRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0, hRotation * mouseSensitivity, 0);
        cam.Rotate(-vRotation * mouseSensitivity, 0, 0);

        Vector3 currentRotation = cam.localEulerAngles;
        if(currentRotation.x > 180)
        {
            currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
            cam.localRotation = Quaternion.Euler(currentRotation);
        }

        
        
        /*
       if(movementDirection != Vector3.zero)
            //if (movementDirection.x != 0f)
            {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }*/

    }

    void Jumping()
    {
        isJumping = true;
        
    }
}

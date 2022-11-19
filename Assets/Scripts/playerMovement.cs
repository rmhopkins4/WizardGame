using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject wand;

    public GameObject cameraHolder;
    public imageSwitcher dashSwap;
    
    Vector3 localMovement;
    float cameraRot;
    float camVelocity;

    public float speed = 12f;
    public float aerialDampener;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private float moveMultiplier = 1;
    private float gravMultiplier = 1;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    public bool isGrounded;

    bool isDashing;
    public float dashDistance;
    public float dashTime;
    private float dashTimer = 100f;
    public float dashCooldown;
    private float nextTimeToDash;
    private Vector3 dashDirection;
    public GameObject dashForwardEffect, dashBackEffect, dashLeftEffect, dashRightEffect;


    // Update is called once per frame
    void Update()
    {
        if(wand.GetComponent<wand>().fireIsSpawned) //if fireball is in the hand
        {
            if (!isGrounded)
            {
                gravMultiplier = 0.25f;
            }
        }
        else if(!wand.GetComponent<wand>().isLightning)
        {
            gravMultiplier = 1;
        }

        if(wand.GetComponent<wand>().isLightning)
        {
            gravMultiplier = 2f;
            moveMultiplier = 0.5f;
        }
        else if(!wand.GetComponent<wand>().fireIsSpawned)
        {
            gravMultiplier = 1;
            moveMultiplier = 1;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //stick to ground when landing
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float xR = Input.GetAxisRaw("Horizontal");
        float zR = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if(x != 0 && z != 0)
        {
            move.x = move.x / Mathf.Sqrt(2);
            move.z = move.z / Mathf.Sqrt(2);
        }
        if(!isDashing) controller.Move(move * speed * Time.deltaTime * moveMultiplier); //do horiz movement

        float valUse = new Vector2(x, z).normalized.x;
        cameraRot = Mathf.SmoothDamp(cameraRot, valUse * moveMultiplier * 2, ref camVelocity, 0.1f);
        cameraHolder.transform.Rotate(0, 0, -cameraRot, Space.Self);


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * gravMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); //do gravity

        if(Time.time >= nextTimeToDash)
        {
            dashSwap.isAvailable = true;
        }
        else
        {
            dashSwap.isAvailable = false;
        }

        if(Input.GetButtonDown("Fire3") && Time.time >= nextTimeToDash)
        {
            velocity.y = 0;
            //get dash direction
            if(xR == 0 && zR == 0)
            {
                dashDirection = transform.forward;
            }
            else{
                dashDirection = (transform.right * xR + transform.forward * zR).normalized;
            }

            //dash particle logic
            if(xR == 0 && zR == 0)
            {
                dashForwardEffect.GetComponent<ParticleSystem>().Play();
            }
            else if(Mathf.Abs(xR) > Mathf.Abs(zR))
            {
                if(xR > 0)
                {
                    //right
                    dashRightEffect.GetComponent<ParticleSystem>().Play();
                }
                else{
                    //left
                    dashLeftEffect.GetComponent<ParticleSystem>().Play();
                }
            }
            else
            {
                if(zR > 0)
                {
                    //forward
                    dashForwardEffect.GetComponent<ParticleSystem>().Play();
                }
                else{
                    //back
                    dashBackEffect.GetComponent<ParticleSystem>().Play();
                }
            }

            dashTimer = 0;
            isDashing = true;//become dashing
        }

        if(isDashing)
        {
            dashTimer += Time.deltaTime; //dashing time adds up

            controller.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);

            if(dashTimer >= dashTime)
            {
                isDashing = false;
                nextTimeToDash = Time.time + dashCooldown;
            }
        }

    }

}

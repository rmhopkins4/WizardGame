using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour
{
    public GameObject player;
    Animator anim;

    private bool lightningAerialActive;
    private float pastPlayerHeight;
    private float lightningTimer;

    public GameObject lightningOrb;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if(lightningAerialActive)
        {
            anim.Play("lightningOn");
            //lightningOrb.SetActive(true);
            if(player.transform.position.y < pastPlayerHeight)
            {
                lightningTimer += Time.deltaTime;
            }
            pastPlayerHeight = player.transform.position.y;       
            if(player.GetComponent<playerMovement>().isGrounded)
            {
                Debug.Log("landed" + Mathf.Pow(3, lightningTimer));
                lightningAerialActive = false;
                anim.Play("lightningOff");
                //lightningOrb.SetActive(false);
                PunchDone();
            }
        }
    }

    public void aerialPunch()
    {
        Debug.Log("aerial punching");
        lightningTimer = 0;
        pastPlayerHeight = player.transform.position.y;
        lightningAerialActive = true;
        //run punch done at landing time
    }

    public void groundedPunch()
    {
        //run punch done at the end of the animation for the punch
    }

    public void PunchDone()
    {
        GameObject.Find("wand").GetComponent<wand>().lightningFistDone();
    }
}
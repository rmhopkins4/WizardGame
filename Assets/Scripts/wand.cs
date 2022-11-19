using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class wand : MonoBehaviour
{
    Animator anim;
    public Camera fpsCam;
    public GameObject wandTip;
    public LayerMask IgnoreMe;
    public LayerMask rockAble;
    public GameObject player;
    public GameObject lightningFist;
    CameraShakeInstance currentShake;


    public imageSwitcher lightningSwap, fireSwap, dartSwap, rockSwap;


    #region dart
    public float damageDart = 10f;
    public float rangeDart = 100f;
    public float fireRateDart = 15f;
    public ParticleSystem muzzleFlashDart;
    public GameObject Dart;
    private float nextTimeToFireDart = 0f;
    #endregion

    #region grav
    public float damageGrav = 10f;
    public float rangeGrav = 100f;
    public float fireRateGrav = 15f;
    public ParticleSystem muzzleFlashGrav;
    public GameObject impactEffectGrav;
    private float nextTimeToFireGrav = 0f;
    #endregion

    #region fireball
    public float fireRateBall = 0.25f;
    public GameObject Ball;
    private float nextTimeToFireBall = 0f;
    public float chargeTimeBall = 1f;
    public bool fireIsSpawned = false;
    bool timeHasReset = false;
    #endregion

    #region lightning
    public float fireRateLightning = 0.10f;
    private float nextTimeToFireLightning;
    public bool isLightning = false;
    #endregion


    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextTimeToFireGrav)
        {
            rockSwap.isAvailable = true;
        }
        else
        {
            rockSwap.isAvailable = false;
        }

        if(Time.time >= nextTimeToFireDart)
        {
            dartSwap.isAvailable = true;
        }
        else
        {
            dartSwap.isAvailable = false;
        }

        if(Time.time >= nextTimeToFireBall)
        {
            fireSwap.isAvailable = true;
        }
        else
        {
            fireSwap.isAvailable = false;
        }

        if(Time.time >= nextTimeToFireLightning)
        {
            lightningSwap.isAvailable = true;
        }
        else
        {
            lightningSwap.isAvailable = false;
        }

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFireDart)
        {
            //ShootDart();
            anim.Play("wandDart");
        }
        if(Input.GetButton("Fire2") && Time.time >= nextTimeToFireGrav)
        {
            //ShootGrav();
            anim.Play("wandGrav");
        }

        if(Input.GetButtonDown("Ball") && Time.time >= nextTimeToFireBall && !fireIsSpawned && !isLightning)
        {
            GameObject ball = Instantiate(Ball, wandTip.transform.position, Quaternion.identity);
            Fireball fireball = ball.GetComponent<Fireball>();
            timeHasReset = false;
            fireIsSpawned = true;

            currentShake = CameraShaker.Instance.StartShake(2f, 5f, 3f);
        }
        if(!fireIsSpawned && !timeHasReset)
        {
            nextTimeToFireBall = Time.time + 1 / fireRateBall;
            timeHasReset = true;
        }

        if(Input.GetButtonDown("Lightning") && Time.time >= nextTimeToFireLightning)
        {
            if(!fireIsSpawned && !isLightning)
            {
                if(player.GetComponent<playerMovement>() != null)
                {
                    if(player.GetComponent<playerMovement>().isGrounded)
                    {
                        //grounded lightning punch
                        //lightningFist.GetComponent<lightning>().groundedPunch();
                    }
                    else
                    {
                        //aerial lightning punch
                        lightningFist.GetComponent<lightning>().aerialPunch();
                        isLightning = true;

                        currentShake = CameraShaker.Instance.StartShake(2f, 8f, 1f);
                    }
                }
            }
        }

    }

    void ShootGrav()
    {
        nextTimeToFireGrav = Time.time + 1f / fireRateGrav;
        muzzleFlashGrav.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, rangeGrav, rockAble))
        {
            GameObject impactGO = Instantiate(impactEffectGrav, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    void ShootDart()
    {
        nextTimeToFireDart = Time.time + 1f / fireRateDart;
        muzzleFlashDart.Play();

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, rangeDart, ~IgnoreMe))
        {
            GameObject dart = Instantiate(Dart, wandTip.transform.position, Quaternion.identity);
            dart.transform.LookAt(hit.point);
            if(hit.transform.tag == "Enemy")
            {
                dart.GetComponent<dartBehavior>().target = hit.transform.gameObject;
            }
        }
    }

    public void fireBallDone()
    {
        fireIsSpawned = false;
        currentShake.StartFadeOut(0.1f);
    }

    public void lightningFistDone()
    {
        nextTimeToFireLightning = Time.time + 1 / fireRateLightning;
        currentShake.StartFadeOut(0);
        CameraShaker.Instance.ShakeOnce(4f, 10f, 0.1f, 2f);
        isLightning = false;
    }
}

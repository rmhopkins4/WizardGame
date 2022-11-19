using UnityEngine;

public class Fireball : MonoBehaviour
{
    Rigidbody rb;
    GameObject fpsCam;
    public LayerMask IgnoreMe;
    ParticleSystem particlySys;
    Animator anim;
    TrailRenderer trail;

    bool currentlyGrowing = false;

    public float timeToCharge = 2f;
    private float chargingTime;
    bool isCharging = true;
    bool isThrown = false;
    int chargeNum = 0;
    GameObject ballSpot;
    GameObject bookParent;
    bool wantsEarlyThrow = false;
    bool wantsThrow = false;

    public float gravity = -9.81f;
    public float shootSpeed = 100f;

    public float rotationSpeed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballSpot = GameObject.Find("ballSpot");
        bookParent = GameObject.Find("bookParent");
        fpsCam = GameObject.Find("Main Camera");
        particlySys = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        chargingTime = 2 * timeToCharge / 3;

        bookParent.GetComponent<Animator>().Play("bookOpen");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = fpsCam.transform.rotation;

        rb.velocity = new Vector3 (rb.velocity.x , rb.velocity.y + gravity * Time.deltaTime, rb.velocity.z);
        if (!isThrown)
        {
            if (wantsThrow || (Input.GetButton("Ball") && isCharging))
            {
                chargingTime += Time.deltaTime;
                if (chargingTime >= timeToCharge)
                {
                    ChargeSize(chargeNum);
                    chargeNum++;
                    chargingTime = 0;
                }
                transform.position = ballSpot.transform.position;
            }

            if (Input.GetButtonUp("Ball"))
            {
                wantsThrow = true;
                if(chargeNum == 0)
                {
                    wantsEarlyThrow = true;
                }
                isCharging = false;
                if (!wantsEarlyThrow)
                {
                    Throw();
                }
            }

            if (wantsThrow == true && chargeNum > 0 && !currentlyGrowing)
            {
                Throw();
            }

            if(Input.GetButtonDown("Ball"))
            {
                wantsEarlyThrow = false;
            }
        }

        if(isThrown)
        {
            transform.rotation.SetLookRotation(rb.velocity);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).transform.rotation.SetLookRotation(rb.velocity);
            //turn on rings
        }

    }

    public void ChargeSize(int num)
    {
        if(num >= 3)
        {
            wantsThrow = true;
        }
        else
        {
            currentlyGrowing = true;
            if(chargeNum == 0)
            {
                anim.Play("fireballGrow");
            }
            else if(chargeNum == 1)
            {
                anim.Play("fireballGrow2");
            }
            else if(chargeNum == 2)
            {
                anim.Play("fireballGrow3");
            }
            //grow anim
            print("grow" + num);
        }
    }

    public void ChargeDone()
    {
        //will trigger at end of growing animation
        currentlyGrowing = false;
    }


    private void Throw()
    {
        if (!currentlyGrowing)
        {
            var main = particlySys.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            wantsEarlyThrow = false;
            print("throw");
            chargeNum = 0;
            rb.velocity = new Vector3(0, 0, 0);
            //reset velocity

            gameObject.layer = 0;
            trail.enabled = true;
            transform.GetChild(0).gameObject.layer = 0;
            transform.GetChild(1).gameObject.layer = 0;
            var emissionRate = transform.GetChild(0).GetComponent<ParticleSystem>().emission.rateOverTime;
            emissionRate = new ParticleSystem.MinMaxCurve(3* emissionRate.curveMultiplier, emissionRate.curveMin, emissionRate.curveMax);

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 100f, ~IgnoreMe))
            {
                Debug.Log(hit.transform.name);
                rb.velocity = fpsCam.transform.forward * shootSpeed;
            }
            isThrown = true;

            GameObject.Find("wand").GetComponent<wand>().fireBallDone();
            bookParent.GetComponent<Animator>().Play("bookClose");
            transform.localScale = new Vector3(transform.localScale.x * 3, transform.localScale.y * 3, transform.localScale.z * 3);
        }
    }
}

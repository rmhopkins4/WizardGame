using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartBehavior : MonoBehaviour
{
    Rigidbody rb;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = this.transform.forward * 50f;
        if(target != null)
        {
            transform.LookAt(target.transform.position);
        }
    }
}

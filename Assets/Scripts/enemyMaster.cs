using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMaster : MonoBehaviour
{

    public GameObject player;
    public float distance;

    public bool isAngered;

    public NavMeshAgent _agent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);

        isAngered = true;

        if(isAngered){
            _agent.SetDestination(player.transform.position);
        }
        else{
        }
    }
}

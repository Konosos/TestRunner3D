using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        coll=GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<PlayerMoving>().TakeDamage(20);
            coll.enabled=false;
        }
    }
}

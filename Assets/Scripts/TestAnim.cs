using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    private Animator anim;
    private Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        coll=GetComponentInChildren<Collider>();
        anim.SetFloat("MoveZ",-1f);
        StartCoroutine(Attaching());
    }
    private IEnumerator Attaching()
    {
        while(true)
        {
            anim.SetBool("isAttack",true);
            
            coll.enabled=true;
            yield return new WaitForSeconds(1f);
            anim.SetBool("isAttack",false);
            
            coll.enabled=false;
            yield return new WaitForSeconds(1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

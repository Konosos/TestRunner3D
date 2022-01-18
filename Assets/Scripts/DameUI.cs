using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DameUI : MonoBehaviour
{
    public Text dgText;

    private Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        trans=GetComponent<Transform>();
        StartCoroutine(DestroyMySefl(2f));
    }

    // Update is called once per frame
    void Update()
    {
        trans.position=new Vector3(trans.position.x,trans.position.y+Time.deltaTime,trans.position.z);
    }
    private IEnumerator DestroyMySefl(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
    
}

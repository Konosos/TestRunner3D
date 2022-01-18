using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillPan : MonoBehaviour
{
    private GameObject mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam=GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position+mainCam.transform.forward);
    }
}

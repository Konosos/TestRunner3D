using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    private Transform parent;
    [SerializeField]private float mouseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        parent=transform.parent;
        //Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
    private void Rotate()
    {
        float mouseX=Input.GetAxis("Mouse X")*mouseSpeed*Time.deltaTime;
        parent.Rotate(Vector3.up,mouseX);
    }
}

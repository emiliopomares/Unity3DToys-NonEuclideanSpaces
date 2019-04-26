using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffsetter : MonoBehaviour
{
    public Camera master;
    public Camera slave;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slave.transform.position = master.transform.position + offset;
        slave.transform.rotation = master.transform.rotation;
    }
}

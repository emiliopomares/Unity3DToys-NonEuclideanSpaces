using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    public float angSpeed = 80.0f;
    public float linSpeed = 20.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(new Vector3(0, -angSpeed, 0)*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(new Vector3(0, angSpeed, 0) * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(this.transform.rotation * (new Vector3(-linSpeed, 0, 0)) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(this.transform.rotation * (new Vector3(linSpeed, 0, 0)) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //this.transform.Translate(new Vector3(0, 0, linSpeed) * Time.deltaTime, Space.Self);
            this.transform.Translate(this.transform.rotation * (new Vector3(0, 0, linSpeed)) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //this.transform.Translate(new Vector3(0, 0, -linSpeed) * Time.deltaTime, Space.Self);
            this.transform.Translate(this.transform.rotation * (new Vector3(0, 0, -linSpeed)) * Time.deltaTime);
        }


    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDismisser : MonoBehaviour
{
    public float Delay;
    float remain;
    // Start is called before the first frame update
    void Start()
    {
        remain = Delay;
    }

    // Update is called once per frame
    void Update()
    {
        remain -= Time.deltaTime;
        if(remain<0.0f || Input.GetMouseButtonDown(0))
        {
            Destroy(this.gameObject);
        }
    }
}

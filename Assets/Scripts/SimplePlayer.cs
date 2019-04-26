using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float angSpeed = 80.0f;
    public float linSpeed = 20.0f;
    Rigidbody r;
    int previousRegion = 3;
    public SpaceController spaceController;
    TransformParams previousParams;
    // Start is called before the first frame update
    float startMouseX;
    float previousMouseX;
    public float mouseFactor = 1.0f;
    float keyboardRotation = 0.0f;
    void Start()
    {
        r = this.GetComponent<Rigidbody>();
        previousRegion = spaceController.spacePolicy.GetPartition(this.transform.position);
        previousParams = spaceController.spacePolicy.GetTransformParams(this.transform.position);
        startMouseX = previousMouseX = Input.mousePosition.x;
    }


    private void Update() // a little hack to assure smooth transitions between different euclidean partitions
    {
        int region = spaceController.spacePolicy.GetPartition(this.transform.position);

        if (region != previousRegion)
        {
            TransformParams currentParams = spaceController.spacePolicy.GetTransformParams(this.transform.position);
            float currentZ = this.transform.position.z;
            float frontier = currentParams.ZfrontierMax;
            float d1 = Mathf.Abs(currentZ - currentParams.ZfrontierMax);
            float d2 = Mathf.Abs(currentZ - currentParams.ZfrontierMin);
            if (d2 < d1) frontier = currentParams.ZfrontierMin;
            float currentRegionSegment = frontier - currentZ;
            float scaleFactor = currentParams.speedScale.z / previousParams.speedScale.z;
            if (scaleFactor < 1.0f)
            {
                float adjusterRegionSegment = currentRegionSegment * scaleFactor;
                Vector3 newPos = this.transform.position;
                newPos.z = frontier - adjusterRegionSegment;
                this.transform.position = newPos;
            }
            previousParams = currentParams;
            previousRegion = region;
            spaceController.TransformCamera();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float y = r.velocity.y;
        Vector3 newVel = new Vector3(0, y, 0);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, (Input.mousePosition.x - startMouseX) * mouseFactor + keyboardRotation));

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            keyboardRotation -= angSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            keyboardRotation += angSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A)) 
        {
            newVel += this.transform.rotation * (new Vector3(-linSpeed, 0, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            newVel += this.transform.rotation * (new Vector3(linSpeed, 0, 0));
        }
        if (Input.GetKey(KeyCode.W) || Input.GetMouseButtonDown(0))
        {
            //this.transform.Translate(new Vector3(0, 0, linSpeed) * Time.deltaTime, Space.Self);
            newVel += this.transform.rotation * (new Vector3(0, 0, linSpeed));
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetMouseButtonDown(1))
        {
            //this.transform.Translate(new Vector3(0, 0, -linSpeed) * Time.deltaTime, Space.Self);
            newVel += this.transform.rotation * (new Vector3(0, 0, -linSpeed));
        }
        Vector3 scale = spaceController.GetPartitionSpeed(this.transform.position);
        newVel.x *= scale.x;
        newVel.y *= scale.y;
        newVel.z *= scale.z;
        r.velocity = newVel;
        r.angularVelocity = Vector3.zero;

    }
}

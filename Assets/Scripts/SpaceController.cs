using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceController : MonoBehaviour
{
    public Camera master;
    public Camera slave;
    public Renderer masterStencil;
    public Renderer slaveStencil;

    public bool ForceStencil;

    public Text Debug_N;

    public SpacePolicy spacePolicy;

    bool showmaster = true;

    // Start is called before the first frame update
    void Start()
    {
        //showmaster = false;
        //switchCameras();
    }

    public void switchCameras()
    {
        showmaster = !showmaster;
        master.enabled = showmaster;
        slave.enabled = !showmaster;
    }

    bool invertStencil;
    Vector3 partitionSpeed;
    Vector3 lastPartitionSpeed;

    public void TransformCamera()
    {
        TransformParams parms = spacePolicy.GetTransformParams(master.transform.position);
        slave.transform.position = parms.position;
        slave.transform.rotation = master.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        TransformParams parms = spacePolicy.GetTransformParams(master.transform.position);
        TransformParams paramsWithSlack = spacePolicy.GetTransformParams(master.transform.position, true);
        slave.transform.position = parms.position;
        slave.transform.rotation = master.transform.rotation;
        invertStencil = paramsWithSlack.invertStencil;
        partitionSpeed = parms.speedScale;

        if(ForceStencil)
        {
            masterStencil.enabled = true;
            slaveStencil.enabled = true;
        }
        else
        {
            masterStencil.enabled = invertStencil;
            slaveStencil.enabled = invertStencil;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // switchCameras();
        }
        if(Debug_N!=null)
        {
            int part = spacePolicy.GetPartition(master.transform.position);
            Debug_N.text = "" + part + "/" + master.transform.position.z + " / " + slave.transform.position.z;
        }

    }

    public Vector3 GetPartitionSpeed(Vector3 masterPos)
    {

        return partitionSpeed;  
    }

}

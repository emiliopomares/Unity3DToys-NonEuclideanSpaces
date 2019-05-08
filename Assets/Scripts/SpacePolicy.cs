using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformParams
{
    public Vector3 position;
    public Vector3 speedScale;
    public bool invertStencil;
    public float ZfrontierMin;
    public float ZfrontierMax;
}

public abstract class PolicyTransform
{
    public abstract TransformParams Transform(Vector3 masterPos);

}

public class FrontSpacePolicy : PolicyTransform
{
    public override TransformParams Transform(Vector3 masterPos)
    {
        TransformParams res = new TransformParams();
        res.position = masterPos + new Vector3(0, -33.32f, 0);
        res.speedScale = Vector3.one;
        res.ZfrontierMin = 0.0f;
        res.ZfrontierMax = 0.0f;
        res.invertStencil = false;
        return res;
    }

}

public class BackSpacePolicy : PolicyTransform
{
    public override TransformParams Transform(Vector3 masterPos)
    {
        TransformParams res = new TransformParams();
        res.position = masterPos + new Vector3(0, -33.32f, -20);
        res.speedScale = Vector3.one;
        res.invertStencil = false;
        res.ZfrontierMin = -1.0f;
        res.ZfrontierMax = 0.0f;
        return res;
    }
}

public class MiddleSpacePolicy : PolicyTransform
{
    public override TransformParams Transform(Vector3 masterPos)
    {
        TransformParams res = new TransformParams();
        Vector3 intermediate = masterPos;
        intermediate.z *= 21.0f;
        res.position = intermediate + new Vector3(0, -33.32f, 0);
        res.speedScale = new Vector3(1, 1, 1.0f / 14.0f); //0.142857f);
        res.invertStencil = true;
        res.ZfrontierMin = -1.0f;
        res.ZfrontierMax = 0.0f;
        return res;
    }
}

public class SideSpacePolicy : PolicyTransform
{
    public override TransformParams Transform(Vector3 masterPos)
    {
        TransformParams res = new TransformParams();
        Vector3 intermediate = masterPos;
        intermediate.z *= 21.0f;
        res.position = intermediate + new Vector3(0, -33.32f, 0);
        res.speedScale = Vector3.one;
        res.invertStencil = false;
        res.ZfrontierMin = -1.0f;
        res.ZfrontierMax = 0.0f;
        return res;
    }
}

public class Partitioner
{
    FrontSpacePolicy frontPolicy;
    BackSpacePolicy backPolicy;
    MiddleSpacePolicy middlePolicy;
    SideSpacePolicy sidePolicy;

    const float slack = 0.011f; // this has something to do with the camera near clipping plane

    public void Init()
    {
        frontPolicy = new FrontSpacePolicy();
        backPolicy = new BackSpacePolicy();
        middlePolicy = new MiddleSpacePolicy();
        sidePolicy = new SideSpacePolicy();
    }

    public int GetPartition(Vector3 worldPos)
    {
    
        if (worldPos.z > (0.0f)) return 0;
        else if (worldPos.z < (-1.0f)) return 3;
        else
        {
            if (worldPos.x > 0.05f && worldPos.x < 1.2f)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }

    public PolicyTransform GetPartitionPolicy(Vector3 worldPos)
    {
        return GetPartitionPolicy(worldPos, false);
    }

    public PolicyTransform GetPartitionPolicy(Vector3 worldPos, bool withSlack)
    {
        float s = 0.0f;
        if (withSlack) s = slack;
        if (worldPos.z > (0.0f + s)) return frontPolicy;
        else if (worldPos.z < (-1.0f - s)) return backPolicy;
        else
        {
            if (worldPos.x > 0.05f && worldPos.x < 1.2f)
            {
                return middlePolicy;
            }
            else
            {
                return sidePolicy;
            }
        }
    }
}

public class SpacePolicy : MonoBehaviour
{
    PolicyTransform policy;

    Partitioner partitioner;

    public TransformParams GetTransformParams(Vector3 masterPos)
    {
        policy = partitioner.GetPartitionPolicy(masterPos);
        return policy.Transform(masterPos);
    }

    public TransformParams GetTransformParams(Vector3 masterPos, bool withSlack)
    {
        policy = partitioner.GetPartitionPolicy(masterPos, withSlack);
        return policy.Transform(masterPos);
    }

    public int GetPartition(Vector3 worldPos)
    {
        return partitioner.GetPartition(worldPos);
    }

    private void Start()
    {
        policy = new FrontSpacePolicy();
        partitioner = new Partitioner();
        partitioner.Init();
    }
}

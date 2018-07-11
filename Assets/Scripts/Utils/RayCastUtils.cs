using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastUtils {

    public static RaycastHit[] RaycastTo(Transform source, Transform target, int layerMask)
    {
        return RaycastTo(source.position, target.position, layerMask);
    }

    public static RaycastHit[] RaycastTo(Transform source, Vector3 target, int layerMask)
    {
        return RaycastTo(source.position, target, layerMask);
    }

    public static RaycastHit[] RaycastTo(Vector3 source, Vector3 target, int layerMask)
    {
        return RaycastTo(source, target, layerMask, Vector3.Distance(source, target));
    }

    public static RaycastHit[] RaycastTo(Vector3 source, Vector3 target, int layerMask, float distance)
    {
        Vector3 relativePosition = target - source;
        return Physics.RaycastAll(source, relativePosition, distance, layerMask);
    }

    //cast a ray from source to target of length distance, using layerMask and ignoring all hits that happen after one happened on an object on layerMask2
    public static List<RaycastHit> RaycastTo(Vector3 source, Vector3 target, int checkOnThisLayermask, int ignoreHitsAfterThisLayerMask, float distance)
    {
        List<RaycastHit> raycasts = new List<RaycastHit>();
        Vector3 relativePosition = target - source;
        RaycastHit[] hits =Physics.RaycastAll(source, relativePosition, distance, checkOnThisLayermask);

        foreach (RaycastHit hit in hits)
        {
            raycasts.Add(hit);
        }

        raycasts.Sort((x, y) => x.distance.CompareTo(y.distance));

        bool obstructed = false;

        List<RaycastHit> result = new List<RaycastHit>();
        foreach (RaycastHit hit in raycasts)
        {
            if (!obstructed)
            {
                if (ignoreHitsAfterThisLayerMask == (ignoreHitsAfterThisLayerMask | 1 << hit.collider.gameObject.layer))
                {
                    obstructed = true;
                }
            }
            if (!obstructed)
            {
                result.Add(hit);
            }
        }

        return result;
    }
    
}

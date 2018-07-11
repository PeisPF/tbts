using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ClearSight : MonoBehaviour
{
    public static HashSet<TileBFSScript> tilesToClear;
    private List<RaycastHit> actualHits = new List<RaycastHit>();
    public LayerMask layerMask;

    //public float DistanceToPlayer = 5.0f;
    void Update()
    {
        if (TurnManager.GetCurrentPlayer() != null)
        {
            Transform target = TurnManager.GetCurrentPlayer().transform;
            actualHits.AddRange(RaycastTo(target));
            if (tilesToClear != null)
            {
                foreach (TileBFSScript tile in tilesToClear)
                {
                    actualHits.AddRange(RaycastTo(tile.transform));
                }
            }


            foreach (RaycastHit hit in actualHits)
            {
                Renderer R = hit.collider.GetComponent<Renderer>();
                if (R == null)
                    continue;
                AutoTransparent AT = R.GetComponent<AutoTransparent>();
                if (AT == null) // if no script is attached, attach one
                {
                    AT = R.gameObject.AddComponent<AutoTransparent>();
                }
                AT.BeTransparent();
            }
            actualHits.Clear();
        }
    }

    private RaycastHit[] RaycastTo(Transform target)
    {
        Vector3 relativePosition = target.position - transform.position;
        return Physics.RaycastAll(transform.position, relativePosition, Vector3.Distance(transform.position, target.transform.position), layerMask);
    }


}


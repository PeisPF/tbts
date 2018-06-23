using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ClearSight : MonoBehaviour
{
    public static List<Tile> tilesToClear;
    private List<RaycastHit> actualHits = new List<RaycastHit>();
    public LayerMask layerMask;

    //public float DistanceToPlayer = 5.0f;
    void Update()
    {
        if (TurnManager.GetCurrentPlayer() != null)
        {
            Transform target = TurnManager.GetCurrentPlayer().transform;
            actualHits.AddRange(RayCastUtils.RaycastTo(this.transform, target, layerMask));
            if (tilesToClear != null)
            {
                
                foreach (Tile tile in tilesToClear)
                {
                    actualHits.AddRange(RayCastUtils.RaycastTo(this.transform, tile.transform, layerMask));
                }
                //Debug.Log("hit " + actualHits.Count + " walls");
                tilesToClear.Clear();
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

    

}


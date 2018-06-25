using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFogScript : MonoBehaviour {

    public LayerMask fogAndWalls;

    public LayerMask lineOfSightObstructing;

    private Transform rayCastTarget;

    void Start()
    {
        //Init();
        rayCastTarget = new GameObject().transform;
    }

    public void CheckFog()
    {
        List<RaycastHit> actualHits = new List<RaycastHit>();
        rayCastTarget.position = this.transform.position + new Vector3(0.1f, 0, 0);
        for (int i = 0; i < 360; i += 1)
        {
            rayCastTarget.RotateAround(this.transform.position, new Vector3(0, 1, 0), ((float)i));
            actualHits.AddRange(RayCastUtils.RaycastTo(this.transform.position, rayCastTarget.position, fogAndWalls, lineOfSightObstructing, float.PositiveInfinity));
            //Debug.Log("rotated to "+rayCastTarget.position);
        }
        //casto un raggio anche verso il giocatore, per eliminare la nebbia su di lui
        rayCastTarget.position = this.transform.position + new Vector3(0, 2, 0);
        actualHits.AddRange(RayCastUtils.RaycastTo(rayCastTarget.position, this.transform.position, fogAndWalls, lineOfSightObstructing, 1));
        //Debug.Log("CheckFog hit " + actualHits.Count + " items");
        foreach (RaycastHit hit in actualHits)
        {
            Destroy(hit.collider.gameObject);
        }
    }

}

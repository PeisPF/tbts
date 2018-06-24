using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestScript : Item {
    public override float GetInteractionReach()
    {
        return 1f;
    }

    public override int[] InitActionCosts()
    {
        return new int[] { 1};
    }

    public override string[] InitActions()
    {
        return new string[] { "Open" };
    }

    public override void Interact1()
    {
        Debug.Log("YOU FUCKIN' WIN");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

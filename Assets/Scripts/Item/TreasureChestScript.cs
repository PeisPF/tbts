using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public override void Interact1(ActionWithCallback action)
    {
        Debug.Log("YOU FUCKIN' WIN");
        SceneManager.LoadScene(3);
    }

    public override bool isActionPossible(PlayerActionScript player)
    {
        return true;
    }
}

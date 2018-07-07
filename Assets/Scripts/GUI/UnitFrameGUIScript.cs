using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrameGUIScript : MonoBehaviour {

    private PlayerStatusScript player;
    private PlayerActionScript playerActionScript;
    public VerticalLayoutGroup actionPointsLayoutGroup;

    public float alpha;

    private Object obj;

    int previousActionPoints;

    private void Start()
    {
        obj = Resources.Load("UI/ActionPoint");
        previousActionPoints = 0;
    }

    public void SetPlayer(PlayerStatusScript player)
    {
        this.player = player;
    }

    public PlayerStatusScript GetPlayer()
    {
        return player;
    }

    private PlayerActionScript GetPlayerActionScript()
    {
        if (playerActionScript == null)
        {
            playerActionScript = player.GetComponent<PlayerActionScript>();
        }
        return playerActionScript;
    }

    private void Update()
    {
        int actionPoints = GetPlayerActionScript().GetRemainingActionPoints();
        if (previousActionPoints != actionPoints)
        {
            foreach (Transform child in actionPointsLayoutGroup.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            for (int i = 0; i < actionPoints; i++)
            {
                GameObject go = (GameObject)Instantiate(obj);
                go.transform.SetParent(actionPointsLayoutGroup.transform, false);
            }
            previousActionPoints = actionPoints;
        }


        if (IsCurrentTurn())
        {
            this.GetComponent<Image>().color = Color.yellow* alpha;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white* alpha;
        }


    }

    private bool IsCurrentTurn()
    {
        return TurnManager.GetCurrentPlayer().GetComponent<PlayerStatusScript>().Equals(player);
    }
}

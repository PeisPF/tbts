using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrameGUIScript : MonoBehaviour
{

    private PlayerStatusScript player;
    private PlayerActionScript playerActionScript;
    public VerticalLayoutGroup actionPointsLayoutGroup;

    private List<GameObject> actionPointsGameObject;

    public float alpha;

    private UnityEngine.Object obj;

    int previousActionPoints;

    private void Start()
    {
        obj = Resources.Load("UI/ActionPoint");
        previousActionPoints = 0;
        actionPointsGameObject = new List<GameObject>();
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
                actionPointsGameObject = new List<GameObject>();
                GameObject.Destroy(child.gameObject);
            }
            for (int i = 0; i < actionPoints; i++)
            {
                GameObject go = (GameObject)Utils.MyInstantiate(obj);
                go.transform.SetParent(actionPointsLayoutGroup.transform, false);
                actionPointsGameObject.Add(go);
            }
            previousActionPoints = actionPoints;
        }


        if (IsCurrentTurn())
        {
            this.GetComponent<Image>().color = Color.yellow * alpha;
            Blink();

        }
        else
        {
            this.GetComponent<Image>().color = Color.white * alpha;
        }


    }


    private float nextBlink = 0;
    private float blinkFrequency = 0.5f;

    private void Blink()
    {
        if (ActionSelected())
        {
            int cost = TurnManager.GetCurrentPlayer().GetComponent<NewPlayerController>().GetCurrentAction().GetCost();

            if (Time.deltaTime > nextBlink)
            {
                nextBlink = nextBlink + blinkFrequency;

                List<GameObject> temp = actionPointsGameObject.GetRange(0, cost);
                foreach (GameObject actionPoint in temp)
                {
                    if (actionPoint.GetComponent<Image>().enabled)
                    {
                        actionPoint.GetComponent<Image>().enabled = false;
                    }
                    else
                    {
                        actionPoint.GetComponent<Image>().enabled = true;
                    }
                }
            }
            else
            {
                nextBlink -= Time.deltaTime;
            }
        }
    }

    private bool ActionSelected()
    {
        return TurnManager.GetCurrentPlayer().GetComponent<NewPlayerController>().GetCurrentAction() != null;
    }

    private bool IsCurrentTurn()
    {
        return TurnManager.GetCurrentPlayer().GetComponent<PlayerStatusScript>().Equals(player);
    }
}

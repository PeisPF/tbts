using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialScript : MonoBehaviour
{
    private float minEmission = 0.0f;
    private float maxEmission = 0.2f;

    private TileController controller;

    private void Start()
    {
        this.controller = this.gameObject.GetComponent<TileController>();
    }

    public void SetMaterial(bool walkable, bool current, bool path, bool target, bool selectable)
    {
        Color baseColor = Color.black;
        float minEmissionModifier = 0f;
        float maxEmissionModifier = 0f;
        if (walkable)
        {
            if (current)
            {
                baseColor = Color.magenta;
            }
            else if (path)
            {
                baseColor = Color.yellow;
                minEmissionModifier = 0.5f;
                maxEmissionModifier = 0.5f;
            }
            else if (target)
            {
                baseColor = Color.green;
            }
            else if (selectable)
            {
                baseColor = Color.red;
            }
            else
            {
                baseColor = Color.black;
                minEmissionModifier = 0.2f;
            }
            controller.SetEmission(minEmission+ minEmissionModifier, maxEmission+ maxEmissionModifier, baseColor);
        }
        else
        {
            controller.SetEmission(0.2f, 0.2f, Color.black);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialScript : MonoBehaviour
{
    private float minEmission = 0.0f;
    private float maxEmission = 0.2f;
    private Color baseColor;

    private TileController controller;

    private void Start()
    {
        this.controller = this.gameObject.GetComponent<TileController>();
        SetEmission(0.2f, 0.2f, Color.black);
    }

    public void MakeMaterialGlow()
    {
        float emission;
        if (minEmission < maxEmission)
        {
            emission = minEmission + Mathf.PingPong(Time.time / 5, maxEmission - minEmission);
        }
        else
        {
            emission = minEmission;
        }

        GetComponent<Renderer>().material.SetColor("_EmissionColor", baseColor * emission);
    }

    public void SetEmission(float minEmission, float maxEmission, Color baseColor)
    {
        this.minEmission = minEmission;
        this.maxEmission = maxEmission;
        this.baseColor = baseColor;
    }


    public void SetMaterial(/*bool walkable, bool current, bool path, bool target, bool selectable*/)
    {
        Color baseColor = Color.black;
        float minEmissionModifier = 0f;
        float maxEmissionModifier = 0f;
        if (GetComponent<TileStatus>().walkable)
        {
            if (GetComponent<TileStatus>().current)
            {
                baseColor = Color.magenta;
            }
            else if (GetComponent<TileStatus>().path)
            {
                baseColor = Color.yellow;
                minEmissionModifier = 0.5f;
                maxEmissionModifier = 0.5f;
            }
            else if (GetComponent<TileStatus>().target)
            {
                baseColor = Color.green;
            }
            else if (GetComponent<TileStatus>().selectable)
            {
                baseColor = Color.red;
            }
            else
            {
                baseColor = Color.black;
                minEmissionModifier = 0.2f;
            }
            SetEmission(minEmission+ minEmissionModifier, maxEmission+ maxEmissionModifier, baseColor);
        }
        else
        {
            SetEmission(0.2f, 0.2f, Color.black);
        }
    }

}

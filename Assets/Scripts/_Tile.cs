using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /*public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool path = false;*/

    //public List<Tile> adjacencyList = new List<Tile>();

    //BFS
    

    //for A*
    public float f = 0; //g+h
    public float g = 0; //from parent to current
    public float h = 0; //from current to dest

    private float minEmission = 0.0f;
    private float maxEmission = 0.2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    /*void Update()
    {
        if (walkable)
        {
            Color baseColor;
            float emissionModifier = 0f;
            if (current)
            {
                baseColor = Color.magenta;
                //GetComponent<Renderer>().material.color = Color.magenta;
            }
            else if (path)
            {
                baseColor = Color.yellow;
                emissionModifier = 0.5f;
               // GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (target)
            {
                baseColor = Color.green;
                //GetComponent<Renderer>().material.color = Color.green;
            }
            else if (selectable)
            {
                baseColor = Color.red;
                //GetComponent<Renderer>().material.color = Color.red;
            }

            else
            {
                baseColor = Color.black;
                //GetComponent<Renderer>().material.color = Color.white;
            }
            float emission = minEmission+ emissionModifier + Mathf.PingPong(Time.time/5, maxEmission-minEmission);
             //Replace this with whatever you want for your base color at emission level '1'
            GetComponent<Renderer>().material.SetColor("_EmissionColor", baseColor * emission);
        }
    }*/

    /*public void Reset()
    {
        Reset(false);
    }*/

    public void Reset(bool alsoPath)
    {
       
        /* current = false;
         target = false;
         selectable = false;
         if (alsoPath)
         {
             path = false;
         }*/



        f = g = h = 0;
    }

    /*public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }*/

    /*public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, 1 + jumpHeight / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents); //ritorna un array di colliders presenti in una posizione centrata sul primo elemento e con un range pari a halfextents

        foreach (Collider coll in colliders)
        {
            Tile tile = coll.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || tile == target) //controlla se c'è qualcosa sopra la cella, sparando un raggio verso l'alto con lunghezza 1
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }*/
}

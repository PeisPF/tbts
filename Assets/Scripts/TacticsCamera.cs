﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCamera : MonoBehaviour
{
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float moveSpeed = 10;
    public Vector3 offset = new Vector3(20, 20, -12);
    //public Vector3 offset = new Vector3(0, 10, -7.50f);
    public float rotationSpeed = 80;
    public float zoomSpeed = 0.2f;

    public Vector3 rotationVector = new Vector3();

    private Vector3 previousTargetPosition;


    public void Update()
    {
        AdjustCameraZoomWithMouseWheel();
        if (TurnManager.GetCurrentPlayer())
        {
            CenterCameraOnCurrentPlayer();
        }
        /*  if (Input.GetKey("e"))
         {
             Debug.Log("pressed");
             TurnCamera(1);
         }

         if (Input.GetKey("q"))
         {
             Debug.Log("pressed");
             TurnCamera(-1);
         }
     */
    }

    private void AdjustCameraZoomWithMouseWheel()
    {
        //for ortographic
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize + zoomSpeed, 8);

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize - zoomSpeed, 3);
        }

        //for perspective
        /*if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            Camera.main.fieldOfView = Mathf.Min(Camera.main.fieldOfView + zoomSpeed, 30);

        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.fieldOfView = Mathf.Max(Camera.main.fieldOfView - zoomSpeed, 7);
        }*/
    }

    private void TurnCamera(float value)
    {
        transform.RotateAround(TurnManager.GetCurrentPlayer().transform.position, new Vector3(0.0f, 1.0f, 0.0f), value * Time.deltaTime * rotationSpeed);
        offset = transform.position - TurnManager.GetCurrentPlayer().transform.position;
    }

    private void CenterCameraOnCurrentPlayer()
    {

        //this.GetComponent<RTS_Cam.RTS_Camera>().targetFollow = TurnManager.GetCurrentPlayer().transform;

        Vector3 targetPosition = TurnManager.GetCurrentPlayer().transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void Start()
    {

    }

    public void RotateLeft()
    {
        transform.RotateAround(TurnManager.GetCurrentPlayer().transform.position, new Vector3(0.0f, 1.0f, 0.0f), 90);
        offset = transform.position - TurnManager.GetCurrentPlayer().transform.position;
    }

    public void RotateRight()
    {
        transform.RotateAround(TurnManager.GetCurrentPlayer().transform.position, new Vector3(0.0f, -1.0f, 0.0f), 90);
        offset = transform.position - TurnManager.GetCurrentPlayer().transform.position;
    }

}

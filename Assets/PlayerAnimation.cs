using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private Animator mAnimator;
    private PlayerMove mv;
    // Use this for initialization
    void Start () {
        mAnimator = GetComponent<Animator>();
        GameObject thePlayer = GameObject.Find("Player");
        mv = thePlayer.GetComponent<PlayerMove>();
    }
	
	// Update is called once per frame
	void Update () {
        bool walking = mv.isMoving();
        mAnimator.SetBool("walking", walking);
        if (mv.isOpeningADoor())
        {
            mv.setOpeningADoor(false);
            mAnimator.SetTrigger("openingADoor");
        }
    }
}

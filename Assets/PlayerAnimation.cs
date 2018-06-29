using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator mAnimator;
    private PlayerStatusScript mv;
    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();

        //questa credo sia una pessima practice, consideriamo che lo script è associato già al gameobject
        //GameObject thePlayer = GameObject.Find("Player");
        //mv = thePlayer.GetComponent<PlayerMove>();
        //quindi si può fare come mostro nella update


    }

    // Update is called once per frame
    void Update()
    {
        if (mv == null)
        {
            mv = this.gameObject.GetComponentInParent<PlayerStatusScript>();
        }


        bool walking = mv.IsMoving();
        mAnimator.SetBool("walking", walking);
        if (mv.IsInteractingWithObject())
        {
            mv.SetInteractingWithObject(false);
            mAnimator.SetTrigger("openingADoor");
        }
    }
}

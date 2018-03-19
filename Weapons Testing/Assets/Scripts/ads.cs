using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ads : MonoBehaviour {

    public Animator animator;

    public static bool isAds = false;

    void Update ()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isAds = !isAds;
            animator.SetBool("ads", isAds);
        }
    }
}
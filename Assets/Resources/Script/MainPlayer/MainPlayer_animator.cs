using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer_animator : MonoBehaviour
{
    Animator animator;
    MainPlayer mainPlayer;
    Pause pause;
    //=========================================================
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>(); // 애니매이터 사용.
        mainPlayer = GameObject.FindWithTag("Player").GetComponent<MainPlayer>();
        pause = GameObject.FindWithTag("UI").GetComponent<Pause>();
    }
    // Update is called once per frame
    void Update()
    {
            Hit();
    }
    public void Hit()
    {
        if(mainPlayer.stop)
        {
            animator.SetBool("stop", true);
        }
        else if(!mainPlayer.stop)
        {
            animator.SetBool("stop", false);
        }
        if(mainPlayer.DoubleJump)
        {
            animator.SetBool("DoubleJump", true);
        }
        else if (!mainPlayer.DoubleJump)
        {
            animator.SetBool("DoubleJump", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public AnimatorOverrideController attackOverrideController;
    
    private RuntimeAnimatorController originalController;
    private bool isAttacking = false;

    void Start()
    {
        originalController = animator.runtimeAnimatorController;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Alpha1)) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.runtimeAnimatorController = attackOverrideController;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(.5f); 

        animator.runtimeAnimatorController = originalController;
        isAttacking = false;
    }
}

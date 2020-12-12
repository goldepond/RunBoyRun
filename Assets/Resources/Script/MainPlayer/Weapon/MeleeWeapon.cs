using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private Animator animator;
    public float attackRange;
    public Transform attaclPos;
    public LayerMask WhatIsEnemies;

    private void Start()
    {
        animator = GetComponent<Animator>();// 애니매이터 사용.

    }
    private void Update()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attaclPos.position, attackRange, WhatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (Input.GetKeyUp(KeyCode.B) == true)
            {
                enemiesToDamage[i].GetComponent<MonsterScript>().Die();
            }
        }


        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            animator.SetTrigger("attack");
        }

    }
    ///=============================================================================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attaclPos.position, attackRange);

    }
}

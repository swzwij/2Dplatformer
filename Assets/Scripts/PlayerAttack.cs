using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _timeBtwAttack;
    [SerializeField] private float _startTimeBtwAttack;

    [SerializeField] private Transform _attackPos;
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private float _attackRange;

    public int damage;

    private void Update()
    {
        if(_timeBtwAttack <= 0)
        {
            //print("TimeBtwAttack = 0");

            if (Input.GetKey(KeyCode.X))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _whatIsEnemy);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            _timeBtwAttack = _startTimeBtwAttack;
        }
        else
        {
            _timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }
}

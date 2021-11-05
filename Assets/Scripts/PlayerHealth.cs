using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int startHealth = 100;
    [SerializeField] private int _spikeDmg = 10;

    private void Start()
    {
        health = startHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            health -= _spikeDmg;
        }
    }
}

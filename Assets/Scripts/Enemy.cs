using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    [SerializeField] private float _speed;
    [SerializeField] private float _startSpeed;
    [SerializeField] private ParticleSystem _bloodEffect;

    [SerializeField] private float _dazedTime;
    [SerializeField] private float _startDazedTime;

    private void Update()
    {
        if(_dazedTime <= 0)
        {
            _speed = _startSpeed;
        }
        else
        {
            _speed = 0;
            _dazedTime -= Time.deltaTime;
        }

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        _dazedTime = _startDazedTime;
        _bloodEffect.Play();
        health -= damage;
        Debug.Log("dmg");
    }
}

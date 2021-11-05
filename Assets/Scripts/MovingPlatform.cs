using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _pos1, _pos2;
    [SerializeField] private float _speed;
    [SerializeField] private  Transform _startPos;
    Vector3 nextPos;

    private void Start()
    {
        nextPos = _startPos.position;
    }

    private void Update()
    {
        if(transform.position == _pos1.position)
        {
            nextPos = _pos2.position;
        }
        if(transform.position == _pos2.position)
        {
            nextPos = _pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
    }

}

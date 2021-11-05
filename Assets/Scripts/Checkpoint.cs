using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager cm;
    [SerializeField] private GameObject _flag;

    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            cm.lastCheckpointPos = transform.position;
            _flag.SetActive(true);
        }
    }
}

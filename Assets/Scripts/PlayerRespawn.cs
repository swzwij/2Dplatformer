using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private CheckpointManager cm;

    private void Update()
    {
        GameObject thePlayer = GameObject.Find("Player");
        PlayerHealth playerHealth = thePlayer.GetComponent<PlayerHealth>();

        if(playerHealth.health <= 0)
        {
            playerHealth.health = playerHealth.startHealth;
            cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
            transform.position = cm.lastCheckpointPos;
        }
    }
}

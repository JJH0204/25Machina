using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneObject : MonoBehaviour
{
    private PlayerController player = null;

    private void OnDisable()
    {
        if (player)
        {
            player.SetPlayerState(EPlayerState.Invincibility, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player)
            {
                player.SetPlayerState(EPlayerState.Invincibility, true);
                this.player = player;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player)
            {
                player.SetPlayerState(EPlayerState.Invincibility, false);
            }
        }
    }
}

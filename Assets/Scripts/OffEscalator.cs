using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffEscalator : MonoBehaviour
{
    public Transform escalatorOffPoint;
    public Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.interacting)
        {
            player.escalatorInteractPoint = escalatorOffPoint;
            player.OffEscalator();
        }
    }
}

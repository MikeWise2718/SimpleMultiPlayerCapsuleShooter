using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    private int currentHealth = maxHealth;

    public bool destroyOnDeath;

    public RectTransform healthBar;

    // Use this for initialization



    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        Debug.Log("Current Health:"+currentHealth);
        if (currentHealth <= 0) { 
            currentHealth = 0;
            if (destroyOnDeath)
            {
                Debug.Log("Dead - destroying !");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Dead - respawning !");
                RpcRespawn();
            }
        }
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }


    [ClientRpc]
    void RpcRespawn()
    {
        Debug.Log("RpcRespawn called:"+isLocalPlayer);
        if (isLocalPlayer)
        {
            // move back to zero location
  //          transform.position = Vector3.zero;
            currentHealth = maxHealth;
        }
    }
}

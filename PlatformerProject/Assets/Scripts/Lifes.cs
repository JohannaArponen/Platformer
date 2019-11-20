using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifes : MonoBehaviour{

    [HideInInspector]
    public float startHealth = 100;
    private float health;

    public GameObject deathEffect;

    [Header("Player Health")]
    public Image healthBar;

    void Start() {
        health = startHealth;
    }
    public void DamagePlayer(float amount) {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if(health <= 0) {
            Die();
        }
    }
}

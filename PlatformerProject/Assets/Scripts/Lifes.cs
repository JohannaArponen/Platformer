using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour{

    [System.Serializable]
    public class PlayerStats{
        public int Health = 100;
    }

    public PlayerStats playerStats = PlayerStats();

    public int fallBoundary = -20;

    void Update() {
        if (transform.position.y <= -20) {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer(int damage) {
        playerStats.Health -= damage;
        if (playerStats.Health <= 0) {
            GameMaster.KillPlayer(this);
        }
    }
}

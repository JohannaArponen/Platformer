using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class Lifes : MonoBehaviour{

    //public Text centerText;

    [HideInInspector]
    public float startHealth = 100;
    private float health;
    public GameObject healthElement;
    public Canvas canvas;
    
    public float gap = 5;
    private List<GameObject> elements = new List<GameObject>();

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
    void UpdateHealth() {
        foreach(var element in elements) {
            Destroy(element);
        }
        elements.Clear();

        for(int i = 0; i < health; i++) {
            var element = Instantiate(healthElement, healthElement.transform.position, healthElement.transform.rotation);
            element.transform.SetParent(canvas.transform, false);

            var rect = element.GetComponent<RectTransform>();
            rect.anchoredPosition -= new Vector2(i * (gap + rect.sizeDelta.x), 0);
            elements.Add(element);
        }
    }
    void Die() {
       /* health--;
        if(health <= 0) {
            string str = "Game Over!";
            print("Game Over");
            centerText.text = str;
            Time.timeScale = 0f;
        }*/
        print("Die!!");
    }
}

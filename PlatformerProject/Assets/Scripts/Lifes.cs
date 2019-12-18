using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class Lifes : MonoBehaviour {

  public Text centerText;

  public float startHealth = 100;
  public float health;
  public GameObject healthElement;
  public Canvas canvas;
  [Tooltip("Optional")]
  public Physics2DCharacter phys;
  public bool handleDie = true;

  public float gap = 5;
  public float invulnerableDuration = 0.5f;
  public float onHitPushForce = 0.5f;
  private float invulnerableStart = float.NegativeInfinity;
  private List<GameObject> elements = new List<GameObject>();

  public GameObject deathEffect;

  [Header("Player Health")]
  public Image healthBar;

  void Start() {
    if (phys == null) phys = GetComponent<Physics2DCharacter>();
    health = startHealth;
  }
  public void DamagePlayer(float amount, GameObject source) {
    if (invulnerableStart > Time.time - invulnerableDuration) return;

    if (phys != null) {
      phys.velocity += (float2)((source.transform.position - transform.position).xy() * onHitPushForce);
    }

    invulnerableStart = Time.time;
    health -= amount;
    healthBar.fillAmount = health / startHealth;
    if (health <= 0) {
      if (handleDie) Die();
    }
  }
  void UpdateHealth() {
    foreach (var element in elements) {
      Destroy(element);
    }
    elements.Clear();

    for (int i = 0; i < health; i++) {
      var element = Instantiate(healthElement, healthElement.transform.position, healthElement.transform.rotation);
      element.transform.SetParent(canvas.transform, false);

      var rect = element.GetComponent<RectTransform>();
      rect.anchoredPosition -= new Vector2(i * (gap + rect.sizeDelta.x), 0);
      elements.Add(element);
    }
  }
  void Die() {
    health--;
    if (health <= 0) {
      string str = "Game Over!";
      print("Game Over");
      centerText.text = str;
      Time.timeScale = 0f;
    }
    print("Die!!");
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

  public Vector2 center = Vector2.zero;
  public Vector2 dimensions = Vector2.zero;
  public float damage = 5;
  [HideInInspector]
  public Rect room;
  [HideInInspector]
  public GameObject player;
  [HideInInspector]
  public Animator animator;
  [HideInInspector]
  public Lifes lifes;
  [HideInInspector]
  public Rigidbody2D rb;


  [HideInInspector]
  public bool dead = false;
  [HideInInspector]
  public bool active = false;
  private bool hasNotBeenFunckingSpawned = true;


  void OnDrawGizmosSelected() {
    Gizmos.DrawRay(room.min, Vector3.right * room.width);
    Gizmos.DrawRay(room.min, Vector3.up * room.height);
    Gizmos.DrawRay(room.max, Vector3.left * room.width);
    Gizmos.DrawRay(room.max, Vector3.down * room.height);
  }

  void OnValidate() {
    room = new Rect(transform.position.xy() + center - dimensions / 2, dimensions);
  }

  // Start is called before the first frame update
  void Start() {
    room = new Rect(transform.position.xy() + center - dimensions / 2, dimensions);
    player = GameObject.FindGameObjectWithTag("Player");
    animator = GetComponent<Animator>();
    lifes = GetComponent<Lifes>();
    rb = GetComponent<Rigidbody2D>();
    animator.Play("Spawn");
    animator.enabled = false;
  }

  void Update() {
    if (dead) return;
    if (hasNotBeenFunckingSpawned && room.Contains(player.transform.position.xy())) {
      Spawn();
      return;
    }
    if (!active) return;
    var results = new List<Collider2D>();
    rb.OverlapCollider(new ContactFilter2D(), results);
    bool hitPlayer = false;
    bool gotHit = false;
    foreach (var col in results) {
      if (col.tag == "PlayerWeapon") {
        var weap = col.gameObject.GetComponent<Weapon>();
        gotHit = true;
        this.lifes.DamagePlayer(weap.damage, col.gameObject);
        break;
      } else if (col.tag == "Player") {
        hitPlayer = true;
      }
    }
    if (!gotHit && hitPlayer) player.GetComponent<Lifes>().DamagePlayer(damage, gameObject);
    ResetAll();
    var rand = Random.Range(0, 7);
    switch (rand) {
      case 1:
        ExtendHead();
        break;
      case 2:
        ExtendArmL();
        break;
      case 3:
        ExtendArmR();
        break;
      case 4:
        ExtendTailL();
        break;
      case 5:
        ExtendTailR();
        break;
      case 6:
        StartSpiders();
        break;
    }

    if (lifes.health <= 0) {
      Die();
    }
  }


  [MyBox.ButtonMethod]
  void Spawn() {
    hasNotBeenFunckingSpawned = false;
    animator.enabled = true;
    animator.Play("Spawn");
    active = true;
  }
  [MyBox.ButtonMethod]
  void Die() {
    if (dead) return;
    animator.Play("Death");
    active = false;
  }


  void ExtendHead() {
    animator.SetBool("Head_Extend", true);
  }
  void ExtendArmL() {
    animator.SetBool("L_Arm_Extend", true);
  }
  void ExtendArmR() {
    animator.SetBool("R_Arm_Extend", true);
  }
  void ExtendTailL() {
    animator.SetBool("L_Tail_Extend", true);
  }
  void ExtendTailR() {
    animator.SetBool("R_Tail_Extend", true);
  }
  void StartSpiders() {
    animator.SetBool("Spiders", true);
  }

  void ResetAll() {
    animator.SetBool("Head_Extend", false);
    animator.SetBool("L_Arm_Extend", false);
    animator.SetBool("R_Arm_Extend", false);
    animator.SetBool("L_Tail_Extend", false);
    animator.SetBool("R_Tail_Extend", false);
    animator.SetBool("Spiders", false);
  }

  // Update is called once per frame
  public void DeletSpiders() {
    animator.SetBool("SpiderEnd", true);
    animator.SetBool("Spiders", false);
  }
}

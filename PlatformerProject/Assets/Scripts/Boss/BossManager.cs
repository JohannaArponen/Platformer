using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

  public Vector2 center = Vector2.zero;
  public Vector2 dimensions = Vector2.zero;
  [HideInInspector]
  public Rect room;
  [HideInInspector]
  public GameObject player;
  [HideInInspector]
  public Animator animator;

  [HideInInspector]
  public bool dead = false;
  [HideInInspector]
  public bool active = false;


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
    animator.Play("Spawn");
    animator.enabled = false;
  }

  void EarlyUpdate() {
    animator.SetBool("SpiderEnd", false);
  }


  [MyBox.ButtonMethod]
  void Spawn() {
    animator.enabled = true;
    animator.Play("Spawn");
    active = true;
  }
  [MyBox.ButtonMethod]
  void Die() {
    animator.Play("Death");
    dead = true;
  }
  [MyBox.ButtonMethod]
  void BackToIdle() {
    animator.Play("Idle");
  }

  [MyBox.ButtonMethod]
  void ExtendHead() {
    animator.Play("Head_Extend");
  }
  [MyBox.ButtonMethod]
  void ExtendArmL() {
    animator.Play("L_Arm_Extend");
  }
  [MyBox.ButtonMethod]
  void ExtendArmR() {
    animator.Play("R_Arm_Extend");
  }
  [MyBox.ButtonMethod]
  void ExtendTailL() {
    animator.Play("L_Tail_Extend");
  }
  [MyBox.ButtonMethod]
  void ExtendTailR() {
    animator.Play("R_Tail_Extend");
  }
  [MyBox.ButtonMethod]
  void StartSpiders() {
    animator.Play("Spiders");
  }

  // Update is called once per frame
  public void DeletSpiders() {
    animator.SetBool("SpiderEnd", true);
  }
}

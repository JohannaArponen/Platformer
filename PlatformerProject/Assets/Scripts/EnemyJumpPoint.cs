using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyJumpPoint : MonoBehaviour {
  [HideInInspector] public bool right = true;
  [HideInInspector] public float extraForce = 0;
  [ButtonMethod]
  void ChangeDirection() {
    right = !right;
    GetComponent<SpriteRenderer>().flipX = !right;
  }

  void Start() {
    GetComponent<SpriteRenderer>().enabled = false;
  }
}

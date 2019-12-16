using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class AttackTrigger : MonoBehaviour {
  public Collider2D trigger;
  public Weapon weapon;

  void Reset() {
    if (weapon == null) weapon = gameObject.GetComponentInChildren<Weapon>();
  }

  void Start() {
    trigger = gameObject.GetComponentInChildren<Collider2D>();
  }


  void OnTriggerEnter(Collider other) {
    print(other);
    weapon.Attack();
  }
}

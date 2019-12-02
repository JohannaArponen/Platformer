using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weapon : MonoBehaviour {

  public KeyCode attack = KeyCode.X;
  public float damage = 1;
  public float startAngle;
  public float endAngle;
  public float duration;
  public float cooldown;

  private Transform parent;
  private float attackStart = 0;
  private bool attacking = false;

  // Start is called before the first frame update
  void Start() {
    parent = transform.parent;
    if (parent == null) throw new UnityException("Attack requires a parent which is used as a pivot point!");
  }

  // Update is called once per frame
  void Update() {
    if (attacking) {
      var fraction = (Time.time - attackStart) / duration;
      if (fraction <= 1) {
        var angle = (endAngle - startAngle) * fraction;
        parent.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      } else {
        attacking = false;
        parent.localRotation = Quaternion.AngleAxis(endAngle, Vector3.forward);
      }
    } else if (attackStart + duration + cooldown < Time.time && Input.GetKey(attack)) {
      attackStart = Time.time;
      attacking = true;
      parent.localRotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
    }
  }
}

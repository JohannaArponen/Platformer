using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weapon : MonoBehaviour {

  [Tooltip("Optional. Does animation. Attack boolean value is passed as \"Attack\"")]
  public Animator animator;
  public KeyCode key = KeyCode.X;
  public float damage = 1;
  public float startAngle = 45;
  public float endAngle = 135;
  public float duration = 0.25f;
  public float cooldown = 0.2f;

  [Tooltip("By default the component on this gameObject is used")]
  public Collider2D col;
  [Tooltip("Disable these behaviours when starting attack and enable them at the end of attack. ALREADY DISABLED BEHAVIOURS ARE NOT REENABLED")]
  public Behaviour[] disableBehaviours;
  private List<Behaviour> toEnable = new List<Behaviour>();
  [Tooltip("Enable these behaviours when starting attack and disable them at the end of attack. ALREADY ENABLED BEHAVIOURS ARE NOT REDISABLED")]
  public Behaviour[] enableBehaviours;
  private List<Behaviour> toDisable = new List<Behaviour>();

  [HideInInspector]
  public bool disallowAttack;

  private Transform parent;
  private float attackStart = 0;
  private bool attacking = false;



  // Start is called before the first frame update
  void Start() {
    if (col == null) col = GetComponent<Collider2D>();
    parent = transform.parent;
    if (parent == null) throw new UnityException("Attack requires a parent which is used as a pivot point!");

    foreach (var comp in disableBehaviours)
      comp.enabled = true;
    foreach (var comp in enableBehaviours)
      comp.enabled = false;
  }

  // Update is called once per frame
  void Update() {
    if (attacking) {
      var fraction = (Time.time - attackStart) / duration;
      if (animator != null) animator.SetFloat("Attack", fraction);
      if (fraction <= 1) {
        var angle = startAngle + (endAngle - startAngle) * fraction;
        parent.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      } else {
        attacking = false;
        foreach (var comp in toEnable)
          comp.enabled = true;
        foreach (var comp in toDisable)
          comp.enabled = false;
        parent.localRotation = Quaternion.AngleAxis(endAngle, Vector3.forward);
      }
    } else if (!disallowAttack && attackStart + duration + cooldown < Time.time && Input.GetKey(key)) {
      attackStart = Time.time;
      attacking = true;
      toEnable.Clear();
      foreach (var comp in disableBehaviours) {
        if (comp.enabled) {
          comp.enabled = false;
          toEnable.Add(comp);
        }
      }
      foreach (var comp in enableBehaviours) {
        if (!comp.enabled) {
          comp.enabled = true;
          toDisable.Add(comp);
        }
      }
      if (animator != null) animator.SetFloat("Attack", 0.0001f);
      parent.localRotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Weapon : MonoBehaviour {

  public KeyCode key = KeyCode.X;
  public float damage = 1;
  public float startAngle = 45;
  public float endAngle = 135;
  public float duration = 0.25f;
  public float cooldown = 0.2f;
  [Tooltip("Whether or not to disable collider and spriterenderer when not attacking")]
  public bool hide = true;

  [Tooltip("By default the component on this gameObject is used")]
  public Collider2D col;
  [Tooltip("By default the component on this gameObject is used")]
  public SpriteRenderer sr;

  [HideInInspector]
  public bool disallowAttack;

  private Transform parent;
  private float attackStart = 0;
  private bool attacking = false;



  // Start is called before the first frame update
  void Start() {
    if (col == null) col = GetComponent<Collider2D>();
    if (sr == null) sr = GetComponent<SpriteRenderer>();
    if (hide) Hide();
    parent = transform.parent;
    if (parent == null) throw new UnityException("Attack requires a parent which is used as a pivot point!");
  }

  void Hide(bool enable = false) {
    sr.enabled = enable;
    col.enabled = enable;
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
        if (hide) Hide();
      }
    } else if (!disallowAttack && attackStart + duration + cooldown < Time.time && Input.GetKey(key)) {
      attackStart = Time.time;
      attacking = true;
      parent.localRotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
      if (hide) Hide(true);
    }
  }
}

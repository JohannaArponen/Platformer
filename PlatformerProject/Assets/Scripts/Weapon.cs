using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Weapon : MonoBehaviour {

  [Tooltip("Optional. Does animation. Attack boolean value is passed as \"Attack\"")]
  public Animator animator;
  public KeyCode key = KeyCode.X;
  public float damage = 1;
  public float startAngle = 45;
  public float endAngle = 135;
  public float duration = 0.25f;
  public float cooldown = 0.2f;

  [Tooltip("Disable these behaviours or renderers when starting attack and enable them at the end of attack. ALREADY DISABLED OBJECTS ARE NOT REENABLED")]
  public Object[] disableObjects;
  private List<Object> toEnable = new List<Object>();
  [Tooltip("Enable these objects when starting attack and disable them at the end of attack. ALREADY ENABLED OBJECTS ARE NOT REDISABLED")]
  public Object[] enableObjects;
  private List<Object> toDisable = new List<Object>();

  [HideInInspector]
  public bool disallowAttack;

  private Transform parent;
  private float attackStart = 0;
  private bool attacking = false;
  private bool doAttack = false;



  // Start is called before the first frame update
  void Start() {
    parent = transform.parent;
    if (parent == null) throw new UnityException("Weapon requires a parent which is used as a pivot point for rotation!");

    foreach (var comp in disableObjects)
      Enable(comp, false);
    foreach (var comp in enableObjects)
      Disable(comp, false);
  }

  /// <summary> Initiates an attack if possible (Enables a bool if cooldowns and other stuff have expired) </summary>
  public bool Attack() {
    if (!disallowAttack && attackStart + duration + cooldown < Time.time) {
      doAttack = true;
      return true;
    }
    return false;
  }

  /// <summary> Force starts attack </summary>
  public bool ForceAttack() {
    if (!disallowAttack && attackStart + duration + cooldown < Time.time) {
      doAttack = true;
      return true;
    }
    return false;
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
          Enable(comp, false);
        foreach (var comp in toDisable)
          Disable(comp, false);
        parent.localRotation = Quaternion.AngleAxis(endAngle, Vector3.forward);
      }
    } else {

      if (Input.GetKey(key))
        Attack();

      if (doAttack) {
        doAttack = false;
        attackStart = Time.time;
        attacking = true;
        toEnable.Clear();

        foreach (var comp in disableObjects)
          Disable(comp);

        foreach (var comp in enableObjects)
          Enable(comp);

        if (animator != null) animator.SetFloat("Attack", 0.0001f);
        parent.localRotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
      }
    }
  }

  void Disable(Object comp, bool listForActivate = true) {
    if (comp is Renderer) {
      if (((Renderer)comp).enabled) {
        ((Renderer)comp).enabled = false;
        if (listForActivate) toEnable.Add(comp);
      }
    } else if (comp is Behaviour) {
      if (((Behaviour)comp).enabled) {
        ((Behaviour)comp).enabled = false;
        if (listForActivate) toEnable.Add(comp);
      }
    } else if (comp is GameObject) {
      if (((GameObject)comp).activeSelf) {
        ((GameObject)comp).SetActive(false);
        if (listForActivate) toEnable.Add(comp);
      }
    }
  }

  void Enable(Object comp, bool listForDeactivate = true) {
    print(comp.GetType());
    if (comp is Renderer) {
      if (!((Renderer)comp).enabled) {
        ((Renderer)comp).enabled = true;
        if (listForDeactivate) toDisable.Add(comp);
      }
    } else if (comp is Behaviour) {
      if (!((Behaviour)comp).enabled) {
        ((Behaviour)comp).enabled = true;
        if (listForDeactivate) toDisable.Add(comp);
      }
    } else if (comp is GameObject) {
      if (!((GameObject)comp).activeSelf) {
        ((GameObject)comp).SetActive(true);
        if (listForDeactivate) toDisable.Add(comp);
      }
    }
  }
}
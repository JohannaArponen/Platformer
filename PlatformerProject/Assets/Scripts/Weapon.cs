using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using MyBox;

public class Weapon : MonoBehaviour {

  [Tooltip("Optional. Does animation. Attack boolean value is passed as \"Attack\"")]
  public Animator animator;
  public KeyCode key = KeyCode.X;
  public float damage = 1;
  [Tooltip("The minimum value which " + nameof(angleCurve) + " outputs at 0 time")]
  public float minAngle = 45;
  [Tooltip("The maximum value which " + nameof(angleCurve) + " outputs at 1 time")]
  public float maxAngle = 135;
  private float angleDif { get => maxAngle - minAngle; }
  public AnimationCurve angleCurve = new AnimationCurve();
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

  /// <summary> this is 0 at the start of swing and 1 at the end of swing </summary> 
  public float time { get => _time; private set => _time = value; }
  private float _time;
  public Transform parent { get => transform.parent; }

  private float attackStart = 0;
  private bool attacking = false;
  private bool doAttack = false;


  /// <summary> The degrees per second that the sword swings at time </summary>
  public float GetSwingSpeed(float time = -1) => math.abs(angleDif * GetChangeAmount(time));
  /// <summary> Calculates the collision speed in units/sec at a specific distance from the pivot point </summary>
  public float GetHitSpeed(float distance, float time = -1) => Mathf.PI * (distance * distance) * (GetSwingSpeed(time) / 360);
  /// <summary> Calculates the collision speed in units/sec at a specific distance from the pivot point. The pos parameter is only used for retrieving the distance </summary>
  public float GetHitSpeed(Vector3 pos, float time = -1) => GetHitSpeed(Vector3.Distance(parent.transform.position, pos), time);
  /// <summary> Calculates the collision angle at the specified point </summary>
  public float GetHitAngle(Vector3 pos, float time = -1) => (pos - parent.transform.position).xy().Angle() + (GetChangeAmount(time) < 0 ? 90 : -90);
  /// <summary> Returns the current angle of parent </summary>
  public float GetAngle() => parent.transform.rotation.eulerAngles.z;
  /// <summary> Returns the current angle of parent and adds 90 degrees towards swing direction  </summary>
  public float GetDirectionAngle(float time = -1) => parent.transform.rotation.eulerAngles.z + (GetChangeAmount(time) < 0 ? 90 : -90);
  /// <summary> Returns the specified world space position relative to the pivot point </summary>
  public Vector3 GetPosRelativeToPivot(Vector3 pos) => pos - parent.transform.position;

  // On a linear curve from 0 to 1: Let duration be 1 and the value returned by this is always 1. Let duration be 2 and it's 0.5
  /// <summary> Returns the change in value if a second passes when moving along the angle at time of angleCurve </summary>
  float GetChangeAmount(float time = -1) => (angleCurve.Evaluate(Clamptime(time) + 0.01f) - angleCurve.Evaluate(Clamptime(time))) * (100 / duration);

  float Clamptime(float time) => time == -1 ? (this.time > 0.99f ? 0.99f : this.time) : (time > 0.99f ? 0.99f : time);

  // Start is called before the first frame update
  void Start() {
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
      var time = (Time.time - attackStart) / duration;
      if (animator != null) { animator.SetFloat("Attack", time); }
      if (time <= 1) {
        var curved = angleCurve.Evaluate((Time.time - attackStart) / duration);
        var angle = minAngle + (angleDif) * curved;
        parent.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      } else {
        attacking = false;
        foreach (var comp in toEnable)
          Enable(comp, false);
        foreach (var comp in toDisable)
          Disable(comp, false);
        parent.localRotation = Quaternion.AngleAxis(maxAngle, Vector3.forward);
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
        parent.localRotation = Quaternion.AngleAxis(minAngle, Vector3.forward);
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
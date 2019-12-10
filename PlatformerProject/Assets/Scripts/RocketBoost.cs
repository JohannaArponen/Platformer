using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Physics2DCharacter))]
[RequireComponent(typeof(Physics2DBouncyMove))]
public class RocketBoost : MonoBehaviour {

  public float channelTime = 1f;
  public float duration = 1.5f;
  public float startSpeed = 10;
  public float endSpeed = 100;
  public AnimationCurve speedCurve;
  [Tooltip("Weapon controls to use for boost. Defaults to the one returned by GetComponentInChildren")]
  public Weapon weapon;
  [Tooltip("Controller is used to determine direction of movement. Defaults to the one returned by GetComponentInChildren")]
  public Physics2DCharController ctrlr;
  [Tooltip("Optional particle system to use when boosting")]
  public ParticleSystem particles;
  // [Tooltip("Optional sound to play when starting boost")]
  // public AudioSource sound;
  [Tooltip("Enable to use Default Direction when player is not pressing any direction")]
  public bool useDefaultDirection = false;
  [Tooltip("Default direction to use when player is not pressing any direction")]
  public Vector2 defaultDirection = Vector2.right;
  [Tooltip("Disable attacking when holding button (after first attack)")]
  public bool disallowAttack = false;

  private float boostStartTime;
  private float channeledTime;
  private bool boosting = false;
  private Physics2DCharacter charPhysics;
  private Physics2DBouncyMove bouncy;

  // Start is called before the first frame update
  void Start() {
    charPhysics = GetComponent<Physics2DCharacter>();
    bouncy = GetComponent<Physics2DBouncyMove>();
    bouncy.enabled = false;
    if (weapon == null) weapon = GetComponentInChildren<Weapon>();
    if (weapon == null) throw new UnityException("No weapon found in children. A weapon is required either in the children or manually set in the inspector");
    if (ctrlr == null) ctrlr = GetComponentInChildren<Physics2DCharController>();
    if (ctrlr == null) throw new UnityException("No controller found in children. A controller is required either in the children or manually set in the inspector");
  }

  // Update is called once per frame
  void Update() {
    if (boosting) {
      if (Time.time >= boostStartTime + duration) {
        EndBoost();
        return;
      }
      var speed = startSpeed + speedCurve.Evaluate((Time.time - boostStartTime) / duration) * (endSpeed - startSpeed);
      bouncy.velocity = bouncy.velocity.SetLenSafer(speed);
      return;
    } else {
      if (Input.GetKey(weapon.key)) {
        if (channeledTime > 0) EnableAttack(false);
        channeledTime += Time.deltaTime;
      } else {
        EnableAttack(true);
        if (Input.GetKeyUp(weapon.key) && channeledTime >= channelTime)
          StartBoost();
        else
          channeledTime = 0;
      }
    }
  }

  void StartBoost() {
    channeledTime = 0;

    var dir = ctrlr.GetUserDirection();
    if (dir.Equals(Vector2.zero)) {
      if (useDefaultDirection) {
        dir = defaultDirection;
      } else {
        EnableAttack(true);
        return;
      }
    }

    boosting = true;
    boostStartTime = Time.time;
    bouncy.enabled = true;
    charPhysics.enabled = false;
    if (particles != null) particles.Play();
    bouncy.velocity = dir.SetLen(endSpeed);
    EnableAttack(false);
  }

  void EndBoost() {
    boosting = false;
    bouncy.enabled = false;
    charPhysics.enabled = true;
    if (particles != null) particles.Stop();
    charPhysics.velocity = bouncy.velocity;
    charPhysics.ignoreNextPhysicsUpdate = true; // Fix extra speed hitch
    charPhysics.stationary = false;
    EnableAttack(true);
  }

  void EnableAttack(bool enable = true) {
    if (disallowAttack) weapon.disallowAttack = !enable;
  }
}

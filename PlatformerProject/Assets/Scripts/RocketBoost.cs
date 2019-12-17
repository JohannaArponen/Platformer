using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using MyBox;

[RequireComponent(typeof(Physics2DCharacter))]
[RequireComponent(typeof(Physics2DReflective))]
public class RocketBoost : MonoBehaviour {

  [Tooltip("Use weapon swing duration as channel time")]
  public bool useSwingDuration = true;
  [ConditionalField(nameof(useSwingDuration), true)]
  public float channelTime = 1f;
  public float duration = 1.5f;
  public float startSpeed = 10;
  public float endSpeed = 100;
  public AnimationCurve speedCurve;
  [Tooltip("Allow ending boost by pressing the hotkey again")]
  public bool allowPrematureEnd = false;
  [Tooltip("Allow attacking when holding button (after first attack)")]
  public bool allowAttack = false;
  [Tooltip("Allow the player to input a direction for this long after releasing the hotkey")]
  public float waitDuration = 0.1f;
  [Tooltip("Use previously inputted directions if none is inputted when releasing the hotkey. Each axis is invidually recorded for this duration")]
  public float saveDirDuration = 0.1f;
  [Tooltip("Allow the player to input a new direction for this long after initiating a boost")]
  public float allowDirectionChangeDuration = 0.1f;
  [Tooltip("To prevent accidentally changing direction when releasing the directional keys, require the axis to be released for this long before possibly changing direction")]
  public float directionChangeRequireReleaseDuration = 0.1f;
  [Tooltip("Use smoothed axises instead of raw")]
  public bool dirChangeSmoothing = false;

  [Tooltip("Weapon controls to use for boost. Defaults to the one returned by GetComponentInChildren")]
  public Weapon weapon;
  [Tooltip("Controller is used to determine direction of movement. Defaults to the one returned by GetComponentInChildren")]
  public Physics2DCharController ctrlr;
  [Tooltip("Optional particle system to activate when boosting")]
  public ParticleSystem particles;

  [Tooltip("Optional. Does animation. Boost bool is passed as \"Boost\"")]
  public Animator animator;

  private float boostStartTime = float.NegativeInfinity;
  private float waitStartTime = float.NegativeInfinity;
  private float channeledTime;
  private bool boosting = false;
  private (float x, float y) releaseTime = (float.NegativeInfinity, float.NegativeInfinity);
  /// <summary> True if axis was 0 at the start of boost or while boosting </summary>
  private (bool x, bool y) releasedWhileBoost = (false, false);
  private (float x, float y) saveDirTime = (float.NegativeInfinity, float.NegativeInfinity);
  private Vector2 saveDir;
  private float2 prevVel;
  private bool hasHitSomething;

  private Physics2DCharacter charPhysics;
  private Physics2DReflective bouncy;

  // Start is called before the first frame update
  void Start() {
    charPhysics = GetComponent<Physics2DCharacter>();
    bouncy = GetComponent<Physics2DReflective>();
    bouncy.enabled = false;
    if (weapon == null) weapon = GetComponentInChildren<Weapon>();
    if (weapon == null) throw new UnityException("No weapon found in children. A weapon is required either in the children or manually set in the inspector");
    if (ctrlr == null) ctrlr = GetComponentInChildren<Physics2DCharController>();
    if (ctrlr == null) throw new UnityException("No controller found in children. A controller is required either in the children or manually set in the inspector");
  }

  // Update is called once per frame
  void Update() {
    SaveDirs();
    if (boosting) {
      if ((allowPrematureEnd && Input.GetKeyDown(weapon.key)) || Time.time >= boostStartTime + duration) {
        EndBoost();
        return;
      }
      if (bouncy.didHitSomething)
        hasHitSomething = true;
      // Allow changing direction for a duration
      if (!hasHitSomething && Time.time < boostStartTime + allowDirectionChangeDuration) {
        var dir = ctrlr.GetUserDirection(dirChangeSmoothing);
        // Require release duration but recognize new key presses immediately
        if (!releasedWhileBoost.x && dir.x == 0 && Time.time < releaseTime.x + directionChangeRequireReleaseDuration) dir.x = saveDir.x;
        if (!releasedWhileBoost.y && dir.y == 0 && Time.time < releaseTime.y + directionChangeRequireReleaseDuration) dir.y = saveDir.y;
        if (!dir.Equals(Vector2.zero)) {
          bouncy.velocity = bouncy.velocity.SetDir(dir);
        }
      }
      var speed = startSpeed + speedCurve.Evaluate((Time.time - boostStartTime) / duration) * (endSpeed - startSpeed);
      if (bouncy.velocity.Equals(float2.zero)) {
        bouncy.velocity = prevVel.SetLenSafe(-speed);
        if (bouncy.velocity.y < 0) bouncy.velocity.y = 0;
        EndBoost();
        return;
      }
      bouncy.velocity = bouncy.velocity.SetLenSafe(speed);
      prevVel = bouncy.velocity;
      UpdateAnim();
      return;
    } else {
      if (Input.GetKey(weapon.key)) {
        if (channeledTime > 0) EnableAttack(false);
        channeledTime += Time.deltaTime;
      } else {
        EnableAttack(true);
        if (Time.time < waitStartTime + waitDuration || (Input.GetKeyUp(weapon.key) && channeledTime >= (useSwingDuration ? weapon.duration : channelTime)))
          StartBoost();
        else
          channeledTime = 0;
      }
    }
  }

  void UpdateAnim() {
    charPhysics.Flip(bouncy.velocity.x);
    var angle = bouncy.velocity.Angle();
    animator.gameObject.transform.rotation = Quaternion.Euler(0, 0, bouncy.velocity.x < 0 ? -angle : -angle);
  }

  void StartBoost() {
    channeledTime = 0;

    var dir = ctrlr.GetUserDirection();
    if (saveDirDuration > 0) {
      // Allow previous inputs to complement current frame input
      if (dir.x == 0 && Time.time < saveDirTime.x + saveDirDuration) dir.x = saveDir.x;
      if (dir.y == 0 && Time.time < saveDirTime.y + saveDirDuration) dir.y = saveDir.y;
    }
    if (dir.Equals(Vector2.zero)) {
      // Allow using previous inputs when zero was given
      if (Time.time < saveDirTime.x + saveDirDuration) dir.x = saveDir.x;
      if (Time.time < saveDirTime.y + saveDirDuration) dir.y = saveDir.y;
      if (dir.Equals(Vector2.zero)) {
        waitStartTime = Time.time;
        EnableAttack(true);
        return;
      }
    }

    hasHitSomething = false;
    boosting = true;
    boostStartTime = Time.time;
    bouncy.enabled = true;
    charPhysics.enabled = false;
    if (particles != null) particles.Play();
    bouncy.velocity = dir.SetLen(startSpeed);
    releasedWhileBoost.x = dir.x == 0;
    releasedWhileBoost.y = dir.y == 0;
    releaseTime.x = float.NegativeInfinity;
    releaseTime.x = float.NegativeInfinity;
    EnableAttack(false);
    animator.SetBool("Boost", true);
    UpdateAnim();
  }

  void EndBoost() {
    if (animator != null) {
      animator.gameObject.transform.localRotation = Quaternion.identity;
      animator.SetBool("Boost", false);
    }
    boosting = false;
    bouncy.enabled = false;
    charPhysics.enabled = true;
    if (particles != null) particles.Stop();
    charPhysics.velocity = bouncy.velocity;
    charPhysics.ignoreNextPhysicsUpdate = true; // Fix extra speed hitch
    charPhysics.stationary = false;
    EnableAttack(true);
  }

  void SaveDirs() {
    var dir = ctrlr.GetUserDirection();
    if (dir.x == 0) {
      if (boosting) {
        releasedWhileBoost.x = true;
        releaseTime.x = Time.time;
      }
    } else {
      saveDirTime.x = Time.time;
      saveDir.x = dir.x;
    }
    if (dir.y == 0) {
      if (boosting) {
        releasedWhileBoost.y = true;
        releaseTime.y = Time.time;
      }
    } else {
      saveDirTime.y = Time.time;
      saveDir.y = dir.y;
    }
  }

  void EnableAttack(bool enable = true) {
    if (!allowAttack) weapon.disallowAttack = !enable;
  }
}

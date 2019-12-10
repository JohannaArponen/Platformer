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
  [Tooltip("Use smoothed axises instead of raw")]
  public bool dirChangeSmoothing = false;

  [Tooltip("Weapon controls to use for boost. Defaults to the one returned by GetComponentInChildren")]
  public Weapon weapon;
  [Tooltip("Controller is used to determine direction of movement. Defaults to the one returned by GetComponentInChildren")]
  public Physics2DCharController ctrlr;
  [Tooltip("Optional particle system to use when boosting")]
  public ParticleSystem particles;

  private float boostStartTime;
  private float waitStartTime;
  private float channeledTime;
  private bool boosting = false;
  private bool waiting = false;
  private (float x, float y) saveDirTime = (0f, 0f);
  private Vector2 saveDir;
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
      if ((allowPrematureEnd && Input.GetKeyDown(weapon.key)) || Time.time >= boostStartTime + duration) {
        EndBoost();
        return;
      }
      // Allow changing direction for a duration
      if (Time.time < boostStartTime + allowDirectionChangeDuration) {
        print("asd");
        var dir = ctrlr.GetUserDirection(true);
        if (!dir.Equals(Vector2.zero))
          bouncy.velocity = bouncy.velocity.SetDir(dir);
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
        if (Time.time < waitStartTime + waitDuration || (Input.GetKeyUp(weapon.key) && channeledTime >= channelTime))
          StartBoost();
        else
          channeledTime = 0;
      }
    }
    SaveDir();
  }

  void StartBoost() {
    channeledTime = 0;

    var dir = ctrlr.GetUserDirection();
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

    boosting = true;
    boostStartTime = Time.time;
    bouncy.enabled = true;
    charPhysics.enabled = false;
    if (particles != null) particles.Play();
    bouncy.velocity = dir.SetLen(startSpeed);
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

  void SaveDir() {
    var dir = ctrlr.GetUserDirection();
    if (dir.x != 0) {
      saveDirTime.x = Time.time;
      saveDir.x = dir.x;
    }
    if (dir.y != 0) {
      saveDirTime.y = Time.time;
      saveDir.y = dir.y;
    }
  }

  void EnableAttack(bool enable = true) {
    if (!allowAttack) weapon.disallowAttack = !enable;
  }
}

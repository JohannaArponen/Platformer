using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Physics2DCharacter))]
public class Physics2DCharController : MonoBehaviour {
  [Tooltip("Units per second")]
  public float speed;
  [Tooltip("Allow increasing movement speed only to the value of Speed (E.G. if velocity would already give more speed)")]
  public bool limitSpeedIncrease = true;
  [Tooltip("Take input from input horizontal axis. Left and right keys still work with this enabled")]
  public bool useHorizontalAxisMove = true;
  [Tooltip("Use smoothed horizontal axis")]
  public bool axisMoveSmooth = true;
  public KeyCode leftKey;
  public KeyCode rightKey;
  [Tooltip("Allows movement with vertical keys")]
  public bool moveVertically = false;
  [Tooltip("When other scripts request the user direction from this class use vertical keys instead of jump or crouch")]
  public bool useVerticalKeysForUserDirection;
  [Tooltip("Use vertical axis for vertical hotkeys. Up and down keys still work with this enabled")]
  public bool useVerticalAxisMovement = false;
  public KeyCode upKey;
  public KeyCode downKey;
  [Tooltip("Use vertical axis (when positive) for jump. Jump key still works with this enabled")]
  public bool useVerticalAxisJump = false;
  public KeyCode jumpKey = KeyCode.Space;
  public float jumpStrength;
  [Tooltip("Use vertical axis (when negative) for crouch. Crouch key still works with this enabled")]
  public bool useVerticalAxisCrouch = false;
  public KeyCode crouchKey = KeyCode.C;

  private Physics2DCharacter physics;

  void Start() {
    physics = GetComponent<Physics2DCharacter>();
  }

  // Update is called once per frame
  void Update() {
    // Movement
    float move = 0;
    if (useHorizontalAxisMove) {
      move = speed * (axisMoveSmooth ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
    } else {
      if (Input.GetKey(rightKey)) {
        move = Input.GetKey(leftKey) ? 0 : speed;
      } else if (Input.GetKey(leftKey)) {
        move = Input.GetKey(rightKey) ? 0 : -speed;
      }
    }
    if (move > 0 && physics.onSlopeRight) {
      move = 0;
    } else if (move < 0 && physics.onSlopeLeft) {
      move = 0;
    }

    if (limitSpeedIncrease) {
      if (move < 0 == physics.velocity.x < 0) {
        var abs = math.abs(physics.velocity.x);
        if (abs > speed)
          move = 0;
        else
          move *= (speed - abs) / speed;
      }
    }
    physics.staticVelocity.x += move;

    // Jump
    if (((physics.onGround && !physics.onSlope) || physics.stationary) && !physics.onCeiling) {
      if ((useVerticalAxisJump && Input.GetAxisRaw("Vertical") > 0) || Input.GetKey(jumpKey)) {
        physics.velocity.y = jumpStrength;
      }
    }

    // Crouch
    if ((useVerticalAxisJump && Input.GetAxisRaw("Vertical") < 0) || Input.GetKey(crouchKey)) {
      print("crouch");
    }
  }

  /// <summary> Returns the unnormalized direction the player is inputting. Both values range from -1 and 1. Smoothing value is only used if that input type is enabled </summary>
  public Vector2 GetUserDirection(bool smoothing = false) {
    var dir = Vector2.zero;

    if (useVerticalKeysForUserDirection) {
      if (useVerticalAxisCrouch) {
        var axis = (smoothing ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical"));
        dir.y += axis;
      }
      if (Input.GetKey(upKey))
        dir.y += 1;
      if (Input.GetKey(downKey))
        dir.y -= 1;

    } else {
      if (useVerticalAxisCrouch) {
        var axis = (smoothing ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical"));
        if (axis < 0)
          dir.y += axis; // Adds negative value
      }
      if (Input.GetKey(crouchKey))
        dir.y -= 1;

      if (useVerticalAxisJump) {
        var axis = (smoothing ? Input.GetAxis("Vertical") : Input.GetAxisRaw("Vertical"));
        if (axis > 0)
          dir.y += axis; // Adds positive value
      }
      if (Input.GetKey(jumpKey))
        dir.y += 1;

      if (useHorizontalAxisMove) {
        dir.x = (smoothing ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal"));
      }
    }

    if (Input.GetKey(leftKey))
      dir.x -= 1;
    if (Input.GetKey(rightKey))
      dir.x += 1;

    dir = math.clamp(dir, -1, 1);

    return dir;
  }
}
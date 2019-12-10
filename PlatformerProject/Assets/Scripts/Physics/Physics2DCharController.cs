using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Physics2DCharacter))]
public class Physics2DCharController : MonoBehaviour {
  public float speed;
  [Tooltip("Allow increasing movement speed only to the value of Speed (E.G. if velocity would already give more speed)")]
  public bool limitSpeedIncrease = true;
  public bool useHorizontalAxisMove = true;
  public bool axisMoveSmooth = true;
  public KeyCode leftKey;
  public KeyCode rightKey;
  public bool useVerticalAxisJump = true;
  public KeyCode jumpKey;
  public float jumpStrength;
  public bool useVerticalAxisCrouch = true;
  public KeyCode crouchKey;

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
    if ((physics.onGround && !physics.onSlope) || physics.stationary) {
      if ((useVerticalAxisJump && Input.GetAxisRaw("Vertical") > 0) || Input.GetKey(jumpKey)) {
        physics.velocity.y = jumpStrength;
      }
    }

    // Crouch
    if ((useVerticalAxisJump && Input.GetAxisRaw("Vertical") < 0) || Input.GetKey(crouchKey)) {
      print("crouch");
    }
  }

  /// <summary> Returns the direction the player is inputting. Both values range from -1 and 1 </summary>
  public Vector2 GetUserDirection() {
    var dir = Vector2.zero;
    if (useVerticalAxisCrouch ? Input.GetAxisRaw("Vertical") < 0 : Input.GetKey(crouchKey))
      dir.y -= 1;
    if (useVerticalAxisJump ? Input.GetAxisRaw("Vertical") > 0 : Input.GetKey(jumpKey))
      dir.y += 1;

    if (useHorizontalAxisMove) {
      var axis = Input.GetAxisRaw("Horizontal");
      dir.x = axis == 0 ? 0 : Mathf.Sign(axis);
    } else {
      if (Input.GetKey(leftKey))
        dir.x -= 1;
      if (Input.GetKey(rightKey))
        dir.x += 1;
    }
    return dir;
  }
}
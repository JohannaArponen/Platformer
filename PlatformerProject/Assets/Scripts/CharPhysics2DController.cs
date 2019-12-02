﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharPhysics2D))]
public class CharPhysics2DController : MonoBehaviour {
  public float speed;
  public bool useHorizontalAxisMove = true;
  public bool axisMoveSmooth = true;
  public KeyCode leftKey;
  public KeyCode rightKey;
  public bool useVerticalAxisJump = true;
  public KeyCode jumpKey;
  public float jumpStrength;
  public bool useVerticalAxisCrouch = true;
  public KeyCode crouchKey;

  private CharPhysics2D physics;

  void Start() {
    physics = GetComponent<CharPhysics2D>();
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
}
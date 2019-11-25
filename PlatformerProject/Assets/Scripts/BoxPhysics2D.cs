﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BoxPhysics2D : MonoBehaviour {

  [Tooltip("Default gravity")]
  public float defaultGravity = 1000f;
  [Tooltip("Drag")]
  public float drag = 0.05f;
  [Tooltip("Maximum walkable slope. Slopes steeper than this cause sliding")]
  public float maxSlopeAngle = 45;
  [Tooltip("Maximum height of an obstacle that is walked over")]
  public float maxHeightStep = 0.1f;
  [Tooltip("Maximum amount of raycast done to check for walkable obstacles (height). Each iteration the value is halved")]
  public int maxHeightVerticalSteps = 1;
  [Tooltip("Maximum amount of raycast done to check for walkable obstacles (distance). Each iteration the value is halved")]
  public int maxHeightHorizontalSteps = 1;
  [Tooltip("Layers which are checked by the raycasts")]
  public LayerMask layers;
  [Tooltip("Maximum physics box raycasts. When a raycast collides, a new raycast is done along its vector")]
  public int maxPhysicsIters = 3;
  [Tooltip("Avoid getting stuck inside colliders by offsetting collision positions")]
  public float contactOffset = 0.003771f;
  [Tooltip("Length of raycast when testing collision (ground, ceiling, left and right checks)")]
  public float collisionTestLength = 0.01f;
  [Tooltip("Smaller values increase the amount of sliding on steep angles")]
  public float steepVelocityProjectPower = 2;

  [Tooltip("Current gravity")]
  public float gravity = 10f;
  [Tooltip("Simulated velocity")]
  public float2 velocity;
  [Tooltip("Velocity which is not affected by gravity or drag. Resets every update")]
  public float2 staticVelocity;
  [Tooltip("Velocity which is not affected by gravity or drag")]
  public float2 debugVelocity;

  [HideInInspector] public RaycastHit2D onGround;
  [HideInInspector] public RaycastHit2D onCeiling;
  [HideInInspector] public RaycastHit2D onRight;
  [HideInInspector] public RaycastHit2D onLeft;
  [HideInInspector] public bool onSlopeRight;
  [HideInInspector] public bool onSlopeLeft;
  [HideInInspector] public bool onSlope;
  [HideInInspector] public bool stationary;
  [HideInInspector] public float slopeAngle;

  BoxCollider2D col;
  float2 size;



  void Start() {
    col = GetComponent<BoxCollider2D>();
    gravity = defaultGravity;
    size = col.bounds.size.xy();
  }

  void LateUpdate() {
    var prevPos = transform.position;
    if (Input.GetKeyDown(KeyCode.R)) transform.position = Vector3.zero;
    Physics();
    onGround = Physics2D.BoxCast(transform.position, size, 0, Vector2.down, collisionTestLength, layers);
    onCeiling = Physics2D.BoxCast(transform.position, size, 0, Vector2.up, collisionTestLength, layers);
    onRight = Physics2D.BoxCast(transform.position, size, 0, Vector2.right, collisionTestLength, layers);
    onLeft = Physics2D.BoxCast(transform.position, size, 0, Vector2.left, collisionTestLength, layers);

    stationary = prevPos == transform.position && onGround;

    if (onGround) {
      slopeAngle = Vector2.Angle(Vector2.up, onGround.normal);
      onSlopeRight = !onSlopeLeft && slopeAngle > maxSlopeAngle && onGround.normal.x < 0;
      onSlopeLeft = slopeAngle > maxSlopeAngle && onGround.normal.x >= 0;
      onSlope = onSlopeLeft || onSlopeRight;
    } else {
      slopeAngle = 0;
      onSlopeRight = false;
      onSlopeLeft = false;
      onSlope = false;
    }
    staticVelocity = float2.zero;
  }

  void Physics() {
    float multiplier = math.max(0f, 1f - drag * Time.deltaTime);
    velocity *= multiplier;
    velocity.y -= gravity * Time.deltaTime;


    if (Physics2D.BoxCast(transform.position, size, 0, float2.zero, 0, layers))
      transform.position += new Vector3(0, 0.1f, 0);

    var endVel = (staticVelocity + velocity + debugVelocity) * Time.deltaTime;

    var targetPos = transform.position.Add2XY(endVel);
    for (int i = 0; i < maxPhysicsIters; i++) {
      // endVel = (targetPos - transform.position).xy();
      var contact = Physics2D.BoxCast(transform.position, size, 0, math.normalizesafe(endVel, float2.zero), math.length(endVel), layers);
      if (contact) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, contact.normal);
        bool isSteep = contactAngle > maxSlopeAngle;
        if (onGround && isSteep && Vector2.Angle(Vector2.up, endVel) < 135) {
          for (int j = 0; j < maxHeightVerticalSteps; j++) {
            var currentStep = maxHeightStep / (j + 1);
            var pos = transform.position.xy().Add2Y(currentStep);

            bool breakOuter = false;
            RaycastHit2D verContact;
            float verStep = 0;
            for (int k = 0; k < maxHeightHorizontalSteps; k++) {
              verStep = endVel.x / (k + 1);
              verContact = Physics2D.BoxCast(pos, size, 0, new float2(endVel.x >= 0 ? 1 : -1, 0), math.abs(verStep), layers);
              if (!verContact)
                break;
              else if (k == maxHeightHorizontalSteps - 1) {
                breakOuter = true;
                break;
              }
            }
            if (breakOuter) break;

            var downContact = Physics2D.BoxCast(pos.Add2X(verStep), size, 0, new Vector2(0, -currentStep), currentStep, layers);
            if (downContact && Vector2.Angle(Vector2.up, downContact.normal) < maxSlopeAngle) {
              var newPos2 = downContact ? downContact.centroid.Add2Y(contactOffset) : pos + new Vector2(verStep, -currentStep);
              if (!Physics2D.BoxCast(newPos2, size, 0, Vector2.zero, 0, layers))
                transform.position = newPos2;
              return;
            }
          }
        }

        // Handle slopes affecting velocity and stuff
        if (!isSteep)
          endVel.y = 0;
        var newPos = new Vector3();
        if (isSteep && contactAngle < 90)
          velocity *= math.pow(math.normalizesafe(contact.normal, float2.zero) * -1 + 1, steepVelocityProjectPower);
        else
          velocity *= math.abs(math.normalizesafe(contact.normal, float2.zero)) * -1 + 1;
        var oldVel = endVel;
        endVel = Vector3.Project((Vector2)endVel, Quaternion.Euler(0, 0, 90) * contact.normal).xy();
        // targetPos = transform.position.Add2XY(endVel);

        if (math.length(oldVel) > 0.01f) { // Prevent jitter caused by contact offset. Maybe useless
          if (contact.point.x > transform.position.x)
            newPos.x = math.max(transform.position.x, contact.point.x - size.y / 2 - contactOffset);
          else
            newPos.x = math.min(transform.position.x, contact.point.x + size.x / 2 + contactOffset);

          if (contact.point.y > transform.position.y)
            newPos.y = math.max(transform.position.y, contact.point.y - size.x / 2 - contactOffset);
          else
            newPos.y = math.min(transform.position.y, contact.point.y + size.y / 2 + contactOffset);
          if (!Physics2D.BoxCast(newPos, size, 0, float2.zero, 0, layers) || Input.GetKeyDown(KeyCode.P)) {
            transform.position = newPos;
          }
        }
      } else {

        // Free move
        if (!Physics2D.BoxCast(transform.position.Add2XY(endVel), size, 0, Vector2.zero, 0, layers))
          transform.position += new Vector3(endVel.x, endVel.y);

        // Down slopes
        if (onGround) {
          var downContact = Physics2D.BoxCast(transform.position, size, 0, Vector2.down, math.abs(endVel.x) * math.tan(maxSlopeAngle * Mathf.Deg2Rad) + maxHeightStep, layers);
          if (downContact && Vector2.Angle(Vector2.up, downContact.normal) <= maxSlopeAngle) {
            // Debug.DrawRay(downContact.point, downContact.normal, Color.green, 1);
            var newPos = downContact.centroid;
            newPos.y += contactOffset;
            if (!Physics2D.BoxCast(newPos, size, 0, float2.zero, 0, layers))
              transform.position = newPos;
          }
        }
        break;
      }
      if (endVel.Equals(float2.zero)) {
        break;
      }
    }
  }
}
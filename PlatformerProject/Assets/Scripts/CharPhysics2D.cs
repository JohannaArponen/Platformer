﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CharPhysics2D : MonoBehaviour {

  [Tooltip("Default gravity")]
  public float defaultGravity = 1000f;
  [Tooltip("Drag")]
  public float drag = 0.05f;
  [Tooltip("Maximum walkable slope. Slopes steeper than this cause sliding")]
  public float maxSlopeAngle = 45;
  [Tooltip("Maximum height of an obstacle that is stepped over")]
  public float maxHeightStep = 0.1f;
  [Tooltip("Allow moving stepping over obstacles in air")]
  public bool airHeightStep = false;
  [Tooltip("Move with the collider which is stood on")]
  public bool moveWithGround = true;
  [Tooltip("Maximum amount of raycast done to check for walkable obstacles (height). Each iteration the value is halved")]
  public int maxHeightVerticalSteps = 1;
  [Tooltip("Maximum amount of raycast done to check for walkable obstacles (distance). Each iteration the value is halved")]
  public int maxHeightHorizontalSteps = 1;
  [Tooltip("Layers which are checked by the raycasts")]
  public ContactFilter2D layers;
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
  [Tooltip("Velocity which is not affected by deltaTime, gravity or drag")]
  public float2 pureVelocity;
  [Tooltip("Velocity which is not affected by gravity or drag")]
  public float2 debugVelocity;

  [HideInInspector] public RaycastHit2D onGround, onCeiling, onRight, onLeft;
  [HideInInspector] public bool onSlopeRight, onSlopeLeft, onSlope;
  [HideInInspector] public bool stationary;
  [HideInInspector] public float slopeAngle;

  private bool validPrevcollider = false;
  private TransformData colPrevTransform;

  private Collider2D col;
  private Rigidbody2D rb;
  private Vector2 size;



  void Start() {
    col = GetComponent<BoxCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    gravity = defaultGravity;
    size = col.bounds.size.xy();
  }

  private RaycastHit2D GetFirstHit(Vector2 start, Vector2 dir, float distance = float.PositiveInfinity) {
    var prevPos = transform.position;
    transform.position = start;
    Physics2D.SyncTransforms();
    var results = new RaycastHit2D[1];
    var res = rb.Cast(dir, layers, results, distance);
    transform.position = prevPos;
    Physics2D.SyncTransforms();
    return res > 0 ? results[0] : new RaycastHit2D();
  }

  void LateUpdate() {
    if (moveWithGround && validPrevcollider && onGround) MoveWithGround();
    var postMovePos = transform.position;
    if (Input.GetKeyDown(KeyCode.R)) transform.position = Vector3.zero;

    Physics();

    onGround = GetFirstHit(transform.position, Vector2.down, collisionTestLength);
    onCeiling = GetFirstHit(transform.position, Vector2.up, collisionTestLength);
    onRight = GetFirstHit(transform.position, Vector2.right, collisionTestLength);
    onLeft = GetFirstHit(transform.position, Vector2.left, collisionTestLength);

    stationary = postMovePos == transform.position;

    if (onGround) {
      if (moveWithGround) {
        colPrevTransform = onGround.collider.gameObject.transform.Save();
        validPrevcollider = true;
      }

      slopeAngle = Vector2.Angle(Vector2.up, onGround.normal);
      onSlopeRight = !onSlopeLeft && slopeAngle > maxSlopeAngle && onGround.normal.x < 0;
      onSlopeLeft = slopeAngle > maxSlopeAngle && onGround.normal.x >= 0;
      onSlope = onSlopeLeft || onSlopeRight;
    } else {
      validPrevcollider = false;
      slopeAngle = 0;
      onSlopeRight = false;
      onSlopeLeft = false;
      onSlope = false;
    }
    staticVelocity = Vector2.zero;
  }

  void MoveWithGround() {
    var colGo = onGround.collider.gameObject;
    if (colGo.isStatic) return; // Moving or deactivating static things breaks things
    colGo.SetActive(false);
    var dir = (colGo.transform.position - colPrevTransform.position).xy();
    var hit = GetFirstHit(transform.position, math.normalizesafe(dir), dir.magnitude);
    if (!hit) {
      transform.position += colGo.transform.position - colPrevTransform.position;
      // !!! Rotation and scaling is not accounted for
      // transform.rotation = colTransform.rotation * Quaternion.Inverse(colNewTransform.rotation) * transform.rotation;
      // transform.localScale += colTransform.localScale - colNewTransform.localScale;
    } else { // GetFirstHit(newPos, Vector2.one.xo(), 0)
      var newPos = GetCollisionPosition(transform.position, hit).xyo();
      if (!GetFirstHit(newPos, Vector2.one.xo(), 0))
        transform.position = newPos;
    }

    colGo.SetActive(true);
  }

  public Vector2 GetCollisionPosition(Vector2 pos, RaycastHit2D contact) {
    var newPos = new Vector2();

    if (contact.point.x > pos.x) newPos.x = math.max(pos.x, contact.point.x - size.y / 2 - contactOffset);
    else newPos.x = math.min(pos.x, contact.point.x + size.x / 2 + contactOffset);

    if (contact.point.y > pos.y) newPos.y = math.max(pos.y, contact.point.y - size.x / 2 - contactOffset);
    else newPos.y = math.min(pos.y, contact.point.y + size.y / 2 + contactOffset);

    return newPos;
  }

  void Physics() {
    float multiplier = math.max(0f, 1f - drag * Time.deltaTime);
    velocity *= multiplier;
    velocity.y -= gravity * Time.deltaTime;

    // Check if inside a collider, if so move up
    if (GetFirstHit(transform.position, Vector2.one.xo(), 0))
      transform.position += new Vector3(0, 0.1f, 0);

    var endVel = (staticVelocity + velocity + debugVelocity) * Time.deltaTime + pureVelocity;

    for (int i = 0; i < maxPhysicsIters; i++) {
      var contact = GetFirstHit(transform.position, math.normalizesafe(endVel), math.length(endVel));
      if (contact) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, contact.normal);
        bool isSteep = contactAngle > maxSlopeAngle;
        if ((onGround || airHeightStep) && isSteep && Vector2.Angle(Vector2.up, endVel) < 135) {
          for (int j = 0; j < maxHeightVerticalSteps; j++) {
            var currentStep = maxHeightStep / (j + 1);
            var pos = transform.position.xy().AddY(currentStep);

            bool breakOuter = false;
            RaycastHit2D verContact;
            float verStep = 0;
            for (int k = 0; k < maxHeightHorizontalSteps; k++) {
              verStep = endVel.x / (k + 1);
              verContact = GetFirstHit(pos, new float2(endVel.x >= 0 ? 1 : -1, 0), math.abs(verStep));
              if (!verContact)
                break;
              else if (k == maxHeightHorizontalSteps - 1) {
                breakOuter = true;
                break;
              }
            }
            if (breakOuter) break;

            var downContact = GetFirstHit(pos.AddX(verStep), new Vector2(0, -currentStep), currentStep);
            if (downContact && Vector2.Angle(Vector2.up, downContact.normal) < maxSlopeAngle) {
              var newPos2 = downContact ? downContact.centroid.AddY(contactOffset) : pos + new Vector2(verStep, -currentStep);
              if (!GetFirstHit(newPos2, Vector2.zero, 0))
                transform.position = newPos2;
              return;
            }
          }
        }

        // Handle slopes affecting velocity and stuff
        if (!isSteep)
          endVel.y = 0;
        if (isSteep && contactAngle < 90)
          velocity *= math.pow((float2)contact.normal * -1 + 1, steepVelocityProjectPower);
        else if (!onGround && velocity.y < 0 && contactAngle > 90)
          velocity.x *= math.abs(contact.normal.x) * -1 + 1;
        else
          velocity *= math.abs(contact.normal) * -1 + 1;

        if (math.length(endVel) > 0.01f) { // Prevent jitter caused by contact offset. Maybe useless
          var newPos = GetCollisionPosition(transform.position, contact);

          if (!GetFirstHit(newPos, Vector2.one.xo(), 0) || Input.GetKeyDown(KeyCode.P))
            transform.position = newPos;
        }
        // Project velocity along normal of collision
        // !!! causes jitter when trying to go up a too steep slope (somehow determine to not do it when going towards a too steep slope)
        endVel = Vector3.Project((Vector2)endVel, Quaternion.Euler(0, 0, 90) * contact.normal).xy();

      } else {

        // Free move
        if (!GetFirstHit(transform.position.AddXY(endVel), Vector2.zero, 0))
          transform.position += new Vector3(endVel.x, endVel.y);

        // Down slopes
        if (onGround) {
          var downContact = GetFirstHit(transform.position, Vector2.down, math.abs(endVel.x) * math.tan(maxSlopeAngle * Mathf.Deg2Rad) + maxHeightStep);

          if (downContact && (math.abs(downContact.normal.x) < 0.00001f || (endVel.x < 0 == downContact.normal.x < 0)) && Vector2.Angle(Vector2.up, downContact.normal) <= maxSlopeAngle) {
            Debug.DrawRay(downContact.point, downContact.normal, Color.green);
            var newPos = downContact.centroid;
            newPos.y += contactOffset;
            if (!GetFirstHit(newPos, Vector2.one.xo(), 0))
              transform.position = newPos;
            else {
              newPos.x += contactOffset * math.sign(endVel.x) * 2;
              if (!GetFirstHit(newPos, Vector2.one.xo(), 0))
                transform.position = newPos;
            }
          }
        }
        break;
      }
      if (endVel.Equals(Vector2.one)) {
        break;
      }
    }
  }
}
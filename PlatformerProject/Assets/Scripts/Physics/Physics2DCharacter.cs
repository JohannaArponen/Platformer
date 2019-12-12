using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2DCharacter : MonoBehaviour {

  [Tooltip("Optional. Does animation. Speed value is passed as \"Speed\"")]
  public Animator animator;
  [ConditionalField("animator")]
  [Tooltip("Speed of move animation is multiplied by this")]
  public float animationSpeedMultiplier = 1;
  [Tooltip("Default gravity")]
  public float defaultGravity = 1000f;
  [Tooltip("Drag")]
  public float drag = 0.05f;
  [Range(0, 90f)]
  [Tooltip("Maximum walkable slope. Slopes steeper than this cause sliding")]
  public float maxAngle = 45;
  [Tooltip("Maximum height of an obstacle that is stepped over")]
  public float maxHeightStep = 0.1f;
  [Tooltip("Allow moving stepping over obstacles in air")]
  public bool airHeightStep = false;
  [Tooltip("Move with the collider which is stood on")]
  public bool moveWithGround = true;
  [Tooltip("Ignores collides belonging to children by disabling and enabling them when collision checks are done")]
  public bool ignoreChildColliders = true;
  [Tooltip("Maximum amount of iterations done to check for walkable obstacles (horizontal, vertical). Each iteration the value is halved")]
  public float2 maxHeightSteps = 1;
  [Tooltip("Layers which are checked by the raycasts")]
  public ContactFilter2D layers;
  [Tooltip("Maximum physics box raycasts. When a raycast collides, a new raycast is done along its vector")]
  public int maxPhysicsIters = 3;
  [Tooltip("Avoid getting stuck inside colliders by offsetting collision positions")]
  public float contactOffset = 0.003771f;
  [Tooltip("Length of raycast when testing collision (ground, ceiling, left and right checks)")]
  public float dirCollisionTestLength = 0.01f;
  [Tooltip("Smaller values increase the amount of sliding on steep angles")]
  public float steepVelocityProjectPower = 2;
  [Range(0, 45f)]
  [Tooltip("Sometimes go gets stuck on things because the physics engine gives us a wrong normal value. If we failed to move and the normal angle is at most this far away from any axis, move along the normal by the value below")]
  public float nearAxisAngleOffsetRange = 2;
  [Tooltip("Sometimes go gets stuck on things because the physics engine gives us a wrong normal value. If we failed to move and the normal angle is at most this")]
  public float nearAxisAngleOffset = 0.1f;

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


  /// <summary> Block the next physics update done by this component </summary>
  [HideInInspector] public bool ignoreNextPhysicsUpdate = false;

  [HideInInspector] public RaycastHit2D onGround, onCeiling, onRight, onLeft;
  [HideInInspector] public bool onSlopeRight, onSlopeLeft, onSlope;
  [HideInInspector] public bool stationary;
  [HideInInspector] public float slopeAngle;

  public DebugOptions debug;

  private Vector3 finalPos;
  private bool validPrevcollider = false;
  private TransformData colPrevTransform;

  private Physics2DCastUtil cast;
  private Collider2D col;
  private Rigidbody2D rb;


  private void Start() {
    col = GetComponent<BoxCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    gravity = defaultGravity;
  }



  void LateUpdate() {
    if (cast == null) cast = new Physics2DCastUtil(transform, rb, layers);
    List<Transform> disabledChildren = new List<Transform>();
    if (ignoreChildColliders) {
      foreach (Transform child in transform) {
        if (child.GetComponentInChildren<Collider2D>() != null) {
          if (child.gameObject.activeSelf) {
            disabledChildren.Add(child);
            child.gameObject.SetActive(false);
          }
        }
      }
    }

    var movedWithGround = false;
    if (moveWithGround && validPrevcollider && onGround) movedWithGround = MoveWithGround();
    var postMovePos = transform.position;
    if (Input.GetKeyDown(KeyCode.R)) transform.position = Vector3.zero;

    if (debug.noMove) {
      var prevPos = transform.position;
      Physics();
      transform.position = prevPos;
    } else Physics();


    ignoreNextPhysicsUpdate = false;

    stationary = movedWithGround ? (postMovePos == transform.position) : (finalPos == transform.position);

    onGround = cast.Cast(transform.position, Vector2.down * dirCollisionTestLength);
    onCeiling = cast.Cast(transform.position, Vector2.up * dirCollisionTestLength);
    onRight = cast.Cast(transform.position, Vector2.right * dirCollisionTestLength);
    onLeft = cast.Cast(transform.position, Vector2.left * dirCollisionTestLength);

    if (onGround) {
      slopeAngle = Vector2.Angle(Vector2.up, onGround.normal);
      onSlopeRight = !onSlopeLeft && slopeAngle > maxAngle && onGround.normal.x < 0;
      onSlopeLeft = slopeAngle > maxAngle && onGround.normal.x >= 0;
      onSlope = onSlopeLeft || onSlopeRight;

      if (!onSlope) {
        velocity = 0;
        if (moveWithGround) {
          colPrevTransform = onGround.collider.gameObject.transform.Save();
          validPrevcollider = true;
        }
      }
    } else {
      validPrevcollider = false;
      slopeAngle = 0;
      onSlopeRight = false;
      onSlopeLeft = false;
      onSlope = false;
    }

    if (ignoreChildColliders)
      foreach (var child in disabledChildren)
        child.gameObject.SetActive(true);

    if (animator != null)
      animator.SetFloat("Speed", !onGround || onSlope ? 0 : math.abs((finalPos.x - transform.position.x) * animationSpeedMultiplier / Time.deltaTime));
    finalPos = transform.position;
    staticVelocity = Vector2.zero;
  }


  bool MoveWithGround() {
    var colGo = onGround.collider.gameObject;
    if (colGo.isStatic) return false; // Moving or deactivating static things breaks things
    colGo.SetActive(false);
    var dir = (colGo.transform.position - colPrevTransform.position).xy();
    var hit = cast.Cast(transform.position, dir);
    if (!hit) {
      transform.position += colGo.transform.position - colPrevTransform.position;
      // !!! Rotation and scaling is not accounted for
      // transform.rotation = colTransform.rotation * Quaternion.Inverse(colNewTransform.rotation) * transform.rotation;
      // transform.localScale += colTransform.localScale - colNewTransform.localScale;
    } else {
      var newPos = CollisionPos(hit, transform.position, dir).xyo();
      if (!cast.Collides(newPos))
        transform.position = newPos;
    }

    colGo.SetActive(true);
    return true;
  }

  public Vector2 CollisionPos(RaycastHit2D hit, Vector2 origin, Vector2 dir) {
    if (hit.distance <= contactOffset) return origin;
    return origin + dir.SetLen(hit.distance - contactOffset);
  }

  void Physics() {
    if (ignoreNextPhysicsUpdate) return;
    rb.useFullKinematicContacts = true;
    float multiplier = math.max(0f, 1f - drag * Time.deltaTime);
    velocity *= multiplier;
    velocity.y -= gravity * Time.deltaTime;

    if (cast.Collides(transform.position)) {
      Debug.LogWarning("Physics2D Rigidbody was inside a collider");
      transform.position += new Vector3(0, 0.1f, 0);
    }

    var endVel = (staticVelocity + velocity + debugVelocity) * Time.deltaTime + pureVelocity;

    var forceSteps = false;
    for (int i = 0; i < maxPhysicsIters; i++) {
      var hit = cast.Cast(transform.position, endVel);
      if (hit && debug.hitNormal)
        Debug.DrawRay(hit.point, hit.normal, debug.hitNormal.color, 0);
      if (hit) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, hit.normal);
        bool isSteep = contactAngle > maxAngle;
        if (forceSteps || ((onGround || airHeightStep) && isSteep) && Vector2.Angle(Vector2.up, endVel) < 135) {
          forceSteps = false;
          var heighStep = maxHeightStep * 2;
          for (int j = 0; j < maxHeightSteps.y; j++) {
            heighStep /= 2;
            var pos = transform.position.xy().AddY(heighStep);

            bool breakOuter = false;
            RaycastHit2D horHit;
            float horStep = endVel.x * 2;
            for (int k = 0; k < maxHeightSteps.x; k++) {
              horStep /= 2;
              horHit = cast.Cast(pos, new Vector2(horStep, 0));
              if (!horHit)
                break;
              else if (k == maxHeightSteps.x - 1) {
                breakOuter = true;
                break;
              }
            }
            if (breakOuter) break;
            var dir = new Vector2(0, -heighStep);
            var downPos = pos.AddX(horStep);
            var downHit = cast.Cast(downPos, dir);
            if (downHit && Vector2.Angle(Vector2.up, downHit.normal) < maxAngle) {
              var collisionPos = CollisionPos(downHit, downPos, dir);
              cast.TryMoveTo(collisionPos);
              return;
            }
          }
        }

        var colPos = CollisionPos(hit, transform.position, endVel);


        // Handle slopes affecting velocity and stuff
        if (!isSteep)
          endVel.y = 0;
        if (isSteep && contactAngle < 90)
          velocity *= math.pow((float2)hit.normal * -1 + 1, steepVelocityProjectPower);
        else if (!onGround && velocity.y < 0 && contactAngle > 90)
          velocity.x *= math.abs(hit.normal.x) * -1 + 1;
        else
          velocity *= math.abs(hit.normal) * -1 + 1;



        if (!cast.TryMoveTo(colPos))
          if (i != 0 && onGround && (contactAngle % 90 < nearAxisAngleOffsetRange || contactAngle % 90 > 90 - nearAxisAngleOffsetRange))
            cast.TryMoveTo(colPos + hit.normal * nearAxisAngleOffset);



        // Project velocity along normal of collision
        if (!onGround || contactAngle >= 90 || contactAngle < maxAngle || endVel.x == 0 || hit.normal.x >= 0 == endVel.x >= 0) // Fixes going towards slopes causing jitter
          endVel = Vector3.Project((Vector2)endVel, new Vector2(-hit.normal.y, hit.normal.x)).xy();

      } else {

        // Free move
        cast.TryMoveTo(transform.position.AddXY(endVel));

        // Down slopes
        if (endVel.x != 0 && onGround && !onSlope) {
          // !!! PROJECT ALONG GROUND NORMAL INSTEAD
          var dir = Vector2.down * (endVel.x * math.tan(maxAngle * Mathf.Deg2Rad) + maxHeightStep);
          var downHit = cast.Cast(transform.position, dir);

          if (downHit && (math.abs(downHit.normal.x) < 0.00001f || (endVel.x < 0 == downHit.normal.x < 0)) && Vector2.Angle(Vector2.up, downHit.normal) <= maxAngle) {
            var colPos = CollisionPos(downHit, transform.position, dir);
            cast.TryMoveTo(colPos);
          }
        }
        break;
      }
      if (endVel.Equals(Vector2.one)) {
        break;
      }
    }
  }

  void OnDrawGizmos() {
    if (debug.showDirectionalCollisions) {
      float2 min = new float2(float.PositiveInfinity); // Both values are used for both axes
      float2 max = new float2(float.NegativeInfinity); // Both values are used for both axes
      foreach (var col in GetComponents<Collider2D>()) {
        min = new float2(math.min(min.x, col.bounds.min.x), math.min(min.y, col.bounds.min.y));
        max = new float2(math.max(max.x, col.bounds.max.x), math.max(max.y, col.bounds.max.y));
      }
      var top = new Vector3(min.x + (max.x - min.x) / 2, max.y);
      var down = new Vector3(min.x + (max.x - min.x) / 2, min.y);
      var right = new Vector3(max.x, min.y + (max.y - min.y) / 2);
      var left = new Vector3(min.x, min.y + (max.y - min.y) / 2);
      Gizmos.color = debug.showDirectionalCollisions.color * (onLeft ? 1 : 0.1f);
      Gizmos.DrawLine(transform.position, left + Vector3.left * dirCollisionTestLength); // left
      Gizmos.color = debug.showDirectionalCollisions.color * (onRight ? 1 : 0.1f);
      Gizmos.DrawLine(transform.position, right + Vector3.right * dirCollisionTestLength); // right
      Gizmos.color = debug.showDirectionalCollisions.color * (onGround ? 1 : 0.1f);
      Gizmos.DrawLine(transform.position, down + Vector3.down * dirCollisionTestLength); // down
      Gizmos.color = debug.showDirectionalCollisions.color * (onCeiling ? 1 : 0.1f);
      Gizmos.DrawLine(transform.position, top + Vector3.up * dirCollisionTestLength); // up
    }
  }

  [System.Serializable]
  public class DebugOptions {
    [Tooltip("Stops the movement caused by this component")]
    public bool noMove;
    public DebugOption showDirectionalCollisions;
    public DebugOption hitNormal;
    public DebugOption allHitNormals;
    public DebugOption casts;
  }

  [System.Serializable]
  public class DebugOption {
    public bool enabled = false;
    public Color color = Color.white;

    public static implicit operator bool(DebugOption self) => self.enabled;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2DCharacter : MonoBehaviour {

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
  public float dirCollisionTestLength = 0.01f;
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

  [SerializeField]
  private DebugOptions debug;

  private bool validPrevcollider = false;
  private TransformData colPrevTransform;

  private Collider2D col;
  private Rigidbody2D rb;


  private void Start() {
    col = GetComponent<BoxCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    gravity = defaultGravity;
  }


  void LateUpdate() {
    List<bool> preDisabled = new List<bool>();
    List<Transform> children = new List<Transform>();
    if (ignoreChildColliders) {
      foreach (Transform child in transform) {
        preDisabled.Add(!child.gameObject.activeSelf);
        children.Add(child);
        child.gameObject.SetActive(false);
      }
    }

    if (moveWithGround && validPrevcollider && onGround) MoveWithGround();
    var postMovePos = transform.position;
    if (Input.GetKeyDown(KeyCode.R)) transform.position = Vector3.zero;

    if (debug.noMove) {
      var prevPos = transform.position;
      Physics();
      transform.position = prevPos;
    } else Physics();

    onGround = Cast(transform.position, Vector2.down * dirCollisionTestLength);
    onCeiling = Cast(transform.position, Vector2.up * dirCollisionTestLength);
    onRight = Cast(transform.position, Vector2.right * dirCollisionTestLength);
    onLeft = Cast(transform.position, Vector2.left * dirCollisionTestLength);

    stationary = postMovePos == transform.position;

    if (onGround) {
      slopeAngle = Vector2.Angle(Vector2.up, onGround.normal);
      onSlopeRight = !onSlopeLeft && slopeAngle > maxAngle && onGround.normal.x < 0;
      onSlopeLeft = slopeAngle > maxAngle && onGround.normal.x >= 0;
      onSlope = onSlopeLeft || onSlopeRight;

      if (moveWithGround && !onSlope) {
        colPrevTransform = onGround.collider.gameObject.transform.Save();
        validPrevcollider = true;
      }
    } else {
      validPrevcollider = false;
      slopeAngle = 0;
      onSlopeRight = false;
      onSlopeLeft = false;
      onSlope = false;
    }
    staticVelocity = Vector2.zero;

    if (ignoreChildColliders) {
      for (int i = 0; i < preDisabled.Count; i++) {
        if (!preDisabled[i]) {
          children[i].gameObject.SetActive(true);
        }
      }
    }
  }

  private bool Collides(Vector2 pos) => Cast(pos, Vector2.zero);

  private RaycastHit2D Cast(Vector2 start, Vector2 dir) {
    Vector2 normalized = dir.x == 0 && dir.y == 0 ? Vector2.right : dir.normalized;
    float length = dir.magnitude;
    var results = new RaycastHit2D[1];
    var prevPos = transform.position;
    transform.position = start;
    Physics2D.SyncTransforms();
    rb.Cast(normalized, layers, results, length);
    transform.position = prevPos;
    Physics2D.SyncTransforms();
    if (debug.casts) Debug.DrawRay(start, dir, debug.casts.color, 0);
    if (results[0] && debug.allHitNormals) Debug.DrawRay(results[0].point, results[0].normal, debug.allHitNormals.color, 0);
    return results[0];
  }



  void MoveWithGround() {
    var colGo = onGround.collider.gameObject;
    if (colGo.isStatic) return; // Moving or deactivating static things breaks things
    colGo.SetActive(false);
    var dir = (colGo.transform.position - colPrevTransform.position).xy();
    var hit = Cast(transform.position, dir);
    if (!hit) {
      transform.position += colGo.transform.position - colPrevTransform.position;
      // !!! Rotation and scaling is not accounted for
      // transform.rotation = colTransform.rotation * Quaternion.Inverse(colNewTransform.rotation) * transform.rotation;
      // transform.localScale += colTransform.localScale - colNewTransform.localScale;
    } else {
      var newPos = CollisionPos(hit, transform.position, dir).xyo();
      if (!Collides(newPos))
        transform.position = newPos;
    }

    colGo.SetActive(true);
  }

  public Vector2 CollisionPos(RaycastHit2D hit, Vector2 origin, Vector2 dir) {
    if (hit.distance <= contactOffset) return origin;
    return origin + dir.SetLen(hit.distance - contactOffset);
  }

  void Physics() {
    rb.useFullKinematicContacts = true;
    float multiplier = math.max(0f, 1f - drag * Time.deltaTime);
    velocity *= multiplier;
    velocity.y -= gravity * Time.deltaTime;

    if (Collides(transform.position)) {
      Debug.LogWarning("Rigidbody was inside a collider");
      transform.position += new Vector3(0, 0.1f, 0);
    }

    var endVel = (staticVelocity + velocity + debugVelocity) * Time.deltaTime + pureVelocity;
    var forceSteps = false;

    for (int i = 0; i < maxPhysicsIters; i++) {
      var hit = Cast(transform.position, endVel);
      if (hit && debug.hitNormal)
        Debug.DrawRay(hit.point, hit.normal, debug.hitNormal.color, 0);
      if (hit) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, hit.normal);
        bool isSteep = contactAngle > maxAngle;
        if (forceSteps || ((onGround || airHeightStep) && isSteep) && Vector2.Angle(Vector2.up, endVel) < 135) {
          forceSteps = false;
          var heighStep = maxHeightStep * 2;
          for (int j = 0; j < maxHeightVerticalSteps; j++) {
            heighStep /= 2;
            var pos = transform.position.xy().AddY(heighStep);

            bool breakOuter = false;
            RaycastHit2D horHit;
            float horStep = endVel.x * 2;
            for (int k = 0; k < maxHeightHorizontalSteps; k++) {
              horStep /= 2;
              horHit = Cast(pos, new Vector2(horStep, 0));
              if (!horHit)
                break;
              else if (k == maxHeightHorizontalSteps - 1) {
                breakOuter = true;
                break;
              }
            }
            if (breakOuter) break;
            var dir = new Vector2(0, -heighStep);
            var downPos = pos.AddX(horStep);
            var downHit = Cast(downPos, dir);
            if (downHit && Vector2.Angle(Vector2.up, downHit.normal) < maxAngle) {
              var collisionPos = CollisionPos(downHit, downPos, dir);
              if (!Collides(collisionPos))
                transform.position = collisionPos;
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



        if (!Collides(colPos) || Input.GetKey(KeyCode.P))
          transform.position = colPos;
        else if (contactAngle % 90 < 2 || contactAngle % 90 > 88) forceSteps = true; // fix physics aids


        // Project velocity along normal of collision
        if (!onGround || contactAngle >= 90 || contactAngle < maxAngle || endVel.x == 0 || hit.normal.x >= 0 == endVel.x >= 0) // Fixes going towards slopes causing jitter
          endVel = Vector3.Project((Vector2)endVel, new Vector2(-hit.normal.y, hit.normal.x)).xy();

      } else {

        // Free move
        if (!Collides(transform.position.AddXY(endVel)))
          transform.position += new Vector3(endVel.x, endVel.y);

        // Down slopes
        if (onGround) {
          var dir = Vector2.down * (endVel.x * math.tan(maxAngle * Mathf.Deg2Rad) + maxHeightStep);
          var downHit = Cast(transform.position, dir);

          if (downHit && (math.abs(downHit.normal.x) < 0.00001f || (endVel.x < 0 == downHit.normal.x < 0)) && Vector2.Angle(Vector2.up, downHit.normal) <= maxAngle) {
            var colPos = CollisionPos(downHit, transform.position, dir);
            if (!Collides(colPos))
              transform.position = colPos;
            else {
              print("asdasd");
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
  private class DebugOptions {
    [Tooltip("Stops the movement caused by this component")]
    public bool noMove;
    public DebugOption showDirectionalCollisions;
    public DebugOption hitNormal;
    public DebugOption allHitNormals;
    public DebugOption casts;
  }

  [System.Serializable]
  private class DebugOption {
    public bool enabled = false;
    public Color color = Color.white;

    public static implicit operator bool(DebugOption self) => self.enabled;
  }
}

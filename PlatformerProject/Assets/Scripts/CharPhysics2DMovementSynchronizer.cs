using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component allows moving colliders to more accurately collide with CharPhysics2D objects
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CharPhysics2DMovementSynchronizer : MonoBehaviour {

  [Tooltip("Layers which are checked by the raycasts")]
  public ContactFilter2D layers;
  [Tooltip("Check collisions with CharPhysics2D objects with a raycast. Otherwise only direct collions are tested")]
  public bool sweep = false;

  private Vector3 prevPos = Vector3.zero;
  private Rigidbody2D rb;
  private Collider2D col;

  // Start is called before the first frame update
  void Start() {
    prevPos = transform.position;
    rb = GetComponent<Rigidbody2D>();
    col = GetComponent<Collider2D>();
  }

  // Update is called once per frame
  void Update() {
    if (sweep) Sweep();
    else Overlaps();
  }

  // LateUpdate is called once per frame after Update calls
  void LateUpdate() {
    prevPos = transform.position;
  }

  void Sweep() {
    var newPos = transform.position;
    transform.position = prevPos;
    Physics2D.SyncTransforms();

    var dir = prevPos - newPos;
    var results = new List<RaycastHit2D>();

    if (rb == null) col.Cast(dir.normalized, layers, results, dir.magnitude);
    else rb.Cast(dir.normalized, layers, results, dir.magnitude);

    foreach (var hit in results)
      if (CheckResult(hit.collider))
        break;

    transform.position = newPos;
    Physics2D.SyncTransforms();
  }

  void Overlaps() {
    var results = new List<Collider2D>();
    if (rb == null) col.OverlapCollider(layers, results);
    else rb.OverlapCollider(layers, results);

    foreach (var collider in results)
      if (CheckResult(collider))
        break;
  }

  bool CheckResult(Collider2D collider) {
    if (collider.GetComponent<CharPhysics2D>()) {
      // Check collision point
      // Move based on that etc etc
      return true;
    }
    return false;
  }

}

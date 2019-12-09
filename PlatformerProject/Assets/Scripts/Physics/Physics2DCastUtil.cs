using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2DCastUtil {

  public Transform transform;
  public Rigidbody2D rb;
  public ContactFilter2D layers;

  public Physics2DCastUtil(Transform transform, Rigidbody2D rb, ContactFilter2D layers) {
    this.transform = transform;
    this.rb = rb;
    this.layers = layers;
  }

  public RaycastHit2D Collides(Vector2 pos) => Cast(pos, Vector2.zero);

  public bool TryMoveTo(Vector2 pos) {
    var cast = Cast(pos, Vector2.zero);
    if (cast)
      return false;
    transform.position = pos;
    return true;
  }

  public RaycastHit2D Cast(Vector2 start, Vector2 dir) {
    Vector2 normalized = dir.x == 0 && dir.y == 0 ? Vector2.right : dir.normalized;
    float length = dir.magnitude;
    var results = new RaycastHit2D[1];
    var prevPos = transform.position;
    transform.position = start;
    Physics2D.SyncTransforms();
    rb.Cast(normalized, layers, results, length);
    transform.position = prevPos;
    Physics2D.SyncTransforms();
    return results[0];
  }
}

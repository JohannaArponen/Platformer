using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public KeyCode jumpKey = KeyCode.Space;
  public KeyCode up = KeyCode.W;
  public KeyCode down = KeyCode.S;
  public KeyCode right = KeyCode.D;
  public KeyCode left = KeyCode.A;

  public float jumpStrength = 10f;
  public float moveSpeedTarget = 5f;
  public float baseGavity = 20f;
  public float jumpPressGravity = 15f;
  public float downPressGravity = 30f;

  public float xGroundResistance = 0.88f;
  public float xAirResistance = 0.97f;

  public float collisionMargin = 0.15f;
  public float pushBack = 0.00f;

  Rigidbody2D rb;
  new BoxCollider2D collider;

  float gravity;
  Vector2 velocity;

  float width;
  float height;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    collider = GetComponent<BoxCollider2D>();
    gravity = baseGavity;
    width = collider.size.x;
    height = collider.size.y;
  }
  // Update is called once per frame
  void Update() {
    var floored = Floored();
    var ceilinged = Ceilinged();
    var righted = Righted();
    var lefted = Lefted();

    var iseilinged = Lefted();
    if (!Input.GetKey(up) && !Input.GetKey(down)) {
      if (floored) {
        velocity.x *= xGroundResistance;
      } else {
        velocity.x *= xAirResistance;
      }
    }
    gravity = baseGavity;
    if (Input.GetKeyDown(jumpKey) && floored) {
      velocity -= new Vector2(0, velocity.y); // normalize y
      velocity += Vector2.up * jumpStrength;
    }
    if (Input.GetKey(jumpKey)) {
      gravity = jumpPressGravity;
    }

    if (Input.GetKey(up)) {
    }
    if (Input.GetKey(down)) {
      if (floored) {
        // Crouch
      } else {
        gravity = downPressGravity;
      }
    }
    if (Input.GetKey(right) && velocity.x < moveSpeedTarget) {
      velocity.x += moveSpeedTarget;
    }
    if (Input.GetKey(left) && velocity.x > -moveSpeedTarget) {
      velocity.x -= moveSpeedTarget;
    }

    // Collision
    if (righted || lefted) {
      if (righted && velocity.x > 0) {
        if (!lefted) {
          var pos = transform.position;
          pos.x = Mathf.Min(pos.x, pos.x - pushBack);
          transform.position = pos;
        }
        velocity.x = 0;
      }
      if (lefted && velocity.x <= 0) {
        if (!righted) {
          var pos = transform.position;
          pos.x = Mathf.Max(pos.x, pos.x + pushBack);
          transform.position = pos;
        }
        velocity.x = 0;
      }
    }
    if (ceilinged && velocity.y > 0) {
      if (!floored) {
        var pos = transform.position;
        pos.y = Mathf.Min(pos.y, pos.y - pushBack);
        transform.position = pos;
      }
      velocity.y = 0;
    }
    if (floored && velocity.y <= 0) {
      if (!ceilinged) {
        var pos = transform.position;
        pos.y = Mathf.Max(pos.y, pos.y + pushBack);
        transform.position = pos;
      }
      velocity.y = 0;
    } else { // Gravity
      velocity = new Vector2(velocity.x, velocity.y - gravity * Time.deltaTime);
    }

    transform.Translate(velocity * Time.deltaTime);
  }

  RaycastHit2D Floored() => Physics2D.Raycast(
    BottomLeftCorner() + new Vector2(collisionMargin, 0),
    Vector2.right, width - collisionMargin * 2
  );
  RaycastHit2D Ceilinged() => Physics2D.Raycast(
    TopLeftCorner() + new Vector2(collisionMargin, 0),
    Vector2.right, width - collisionMargin * 2
  );
  RaycastHit2D Righted() => Physics2D.Raycast(
    TopRigthCorner() + new Vector2(0, collisionMargin),
    Vector2.down, height - collisionMargin * 2
  );
  RaycastHit2D Lefted() => Physics2D.Raycast(
    TopLeftCorner() + new Vector2(0, collisionMargin),
    Vector2.down, height - collisionMargin * 2
  );

  Vector2 TopRigthCorner() => new Vector2(transform.position.x + width / 2, transform.position.y + width / 2);
  Vector2 TopLeftCorner() => new Vector2(transform.position.x - width / 2, transform.position.y + width / 2);
  Vector2 BottomRigthCorner() => new Vector2(transform.position.x + width / 2, transform.position.y - width / 2);
  Vector2 BottomLeftCorner() => new Vector2(transform.position.x - width / 2, transform.position.y - width / 2);
}

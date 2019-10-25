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
  public float moveSpeed = 5f;
  public float noMoveDecay = 0.5f;
  public float baseGavity = 1f;
  public float jumpPressGravity = 0.5f;
  public float downPressGravity = 1.5f;

  public float collisionMargin = 0.1f;


  Rigidbody2D rb;
  new BoxCollider2D collider;

  float width;
  float height;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    collider = GetComponent<BoxCollider2D>();
    rb.gravityScale = baseGavity;
    width = collider.size.x;
    height = collider.size.y;
  }
  // Update is called once per frame
  void Update() {
    var floored = Floored();
    var ceilinged = Ceilinged();
    var righted = Righted();
    var lefted = Lefted();

    rb.gravityScale = baseGavity;
    if (Input.GetKeyDown(jumpKey) && floored) {
      rb.velocity -= new Vector2(0, rb.velocity.y); // normalize y
      rb.velocity += Vector2.up * jumpStrength;
    }
    if (Input.GetKey(jumpKey)) {
      rb.gravityScale = jumpPressGravity;
    }

    if (Input.GetKey(down)) {
      if (floored) {
        // Crouch
      } else {
        rb.gravityScale = downPressGravity;
      }
    }
    if (Input.GetKey(right) && rb.velocity.x < moveSpeed) {
      rb.velocity += new Vector2((moveSpeed - rb.velocity.x) * 10 * Time.deltaTime, 0);
    }
    if (Input.GetKey(left) && rb.velocity.x > -moveSpeed) {
      rb.velocity -= new Vector2((moveSpeed - -rb.velocity.x) * 10 * Time.deltaTime, 0);
    }
    if (!Input.GetKey(left) && !Input.GetKey(right)) {
      rb.velocity -= new Vector2(rb.velocity.x * noMoveDecay, 0) * Time.deltaTime;
    }
  }

  RaycastHit2D Floored() => Physics2D.Raycast(BottomLeftCorner() - new Vector2(0, collisionMargin), Vector2.right, width);
  RaycastHit2D Ceilinged() => Physics2D.Raycast(TopLeftCorner() + new Vector2(0, collisionMargin), Vector2.right, width);
  RaycastHit2D Righted() => Physics2D.Raycast(TopRigthCorner() + new Vector2(collisionMargin, 0), Vector2.down, height);
  RaycastHit2D Lefted() => Physics2D.Raycast(TopLeftCorner() - new Vector2(collisionMargin, 0), Vector2.down, height);

  Vector2 TopRigthCorner() => new Vector2(transform.position.x + width / 2, transform.position.y + width / 2);
  Vector2 TopLeftCorner() => new Vector2(transform.position.x - width / 2, transform.position.y + width / 2);
  Vector2 BottomRigthCorner() => new Vector2(transform.position.x + width / 2, transform.position.y - width / 2);
  Vector2 BottomLeftCorner() => new Vector2(transform.position.x - width / 2, transform.position.y - width / 2);
}

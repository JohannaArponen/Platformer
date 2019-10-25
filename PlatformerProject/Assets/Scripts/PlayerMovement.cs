using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public KeyCode jump = KeyCode.Space;
  public KeyCode up = KeyCode.W;
  public KeyCode down = KeyCode.S;
  public KeyCode right = KeyCode.D;
  public KeyCode left = KeyCode.A;

  public float jumpStrength = 10f;
  public float moveSpeed = 5f;
  public float noMoveDecay = 10f;
  public float noMoveDecayGround = 20f;
  public float baseGavity = 1f;
  public float onGroundGravity = 10f;
  public float jumpPressGravity = 0.5f;
  public float downPressGravity = 1.5f;

  public float collisionMargin = 0.1f;
  public float maxSlopeAngle = 45;

  [System.Serializable]
  public struct Animations {
    public string move;
    public string jump;
    public string fall;
    public string idle;
    public string run;
    public string crouch;
  }

  Vector2 gravity;

  Rigidbody2D rb;
  new BoxCollider2D collider;

  float width;
  float height;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    collider = GetComponent<BoxCollider2D>();
    gravity = Vector2.down * baseGavity;
    width = collider.size.x;
    height = collider.size.y;
  }
  // Update is called once per frame
  void Update() {
    var floored = Floored();
    var ceilinged = Ceilinged();
    var righted = Righted();
    var lefted = Lefted();

    gravity = gravity.normalized * baseGavity;
    if (Input.GetKeyDown(jump) && floored) {
      rb.velocity -= new Vector2(0, rb.velocity.y); // normalize y
      rb.velocity += Vector2.up * jumpStrength;
    } else if (floored) {
      gravity = gravity.normalized * onGroundGravity;
    }
    if (Input.GetKey(jump)) {
      gravity = gravity.normalized * jumpPressGravity;
    }

    if (Input.GetKey(down)) {
      if (floored) {
        // Crouch
      } else {
        gravity = gravity.normalized * downPressGravity;
      }
    }
    if (Input.GetKey(right) && rb.velocity.x < moveSpeed) {
      rb.velocity += new Vector2((moveSpeed - rb.velocity.x) * 10 * Time.deltaTime, 0);
    }
    if (Input.GetKey(left) && rb.velocity.x > -moveSpeed) {
      rb.velocity -= new Vector2((moveSpeed - -rb.velocity.x) * 10 * Time.deltaTime, 0);
    }
    if (!Input.GetKey(left) && !Input.GetKey(right)) {
      if (floored && !Input.GetKeyDown(jump)) rb.velocity -= rb.velocity * noMoveDecayGround * Time.deltaTime;
      else rb.velocity -= new Vector2(rb.velocity.x * noMoveDecay, 0) * Time.deltaTime;
    }

    rb.velocity += gravity * Time.deltaTime;
  }

  void OnCollisionEnter2D(Collision2D collision) => HugGround(collision.contacts[0].normal);
  void OnCollisionStay2D(Collision2D collision) => HugGround(collision.contacts[0].normal);
  void OnCollisionExit2D(Collision2D collision) => gravity = Vector2.down * baseGavity;

  void HugGround(Vector2 normal) {
    if (Vector2.Angle(normal, Vector2.up) <= maxSlopeAngle) {
      gravity = normal * -1;
    } else {
      gravity = Vector2.down * baseGavity;
    }
  }


  public static Vector2 Rotate(Vector2 v, float degrees) {
    float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
    float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

    float tx = v.x;
    float ty = v.y;
    v.x = (cos * tx) - (sin * ty);
    v.y = (sin * tx) + (cos * ty);
    return v;
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

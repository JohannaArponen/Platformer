using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Move : MonoBehaviour {

  public float speed = 1f;

  public float defaultGravity = 1000f;
  private float gravity = 10f;
  public float drag = 0.05f;
  public int maxPhysicsIters = 3;
  public float maxSlope = 45;
  public float maxHeightStep = 0.1f;
  public int maxHeightStepTests = 1;

  public LayerMask layers;
  public float2 velocity;

  new BoxCollider2D collider;

  bool crouch = false;
  bool jump = false;

  float height;
  float width;


  void Start() {
    collider = GetComponent<BoxCollider2D>();
    width = collider.bounds.size.x;
    height = collider.bounds.size.y;
    gravity = defaultGravity;
  }

  void Update() {
    jump = Input.GetButton("Jump");
    crouch = (Input.GetAxisRaw("Vertical") < 0);
    var move = new float2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);

    var moveVelocity = (move + velocity) * Time.deltaTime;

    for (int i = 0; i < maxPhysicsIters; i++) {
      var contact = Physics2D.BoxCast(transform.position, new float2(width, height), 0, math.normalizesafe(moveVelocity, float2.zero), math.length(moveVelocity), layers);
      if (contact) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, contact.normal);
        if (contactAngle > maxSlope) {
          for (int j = 0; j < maxHeightStepTests; j++) {
            var pos = transform.position + new Vector3(0, maxHeightStep / i, 0);
            var newContact = Physics2D.BoxCast(pos, new float2(width, height), 0, math.normalizesafe(moveVelocity, float2.zero), math.length(moveVelocity), layers);
            if (!newContact) {
              contact = newContact;
              var downVector = new float2(0, moveVelocity.y);
              var downPos = pos + new Vector3(moveVelocity.x, 0, 0);
              contact = Physics2D.BoxCast(downPos, new float2(width, height), 0, math.normalizesafe(downVector, float2.zero), math.length(downVector), layers);
              Debug.DrawRay(contact.point, contact.normal * 1000, Color.yellow, 1);
              break;
            }
          }
        }
      }
      if (contact) {
        var newPos = new float3();
        if (Vector2.Angle(Vector2.up, contact.normal) <= maxSlope) {
          moveVelocity -= velocity * Time.deltaTime;
          velocity = float2.zero;
        } else {
          moveVelocity.x = 0;
        }
        velocity *= math.normalizesafe(contact.normal, float2.zero) * -1 + 1;
        var temp = Vector3.Project((Vector2)moveVelocity, Quaternion.Euler(0, 0, 90) * (Vector3)contact.normal);
        moveVelocity = (new float2(temp.x, temp.y));
        // Debug.DrawRay(transform.position, new Vector3(moveVelocity.x * 1000, moveVelocity.y * 1000), Color.green, 1);
        // Debug.DrawRay(contact.point, temp * 1000, Color.red, 1);

        if (contact.point.x > transform.position.x) {
          newPos.x = math.max(transform.position.x, contact.point.x - height / 2);
        } else {
          newPos.x = math.min(transform.position.x, contact.point.x + height / 2);
        }

        if (contact.point.y > transform.position.y) {
          newPos.y = math.max(transform.position.y, contact.point.y - width / 2);
        } else {
          newPos.y = math.min(transform.position.y, contact.point.y + width / 2);
        }

        if (!Physics2D.BoxCast((Vector3)newPos, new float2(width, height), 0, math.normalizesafe(moveVelocity, float2.zero), 0, layers))
          transform.position = newPos;
      } else {
        if (!Physics2D.BoxCast(transform.position + new Vector3(moveVelocity.x, moveVelocity.y, 0), new float2(width, height), 0, math.normalizesafe(moveVelocity, float2.zero), 0, layers))
          transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0);
        break;
      }
      if (moveVelocity.Equals(float2.zero)) {
        break;
      }
    }

    jump = false;
  }

  void FixedUpdate() {
    float multiplier = math.max(0f, 1f - drag * Time.fixedDeltaTime);
    velocity *= multiplier;
    velocity.y -= gravity * Time.fixedDeltaTime;
  }
}

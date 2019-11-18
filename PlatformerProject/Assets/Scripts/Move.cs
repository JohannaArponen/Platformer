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

  float2 size;


  void Start() {
    collider = GetComponent<BoxCollider2D>();
    gravity = defaultGravity;
    size = new float2(collider.bounds.size.x, collider.bounds.size.y);
  }

  void Update() {
    jump = Input.GetButton("Jump");
    crouch = (Input.GetAxisRaw("Vertical") < 0);
    var move = new float2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);

    var moveVelocity = (move + velocity) * Time.deltaTime;

    for (int i = 0; i < maxPhysicsIters; i++) {
      var contact = Physics2D.BoxCast(transform.position, size, 0, math.normalizesafe(moveVelocity, float2.zero), math.length(moveVelocity), layers);
      if (contact) {
        // Height steps, stairs etc.
        var contactAngle = Vector2.Angle(Vector2.up, contact.normal);
        if (contactAngle > maxSlope) {
          for (int j = 0; j < maxHeightStepTests; j++) {
            var currentStep = maxHeightStep / (j + 1);
            var pos = (Vector2)transform.position + new Vector2(0, currentStep);
            var newContact = Physics2D.BoxCast(pos, size, 0, new float2(moveVelocity.x >= 0 ? 1 : -1, 0), math.abs(moveVelocity.x), layers);
            if (newContact) Debug.DrawRay(newContact.point, newContact.normal * 1000, Color.yellow, 1);
            if (!newContact) {
              var downContact = Physics2D.BoxCast(pos + new Vector2(moveVelocity.x, 0), size, 0, new Vector2(0, -currentStep), currentStep, layers);
              transform.position = downContact ? downContact.centroid + new Vector2(0, 0.01f) : pos + new Vector2(moveVelocity.x, -currentStep);
              return;
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
          newPos.x = math.max(transform.position.x, contact.point.x - size.y / 2);
        } else {
          newPos.x = math.min(transform.position.x, contact.point.x + size.y / 2);
        }

        if (contact.point.y > transform.position.y) {
          newPos.y = math.max(transform.position.y, contact.point.y - size.x / 2);
        } else {
          newPos.y = math.min(transform.position.y, contact.point.y + size.x / 2);
        }

        if (!Physics2D.BoxCast((Vector3)newPos, size, 0, math.normalizesafe(moveVelocity, float2.zero), 0, layers))
          transform.position = newPos;
      } else {
        if (!Physics2D.BoxCast(transform.position + new Vector3(moveVelocity.x, moveVelocity.y, 0), size, 0, math.normalizesafe(moveVelocity, float2.zero), 0, layers))
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

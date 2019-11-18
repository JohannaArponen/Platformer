using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Move : MonoBehaviour {

  public float speed = 1f;

  public float defaultGravity = 1f;
  private float gravity = 1f;
  public float jumpGravity = 0.5f;
  public float drag = 0.05f;
  public int maxPhysicsIters = 10;
  public float physicsGap = 0.0001f;

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

    var modVelocity = (move + velocity) * Time.deltaTime;

    for (int i = 0; i < maxPhysicsIters; i++) {
      var contact = Physics2D.BoxCast(transform.position, new float2(width, height), 0, math.normalizesafe(modVelocity, new float2(1, 1)), math.length(modVelocity), layers);
      if (contact) {
        var newPos = new float3();
        var temp = Vector3.Project((Vector2)velocity, Quaternion.Euler(0, 90, 0) * (Vector3)contact.normal);
        velocity *= math.normalizesafe(new float2(temp.x, temp.y), float2.zero);
        temp = Vector3.Project((Vector2)modVelocity, Quaternion.Euler(0, 90, 0) * (Vector3)contact.normal);
        modVelocity *= math.normalizesafe(new float2(temp.x, temp.y), float2.zero);
        Debug.DrawRay(transform.position, new Vector3(modVelocity.x * 100000, modVelocity.y * 100000), Color.green, 1);
        Debug.DrawRay(transform.position, temp * 100000, Color.red, 1);

        if (contact.point.x > transform.position.x) {
          newPos.x = math.max(transform.position.x, contact.point.x - height / 2 - physicsGap);
        } else {
          newPos.x = math.min(transform.position.x, contact.point.x + height / 2 + physicsGap);
        }

        if (contact.point.y > transform.position.y) {
          newPos.y = math.max(transform.position.y, contact.point.y - width / 2 - physicsGap);
        } else {
          newPos.y = math.min(transform.position.y, contact.point.y + width / 2 + physicsGap);
        }

        if (!Physics2D.BoxCast((Vector3)newPos, new float2(width, height), 0, math.normalizesafe(modVelocity, new float2(1, 1)), 0, layers))
          transform.position = newPos;
      } else {
        if (!Physics2D.BoxCast(transform.position + new Vector3(modVelocity.x, modVelocity.y, 0), new float2(width, height), 0, math.normalizesafe(modVelocity, new float2(1, 1)), 0, layers))
          transform.position += new Vector3(modVelocity.x, modVelocity.y, 0);
        break;
      }
      if (modVelocity.Equals(float2.zero)) {
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

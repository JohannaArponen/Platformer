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

  public LayerMask layers;
  float2 velocity;

  new BoxCollider2D collider;

  float move;
  bool crouch = false;
  bool jump = false;

  float height;
  float width;


  void Start() {
    collider = GetComponent<BoxCollider2D>();
    height = collider.bounds.size.y;
    width = collider.bounds.size.x;
  }

  void Update() {
    jump = Input.GetButton("Jump");
    print(crouch = (Input.GetAxisRaw("Vertical") < 0));
    move = Input.GetAxisRaw("Horizontal") * speed;
  }

  void FixedUpdate() {
    float multiplier = Mathf.Max(0f, 1f - drag * Time.fixedDeltaTime);
    velocity = multiplier * velocity;
    velocity.y -= gravity * Time.fixedDeltaTime;

    var contact = Physics2D.BoxCast(transform.position, collider.bounds.size, 0, math.normalizesafe(velocity, float2.zero), math.length(velocity), layers);
    if (contact) {
      print(2);
      transform.position += new Vector3(velocity.x, velocity.y, 0);
    }
    jump = false;

    transform.position += new Vector3(velocity.x, velocity.y, 0);
  }
}

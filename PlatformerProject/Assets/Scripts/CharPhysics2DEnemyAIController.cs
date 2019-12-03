using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharPhysics2D))]
[RequireComponent(typeof(Enemy))]
public class CharPhysics2DEnemyAIController : MonoBehaviour {
  public float speed;
  public float jumpStrength;

  private CharPhysics2D physics;
  private Enemy enemy;

  private bool wannaMove = false;
  private bool wannaJump = false;


  void Start() {
    physics = GetComponent<CharPhysics2D>();
    enemy = GetComponent<Enemy>();
  }

  // Update is called once per frame
  void Update() {
    if (!physics.onSlope) {
      // Move
      float move = 0;

      if (move > 0 && physics.onSlopeRight) {
        move = 0;
      } else if (move < 0 && physics.onSlopeLeft) {
        move = 0;
      }
      physics.staticVelocity.x += move;

      if (physics.onGround) {
        // Jump
        if (wannaJump) {
          physics.velocity.y = jumpStrength;
        }
      }


    }
  }
}
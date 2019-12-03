using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharPhysics2D))]
[RequireComponent(typeof(Enemy))]
public class CharPhysics2DEnemyAI : MonoBehaviour {
  public float speed;
  public float jumpStrength;

  private CharPhysics2D physics;
  private Enemy enemy;

  public bool wannaMoveRight = false;
  public bool wannaMoveLeft = true;
  public bool wannaJump = false;


  void Start() {
    physics = GetComponent<CharPhysics2D>();
    enemy = GetComponent<Enemy>();
  }

  // Update is called once per frame
  void Update() {
    if (!physics.onSlope) {

      if (physics.onRight && Vector2.Angle(Vector2.up, physics.onRight.normal) > physics.maxSlopeAngle) {
        wannaMoveRight = false;
        wannaMoveLeft = true;
      } else if (physics.onLeft && Vector2.Angle(Vector2.up, physics.onLeft.normal) > physics.maxSlopeAngle) {
        wannaMoveRight = true;
        wannaMoveLeft = false;
      }
      // Move
      float move = 0;

      if (wannaMoveLeft)
        move -= speed;
      else if (wannaMoveRight)
        move += speed;

      // Dont allow moving towards steep slopes
      if ((move > 0 && physics.onSlopeRight) || (move < 0 && physics.onSlopeLeft)) move = 0;

      physics.staticVelocity.x += move;

      if (physics.onGround) {
        // Jump
        if (wannaJump) {
          physics.velocity.y = jumpStrength;
          wannaJump = false;
        }
      }


    }
  }
}
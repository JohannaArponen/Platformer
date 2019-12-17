using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Physics2DCharacter))]
public class Physics2DCharEnemyAI : MonoBehaviour {
  public float speed;
  [PositiveValueOnly]
  public float jumpStrength;

  private Enemy enemy;
  private Physics2DCharacter physics;

  public bool wannaMoveRight = false;
  public bool wannaMoveLeft = true;
  public bool wannaJump = false;
  public float extraJumpPower = 0;

  void OnTriggerEnter2D(Collider2D col) {
    var jumper = col.GetComponent<EnemyJumpPoint>();
    if (jumper != null) {
      if (wannaMoveRight == jumper.right) {
        wannaJump = true;
        extraJumpPower = jumper.extraForce;
      }
    }
  }


  void Start() {
    physics = GetComponent<Physics2DCharacter>();
    enemy = GetComponent<Enemy>();
  }

  // Update is called once per frame
  void Update() {
    if (!physics.onSlope) {

      if (physics.stationary) {
        // And then you switch
        wannaMoveRight = !wannaMoveRight;
        wannaMoveLeft = !wannaMoveRight;
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
          physics.velocity.y = jumpStrength + extraJumpPower;
          extraJumpPower = 0;
          wannaJump = false;
        }
      }


    }
  }
}
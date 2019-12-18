using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AntiAidsSpider : MonoBehaviour {
  private Rect room;
  public float gravity;
  public float speed;
  private BossManager man;
  private Collider2D col;
  private Vector2 velocity = new Vector2(0, 0);

  private bool landed = false;
  private Vector2 dir = Vector2.down;
  private bool clockwise = true;




  void Start() {
    man = FindObjectOfType<BossManager>();
    room = man.room;
    col = GetComponent<Collider2D>();
  }

  void Update() {
    var prevPos = transform.position;
    if (landed) {
      transform.position = transform.position.AddXY(dir * speed * Time.deltaTime);
      Physics2D.SyncTransforms();
      if (!room.Contains(col.bounds.max) || !room.Contains(col.bounds.min)) {
        if (clockwise) dir = new Vector2(-dir.y, dir.x);
        else dir = new Vector2(dir.y, -dir.x);
        transform.position = prevPos;
      }
    } else {
      velocity.y -= gravity * Time.deltaTime;
      transform.position += velocity.xyo();
      if (!room.Contains(col.bounds.max) || !room.Contains(col.bounds.min)) {
        landed = true;
        transform.position = prevPos;
        clockwise = !(man.player.transform.position.x > transform.position.x);
        dir = new Vector2(man.player.transform.position.x > transform.position.x ? 1 : -1, 0);
      }
    }
  }

  void OnColliderEnter2D(Collider2D other) {
    if (other.tag != "Player") return;
    var rocketBoost = other.GetComponent<RocketBoost>();
    if (rocketBoost == null) return;

    if (rocketBoost.enabled) {
      man.DeletSpiders();
    }
  }
}

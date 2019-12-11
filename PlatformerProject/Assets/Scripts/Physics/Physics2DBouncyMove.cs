using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2DBouncyMove : MonoBehaviour {

  public float2 velocity;
  [Tooltip("Layers which are checked by the raycasts")]
  public ContactFilter2D layers;
  [Range(0, 90)]
  [Tooltip("Minimum collision angle for reflection. Smaller collision angles make the object slide along the collision")]
  public float minAngle;
  [Range(0, 90)]
  [Tooltip("Maximum collision angle for reflection. Bigger collision angles stop the object")]
  public float maxAngle;
  [Tooltip("Maximum allowed iterations. This many collisions per frame can be handled. Usually only one iteration is done but in corners 2 might be preferred. At high speeds more iterations may be desirable")]
  public int maxIterations = 5;
  [Tooltip("A new iteration is not done if the velocity vector has lower length")]
  public float minIterationVelocity = 0.01f;
  private float minIterationVelocitySQ;
  [Tooltip("Avoid getting stuck inside colliders by offsetting collision positions")]
  public float contactOffset = 0.003771f;


  private Physics2DCastUtil cast;
  private Collider2D col;
  private Rigidbody2D rb;

  // Start is called before the first frame update
  void Start() {
    col = GetComponent<BoxCollider2D>();
    rb = GetComponent<Rigidbody2D>();
    minIterationVelocitySQ = minIterationVelocity * minIterationVelocity;
  }
  void OnValidate() => minIterationVelocitySQ = minIterationVelocity * minIterationVelocity;


  // Update is called once per frame
  void Update() {
    if (cast == null) cast = new Physics2DCastUtil(transform, rb, layers);
    var endVel = velocity * Time.deltaTime;

    if (cast.Collides(transform.position)) {
      Debug.LogWarning("Physics2D Rigidbody was inside a collider");
      transform.position += new Vector3(0, 0.1f, 0);
    }

    for (int i = 0; i < maxIterations; i++) {
      var hit = cast.Cast(transform.position, endVel);
      if (hit) {
        var collisionPos = CollisionPos(hit, transform.position, endVel);
        cast.TryMoveTo(collisionPos);
        velocity = Vector2.Reflect(velocity, hit.normal);
        endVel *= 1 - hit.fraction;
        endVel = Vector2.Reflect(endVel, hit.normal);
        if (math.lengthsq(endVel) < minIterationVelocity) {
          break;
        }
      } else {
        cast.TryMoveTo(transform.position.AddXY(endVel));
        break;
      }
    }
  }

  public Vector2 CollisionPos(RaycastHit2D hit, Vector2 origin, Vector2 dir) {
    if (hit.distance <= contactOffset) return origin;
    return origin + dir.SetLen(hit.distance - contactOffset);
  }
}

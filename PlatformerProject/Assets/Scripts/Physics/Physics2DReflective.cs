using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using MyBox;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2DReflective : MonoBehaviour {

  public float2 velocity;
  [Tooltip("Layers which are checked by the raycasts")]
  public ContactFilter2D layers;
  [Range(0, 360)]
  [Tooltip("Restrict reflection angle (world space). E.g. having the value 10 would round the reflection angle to nearest 10, 90 would make the direction be up,down,right or left. Leave at 0 to disable. It is recommended that this is a divisor of 360")]
  public float reflectionAngleRounding = 0;
  [Range(0, 360)]
  [Tooltip("Offsets the reflection angle rounding calculation. e.g. value 10 with rounding set to 20 would round to nearest ... -10, 10, 30, 50 ...")]
  public float reflectionAngleRoundingOffset = 0;
  [MinMaxRange(0, 90)]
  [Tooltip("Range of collision angles to reflect from. Smaller collisions cause sliding and bigger stop the object")]
  public RangedFloat angles = new RangedFloat(0, 90);
  [Tooltip("Keep current speed when sliding")]
  public bool keepSpeedOnSlide = false;
  [PositiveValueOnly]
  [Tooltip("Maximum allowed iterations. This many collisions per frame can be handled. Usually only one iteration is done but in corners 2 might be preferred. At high speeds more iterations may be desirable")]
  public int maxIterations = 5;
  [PositiveValueOnly]
  [Tooltip("A new iteration is not done if the velocity vector has lower length")]
  public float minIterationVelocity = 0.01f;
  private float minIterationVelocitySQ;
  [PositiveValueOnly]
  [Tooltip("Avoid getting stuck inside colliders by offsetting collision positions")]
  public float contactOffset = 0.003771f;


  [HideInInspector]
  public bool didHitSomething;

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
    if (reflectionAngleRounding != 0) {
      var dir = Input.mousePosition.xy();
      var dirAngle = dir.Angle();
      var rounded = dirAngle.RoundToNearest(reflectionAngleRounding);
      Debug.DrawLine(Vector2.zero, dir, Color.green);
      Debug.DrawLine(Vector2.zero, dir.SetAngle(rounded), Color.red);
    }
    if (cast == null) cast = new Physics2DCastUtil(transform, rb, layers);
    var endVel = velocity * Time.deltaTime;

    if (cast.Collides(transform.position)) {
      Debug.LogWarning("Physics2D Rigidbody was inside a collider");
      transform.position += new Vector3(0, 0.1f, 0);
    }

    didHitSomething = false;
    for (int i = 0; i < maxIterations; i++) {
      var hit = cast.Cast(transform.position, endVel);
      if (hit) {
        didHitSomething = true;
        var collisionPos = CollisionPos(hit, transform.position, endVel);
        cast.TryTeleport(collisionPos);
        var angle = Vector2.Angle(velocity, hit.normal) - 90;
        if (angle > angles.max) {
          velocity = 0;
          break;
        }
        if (angle < angles.min) {
          if (angles.min != 0 && keepSpeedOnSlide)
            velocity = Vector3.Project((Vector2)velocity, new Vector2(-hit.normal.y, hit.normal.x)).xy().SetLen(math.length(velocity));
          else
            velocity = Vector3.Project((Vector2)velocity, new Vector2(-hit.normal.y, hit.normal.x)).xy();
          break;
        }
        endVel *= 1 - hit.fraction;
        if (math.lengthsq(endVel) < minIterationVelocitySQ)
          break;

        endVel = Vector2.Reflect(endVel, hit.normal);
        velocity = Vector2.Reflect(velocity, hit.normal);
        if (reflectionAngleRounding != 0) {
          var dirAngle = endVel.Angle();
          var rounded = dirAngle.RoundToNearest(reflectionAngleRounding);
          endVel = endVel.SetAngle(rounded);
          velocity = velocity.SetAngle(rounded);
        }
      } else {
        cast.TryTeleport(transform.position.Add(endVel));
        break;
      }
    }
  }

  public Vector2 CollisionPos(RaycastHit2D hit, Vector2 origin, Vector2 dir) {
    if (hit.distance <= contactOffset) return origin;
    return origin + dir.SetLen(hit.distance - contactOffset);
  }
}

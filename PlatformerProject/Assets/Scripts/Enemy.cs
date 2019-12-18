using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour {

  protected abstract bool kill { get; set; }
  protected abstract float health { get; set; }
  protected abstract float damage { get; set; }
  protected abstract float collisionDamage { get; set; }
  protected abstract float invulnerabilityDuration { get; set; }
  protected abstract float activeDistanceFromView { get; set; }

  protected ContactFilter2D contactFilter;
  /// <summary> Time to pass when changing alpha when flashing (invulnerable) </summary>
  protected float flickerSpeed = 0.1f;
  /// <summary> Alpha values for flashing </summary>
  protected (float a, float b) flickerAlpha = (0.25f, 0.5f);
  protected SpriteRenderer sr;
  protected Rect rect;
  protected Collider2D col;

  protected float invulnerabilityStart = float.NegativeInfinity;
  protected bool invulnerable { get => _invulnerable; set { if (_invulnerable = value) return; _invulnerable = value; if (value) OnInvulnerableStart(); else OnInvulnerableEnd(); } }
  protected bool _invulnerable = false;

  private bool _activated = false;

  private Camera cam;
  private bool inView;

  public bool activated {
    get => _activated; set {
      if (!_activated && value)
        OnActivate();
      else if (_activated && !value)
        OnDeactivate();
      _activated = value;
    }
  }


  public void Start() {
    cam = Camera.main;
    sr = GetComponentInChildren<SpriteRenderer>();
    col = GetComponentInChildren<Collider2D>();
    contactFilter = new ContactFilter2D();
    OnCreate();
    Updaterect();
    inView = GetSpawnRect().Overlaps(rect);
    if (inView) OnEnterView();
  }

  public void Update() {
    if (GetSpawnRect().Overlaps(rect)) {
      if (!inView) {
        inView = true;
        OnEnterView();
      }
    } else {
      if (inView) {
        inView = false;
        OnExitView();
      }
    }
    if (activated) {
      EnemyUpdate();
    }
  }

  protected void Updaterect() {
    if (sr == null) rect = new Rect(col.bounds.min.xy(), col.bounds.size.xy());
    rect = new Rect(sr.bounds.min.xy(), sr.bounds.size.xy());
  }

  protected Rect GetSpawnRect() {
    var topLeft = cam.ViewportToWorldPoint(Vector2.zero).xy().AddXY(-activeDistanceFromView);
    var bottomRight = cam.ViewportToWorldPoint(Vector2.one).xy().AddXY(activeDistanceFromView);
    Updaterect();
    MyUtil.DrawBoxXY(topLeft, bottomRight - topLeft, Color.green);
    if (sr == null) MyUtil.DrawBoxXY(col.bounds.min.xy(), col.bounds.size.xy(), Color.cyan);
    MyUtil.DrawBoxXY(sr.bounds.min.xy(), sr.bounds.size.xy(), Color.cyan);
    return new Rect(topLeft, bottomRight - topLeft);
  }


  // On creation
  virtual protected void OnCreate() {

  }
  // On hit duh. weapon may be null
  virtual protected void OnHit(float damage, Collider2D col, Weapon weapon) {
    invulnerabilityStart = Time.time;
    invulnerable = true;
    health -= damage;
    if (health <= 0) {
      OnKill(damage, col, weapon);
    }
  }
  // When killed. weapon may be null. col may be null
  virtual protected void OnKill(float damage, Collider2D col, Weapon weapon) {

  }
  // When activated
  virtual protected void OnActivate() {

  }
  // When deactivated
  virtual protected void OnDeactivate() {

  }
  // Just like Update but only called when activated
  virtual protected void EnemyUpdate() {
    if (kill) return;
    if (invulnerabilityStart < Time.time - invulnerabilityDuration) {
      if (invulnerable) {
        invulnerable = false;
        OnInvulnerableEnd();
      }
      var results = new List<Collider2D>();
      if (col.OverlapCollider(new ContactFilter2D(), results) > 0) {
        foreach (var result in results) {
          if (result.gameObject.tag == "Player") {
            print("HIT PLAYER");
            result.GetComponent<Lifes>().DamagePlayer(2);
            return;
          }
          var weapon = result.gameObject.GetComponent<Weapon>();
          if (weapon != null) {
            OnHit(weapon.damage, result, weapon);
          }
        }
      }
    } else OnInvulnerable();
  }
  // When enters visible area
  virtual protected void OnEnterView() {

  }
  // When exits visible area
  virtual protected void OnExitView() {

  }


  // When starting invulnerability
  protected virtual void OnInvulnerableStart() { }
  // When ending invulnerability
  protected virtual void OnInvulnerableEnd() {
    var color = sr.color;
    color.a = 1;
    sr.color = color;
  }

  // When invulnerable
  protected virtual void OnInvulnerable() {
    if (sr == null) return;
    var color = sr.color;
    color.a = Mathf.FloorToInt((Time.time - invulnerabilityStart) / flickerSpeed) % 2 == 1 ? flickerAlpha.a : flickerAlpha.b;
    sr.color = color;
  }
}

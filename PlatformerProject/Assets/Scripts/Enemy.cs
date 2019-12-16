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
  private float invulnerabilityStart = float.NegativeInfinity;

  private bool _activated = false;

  private Camera cam;
  private bool inView;
  private Rect rect;
  private SpriteRenderer sr;
  private Collider2D col;

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

  public void OnCollisionEnter2D(Collision2D collision) {
    if (Time.time > invulnerabilityStart + invulnerabilityDuration) {
      var weapon = collision.collider.gameObject.GetComponent<Weapon>();
      if (weapon != null) {
        invulnerabilityStart = Time.time;
        OnHit(weapon.damage, collision, weapon);
      }
    }
  }


  // On creation
  virtual protected void OnCreate() {

  }
  // On hit duh. weapon may be null
  virtual protected void OnHit(float damage, Collision2D col, Weapon weapon) {
    health -= damage;
    if (health <= 0) {
      OnKill(damage, col, weapon);
    }
  }
  // When killed. weapon may be null. col may be null
  virtual protected void OnKill(float damage, Collision2D col, Weapon weapon) {

  }
  // When activated
  virtual protected void OnActivate() {

  }
  // When deactivated
  virtual protected void OnDeactivate() {

  }
  // Just like Update but only called when activated
  virtual protected void EnemyUpdate() {

  }
  // When enters visible area
  virtual protected void OnEnterView() {

  }
  // When exits visible area
  virtual protected void OnExitView() {

  }
}

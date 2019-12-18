using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour {

  protected virtual float damage { get; set; } = 2;
  protected virtual float collisionDamage { get; set; } = 2;
  protected virtual float alphaFlickerSpeed { get; set; } = 0.25f;
  protected virtual (float a, float b) flickerAlpha { get; set; } = (0.25f, 0.5f);
  protected virtual float destroyDelay { get; set; } = 1;
  protected virtual float destroyTime { get; set; } = float.PositiveInfinity;
  protected virtual float activeDistanceFromView { get; set; } = 2;
  protected virtual float invulnerabilityDuration { get; set; } = 1;
  protected virtual float invulnerabilityStart { get; set; } = float.NegativeInfinity;

  [System.NonSerialized] protected float _health = 2;
  [System.NonSerialized] protected bool _active = false;
  [System.NonSerialized] protected bool _visible;
  [System.NonSerialized] protected bool _invulnerable = false;
  [System.NonSerialized] protected float _invulnerabilityStart = float.NegativeInfinity;

  protected virtual float health { get => _health; set => _health = value; }
  protected virtual bool active { get => _active; set { if (_active == value) return; _active = value; if (value) _OnActivate(); else _OnDeactivate(); } }
  protected virtual bool visible { get => _visible; set { if (_visible == value) return; _visible = value; if (value) _OnEnterView(); else _OnExitView(); } }
  protected virtual bool invulnerable {
    get => _invulnerable;
    set { if (value) _invulnerabilityStart = Time.time; if (_invulnerable == value) return; _invulnerable = value; if (value) _OnInvulnerableStart(); else _OnInvulnerableEnd(); }
  }

  protected virtual bool dead { get => health <= 0; set { if (dead == value) return; if (value) { var prevHP = health; health = 0; _OnKill(prevHP, null, null); } else health = 1; } }



  protected ContactFilter2D contactFilter;
  protected SpriteRenderer sr;
  protected Rect rect;
  protected Collider2D col;
  protected Camera cam;  



  void Start() {
    cam = Camera.main;
    sr = GetComponentInChildren<SpriteRenderer>();
    col = GetComponentInChildren<Collider2D>();
    contactFilter = new ContactFilter2D();
    _OnCreate();
    Updaterect();
    visible = GetSpawnRect().Overlaps(rect);
    if (!visible) _OnExitView();
  }

  void Update() {
    if (cam != null) {
      if (GetSpawnRect().Overlaps(rect)) {
        visible = true;
      } else {
        visible = false;
      }
    }
    if (active)
      if (dead)
        _OnDead();
      else
        _EnemyUpdate();
  }

  protected void Updaterect() {
    if (sr == null) rect = new Rect(col.bounds.min.xy(), col.bounds.size.xy());
    rect = new Rect(sr.bounds.min.xy(), sr.bounds.size.xy());
  }

  protected Rect GetSpawnRect() {
    if (cam == null) return default(Rect);
    var topLeft = cam.ViewportToWorldPoint(Vector2.zero).xy().AddXY(-activeDistanceFromView);
    var bottomRight = cam.ViewportToWorldPoint(Vector2.one).xy().AddXY(activeDistanceFromView);
    Updaterect();
    MyUtil.DrawBoxXY(topLeft, bottomRight - topLeft, Color.green);
    if (sr == null) MyUtil.DrawBoxXY(col.bounds.min.xy(), col.bounds.size.xy(), Color.cyan);
    MyUtil.DrawBoxXY(sr.bounds.min.xy(), sr.bounds.size.xy(), Color.cyan);
    return new Rect(topLeft, bottomRight - topLeft);
  }

  #region methods

  public virtual bool IsDead() => dead;

  public virtual float GetHealth() => health;
  public virtual float AddHealth(float value) => health += value;
  public virtual void SetHealth(float value) => health = value;

  public virtual float GetDamage() => damage;
  public virtual float GetCollisionDamage() => collisionDamage;
  public virtual bool IsInvulnerable() => invulnerable;


  public virtual void Heal(float heal) => health += heal;
  public virtual void Damage(float damage, Collider2D col = null, Weapon weapon = null) => health -= damage;
  public virtual void Kill(float damage = 1, Collider2D col = null, Weapon weapon = null) { if (!dead) Damage(health); }


  #endregion
    

  #region events

  // On creation
  protected virtual void OnCreate() { }
  protected virtual void _OnCreate() => OnCreate();

  // On heal
  protected virtual void OnHeal(float heal) { }
  protected virtual void _OnHeal(float heal) {
  }

  // When healed alive
  protected virtual void OnRevive(float heal) { }
  protected virtual void _OnRevive(float heal) => OnRevive(heal);

  // On hit. col may be null. weapon may be null
  protected virtual void OnHit(float damage, Collider2D col, Weapon weapon) { }
  protected virtual void _OnHit(float damage, Collider2D col, Weapon weapon) {
    invulnerabilityStart = Time.time;
    invulnerable = true;
    health -= damage;
    if (health <= 0) {
      OnHit(damage, col, weapon);
      _OnKill(damage, col, weapon);
    } else OnHit(damage, col, weapon);
  }

  // When killed. weapon may be null. col may be null
  protected virtual void OnKill(float damage, Collider2D col, Weapon weapon) { }
  protected virtual void _OnKill(float damage, Collider2D col, Weapon weapon) {
    OnKill(damage, col, weapon);
    Destroy(gameObject, destroyDelay);
        //var gm = GameObject.Find("GameManager");
        //gm.GetComponent<GameManager>().EnemyDestroy();
  }

  // When enemy is dead
  protected virtual void OnDead() { }
  protected virtual void _OnDead() => OnDead();

  // When enemy is dead
  protected virtual void OnDestroy() { }
  protected virtual void _OnDestroy() {
    OnDestroy();
    // Recheck for changed values
    if (destroyTime <= Time.time) {
      Destroy(gameObject);
    }
  }

  // When activated
  protected virtual void OnActivate() { }
  protected virtual void _OnActivate() => OnActivate();

  // When deactivated
  protected virtual void OnDeactivate() { }
  protected virtual void _OnDeactivate() => OnDeactivate();

  // Just like Update but only called when activated
  virtual protected void EnemyUpdate() { }
  virtual protected void _EnemyUpdate() {
    if (dead) return;
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
            result.GetComponent<Lifes>().DamagePlayer(collisionDamage, gameObject);
            return;
          }
          var weapon = result.gameObject.GetComponent<Weapon>();
          if (weapon != null) {
            OnHit(weapon.damage, result, weapon);
          }
        }
      } else invulnerable = false;
    } else {
      if (destroyTime <= Time.time) {
        OnDestroy();
      }
    }
    EnemyUpdate();
  }

  // When enters visible area
  protected virtual void OnEnterView() { }
  protected virtual void _OnEnterView() {
    if (dead) active = true;
    OnEnterView();
  }
  // When exits visible area
  protected virtual void OnExitView() { }
  protected virtual void _OnExitView() {
    if (dead) active = false;
    OnExitView();
  }


  // When starting invulnerability
  protected virtual void OnInvulnerableStart() { }
  protected virtual void _OnInvulnerableStart() => OnInvulnerableStart();
  // When invulnerable
  protected virtual void OnInvulnerable() { }
  protected virtual void _OnInvulnerable() {
    if (dead) invulnerable = false;
    else if (sr != null) {
      var color = sr.color;
      color.a = Mathf.FloorToInt((Time.time - invulnerabilityStart) / alphaFlickerSpeed) % 2 == 1 ? flickerAlpha.a : flickerAlpha.b;
      sr.color = color;
    }
    OnInvulnerable();
  }
  // When ending invulnerability
  protected virtual void OnInvulnerableEnd() { }
  protected virtual void _OnInvulnerableEnd() {
    if (sr != null) {
      var color = sr.color;
      color.a = 1;
      sr.color = color;
    }
    OnInvulnerableEnd();
  }

  #endregion
}

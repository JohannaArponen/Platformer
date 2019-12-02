﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Enemy : MonoBehaviour {

  protected abstract float health { get; set; }
  protected abstract float damage { get; set; }
  protected abstract float collisionDamage { get; set; }
  protected abstract float invulnerabilityDuration { get; set; }
  protected abstract float activeDistanceFromView { get; set; }
  private float invulnerabilityStart = float.NegativeInfinity;

  private bool activated = false;

  private Camera cam;
  private bool inView;
  private Rect rect;
  private SpriteRenderer sr;

  public bool Activated {
    get => activated; set {
      if (!activated && value)
        OnActivate();
      else if (activated && !value)
        OnDeactivate();
      activated = value;
    }
  }


  public void Start() {
    cam = Camera.main;
    sr = GetComponent<SpriteRenderer>();
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
    if (Activated) {
      EnemyUpdate();
    }
  }

  protected void Updaterect() {
    rect = new Rect(sr.bounds.min.xy(), sr.bounds.size.xy());
  }

  protected Rect GetSpawnRect() {
    var topLeft = cam.ViewportToWorldPoint(Vector2.zero).xy().AddXY(-activeDistanceFromView);
    var bottomRight = cam.ViewportToWorldPoint(Vector2.one).xy().AddXY(activeDistanceFromView);
    Updaterect();
    MyUtil.DrawBoxXY(topLeft, bottomRight - topLeft, Color.green);
    MyUtil.DrawBoxXY(sr.bounds.min.xy(), sr.bounds.size.xy(), Color.cyan);
    return new Rect(topLeft, bottomRight - topLeft);
  }

  public void OnCollisionEnter2D(Collision2D col) {
    if (Time.time > invulnerabilityStart + invulnerabilityDuration && col.gameObject.tag == "PlayerWeapon") {
      // gameObject.GetComponent<Weapon>();
      invulnerabilityStart = Time.time;
      OnHit(1, col);
    }
  }


  // On hit duh
  protected abstract void OnHit(float damage, Collision2D col);
  // When activated
  protected abstract void OnActivate();
  // When deactivated
  protected abstract void OnDeactivate();
  // Just like Update but only called when activated
  protected abstract void EnemyUpdate();
  // When enters visible area
  protected abstract void OnEnterView();
  // When exits visible area
  protected abstract void OnExitView();
}
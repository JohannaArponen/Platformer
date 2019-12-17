﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Physics2DCharEnemyAI))]
public class BasicEnemy : Enemy {


  protected override bool kill { get => _kill; set => _kill = value; }
  protected override float health { get => _health; set => _health = value; }
  protected override float damage { get => _damage; set => _damage = value; }
  protected override float collisionDamage { get => _collisionDamage; set => _collisionDamage = value; }
  protected override float invulnerabilityDuration { get => _invulnerabilityDuration; set => _invulnerabilityDuration = value; }
  protected override float activeDistanceFromView { get => _activeDistanceFromView; set => _activeDistanceFromView = value; }

  private bool _kill = false;
  [SerializeField] private float _health = 5;
  [SerializeField] private float _damage = 1;
  [SerializeField] private float _collisionDamage = 1;
  [SerializeField] private float _invulnerabilityDuration = 1;
  [SerializeField] private float _activeDistanceFromView = 2;
  [SerializeField] private float destroyTime = 3;
  [SerializeField] private float pushMultiplier = 1;

  private Physics2DCharacter physics;
  private Physics2DCharEnemyAI ai;

  // Inherited virtual overridden functions

  override protected void OnCreate() {
    physics = GetComponent<Physics2DCharacter>();
    ai = GetComponent<Physics2DCharEnemyAI>();
  }

  override protected void EnemyUpdate() {
    base.EnemyUpdate();
  }

  override protected void OnHit(float damage, Collider2D col, Weapon weapon) {
    base.OnHit(damage, col, weapon);
    var dist = Vector3.Distance(transform.position, weapon.parent.transform.position);
    physics.velocity += physics.velocity.Add(AngleToVector(weapon.GetDirectionAngle() * Mathf.Deg2Rad) * weapon.GetHitSpeed(dist)) * pushMultiplier;

    Vector2 AngleToVector(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
  }

  override protected void OnKill(float damage, Collider2D col, Weapon weapon) {
    ai.enabled = false;
    var layers = physics.layers;
    layers.layerMask = 0;
    physics.layers = layers;
    Destroy(gameObject, destroyTime);
  }


  override protected void OnExitView() {
    if (!_kill) physics.enabled = ai.enabled = activated = false;
  }

  override protected void OnEnterView() {
    if (!_kill) physics.enabled = ai.enabled = activated = true;
  }

  override protected void OnActivate() {
  }

  override protected void OnDeactivate() {
  }

}

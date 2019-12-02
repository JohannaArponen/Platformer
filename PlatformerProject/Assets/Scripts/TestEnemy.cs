using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy {


  protected override float health { get => _health; set => _health = value; }
  protected override float damage { get => _damage; set => _damage = value; }
  protected override float collisionDamage { get => _collisionDamage; set => _collisionDamage = value; }
  protected override float invulnerabilityDuration { get => _invulnerabilityDuration; set => _invulnerabilityDuration = value; }
  protected override float activeDistanceFromView { get => _activeDistanceFromView; set => _activeDistanceFromView = value; }

  private float _health = 5;
  private float _damage = 1;
  private float _collisionDamage = 1;
  private float _invulnerabilityDuration = 1;
  private float _activeDistanceFromView = 2;

  override protected void EnemyUpdate() {
    print("EnemyUpdate");
  }

  override protected void OnHit(float damage, Collision2D col) {
    print("OnHit");
  }

  override protected void OnExitView() {
    activated = false;
  }

  override protected void OnEnterView() {
    activated = true;
  }

  override protected void OnActivate() {
    print("OnActivate");
  }

  override protected void OnDeactivate() {
    print("OnDeactivate");
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharPhysics2DEnemyAI))]
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

  private CharPhysics2D physics;
  private CharPhysics2DEnemyAI ai;

  override protected void OnCreate() {
    physics = GetComponent<CharPhysics2D>();
    ai = GetComponent<CharPhysics2DEnemyAI>();
  }

  override protected void EnemyUpdate() {
    print("EnemyUpdate");
  }

  override protected void OnHit(float damage, Collision2D col) {
    health -= damage;
    if (health <= 0) {
      gameObject.SetActive(false);
    }
  }

  override protected void OnExitView() {
    physics.enabled = ai.enabled = activated = false;
  }

  override protected void OnEnterView() {
    physics.enabled = ai.enabled = activated = true;
  }

  override protected void OnActivate() {
    print("OnActivate");
  }

  override protected void OnDeactivate() {
    print("OnDeactivate");
  }

}

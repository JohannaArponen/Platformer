using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Physics2DCharEnemyAI))]
public class BasicEnemy : Enemy {

  [SerializeField] new private float _health = 5;
  [SerializeField] private float _damage = 1;
  [SerializeField] private float _collisionDamage = 1;
  [SerializeField] private float _invulnerabilityDuration = 1;
  [SerializeField] private float _destroyDelay = 2;

  protected override float health { get => _health; set => _health = value; }
  protected override float damage { get => _damage; set => _damage = value; }
  protected override float collisionDamage { get => _collisionDamage; set => _collisionDamage = value; }
  protected override float invulnerabilityDuration { get => _invulnerabilityDuration; set => _invulnerabilityDuration = value; }
  protected override float destroyDelay { get => _destroyDelay; set => _destroyDelay = value; }

  [SerializeField] private float pushMultiplier = 1;


  public Animator anim;
  private Physics2DCharacter physics;
  private Physics2DCharEnemyAI ai;


  protected override void OnCreate() {
    physics = GetComponent<Physics2DCharacter>();
    ai = GetComponent<Physics2DCharEnemyAI>();
  }

  protected override void EnemyUpdate() {
  }

  protected override void OnHit(float damage, Collider2D col, Weapon weapon) {
    var dist = Vector3.Distance(transform.position, weapon.parent.transform.position);
    physics.velocity += physics.velocity.Add(AngleToVector(weapon.GetDirectionAngle() * Mathf.Deg2Rad) * weapon.GetHitSpeed(dist)) * pushMultiplier;

    Vector2 AngleToVector(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
  }

  protected override void OnKill(float damage, Collider2D col, Weapon weapon) {
    ai.enabled = false;
    anim.SetFloat("Dead", 0);
  }

  protected override void OnDead() {
    anim.SetFloat("Dead", anim.GetFloat("Dead") + Time.deltaTime / destroyDelay);
  }

  protected override void OnExitView() {
    if (!dead) physics.enabled = ai.enabled = active = false;
  }

  protected override void OnEnterView() {
    if (!dead) physics.enabled = ai.enabled = active = true;
  }
}

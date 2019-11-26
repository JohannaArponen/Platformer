using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyClass {

  override protected void EnemyUpdate() {
    print(":)");
  }

  override protected void OnHit(float damage, Collision2D col) {
    print("OnHit");
  }

  override protected void OnExitView() {
    print("OnExitView");
  }

  override protected void OnEnterView() {
    print("OnEnterView");
  }

  override protected void OnActivate() {
    print("OnActivate");
  }

  override protected void OnDeactivate() {
    print("OnDeactivate");
  }

}

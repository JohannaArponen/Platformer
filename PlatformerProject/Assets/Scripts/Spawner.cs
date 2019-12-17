using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Spawner : MonoBehaviour {

  [PositiveValueOnly]
  [Tooltip("Amount of spawns per StartSpawning call")]
  public int spawnCount = 10;

  [PositiveValueOnly]
  [Tooltip("Delay between spawns")]
  public float spawnDelay = 0.5f;

  [PositiveValueOnly]
  [Tooltip("Maximum spawns spawned by this spawner")]
  public int maxSpawns = 10;

  [MustBeAssigned]
  [Tooltip("Spawn of hell")]
  public GameObject spawn;

  private GameObject container;
  private bool spawning = false;
  private int spawningCount = 0;
  private int totalSpawned = 0;
  private float prevSpawnTime = float.NegativeInfinity;


  // Start is called before the first frame update
  void Start() {
    container = new GameObject(gameObject.name + " SpawnContainer");
    container.transform.position = transform.position;
    container.transform.parent = transform;
  }

  // Update is called once per frame
  void Update() {
    if (totalSpawned < maxSpawns) {
      while (spawning && prevSpawnTime <= Time.time - spawnDelay)
        Spawn();
    }
  }

  public List<GameObject> GetSpawn() {
    var children = new List<GameObject>();
    foreach (Transform child in container.transform) {
      children.Add(child.gameObject);
    }
    return children;
  }

  [ButtonMethod]
  public void StartSpawning() => StartSpawning(spawnCount);
  public void StartSpawning(int count) {
    spawning = true;
    spawningCount += count;
  }

  void Spawn() {
    prevSpawnTime = Time.time;
    var inst = Instantiate(spawn, transform.position, Quaternion.identity, container.transform);
    totalSpawned++;
  }
}

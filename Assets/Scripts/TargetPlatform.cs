using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlatform : MonoBehaviour
{
    public GameObject spawnArea;

    private void Start() {
        ChangeLocation();
    }

    private Vector2 GetRandomLocation() {
        Vector2 area = (spawnArea.transform.localScale - this.transform.localScale) / 2;
        return spawnArea.transform.position + new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0);
    }

    public void ChangeLocation() {
        this.transform.position = GetRandomLocation();
    }
}

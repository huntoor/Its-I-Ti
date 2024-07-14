using UnityEngine;

class PositionClamper: MonoBehaviour {
  public int mapWidth;
  public int mapHeight;

  void LateUpdate() {
    Vector3 pos = transform.position;

    // assuming map starts at (0, 0)
    pos.x = Mathf.Max(Mathf.Min(pos.x, mapWidth), -40);
    pos.y = Mathf.Max(Mathf.Min(pos.y, mapHeight), -20);

    // setting the transform position. Consider using local position when possible
    transform.position = pos;
  }
}

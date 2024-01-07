using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region : MonoBehaviour
{
    public static Region instance { get; private set; }

    [Header("Region Settings")]
    [SerializeField] Vector2Int size;

    [Header("Tile layers")]
    [SerializeField] Tilemap terrainMap;
    [SerializeField] Tilemap groundFXMap;
    [SerializeField] Tilemap overgroundMap;
    [SerializeField] Tilemap unitMap;
    [SerializeField] Tilemap weatherFXMap;
    [SerializeField] Tilemap hightlightMap;

    Dictionary<Vector3Int, Unit> units;

    private void Awake()
    {
        instance = this;
        units = new Dictionary<Vector3Int, Unit>();
    }

    private bool ValidateMaps()
    {
        if (terrainMap && groundFXMap && overgroundMap && unitMap && weatherFXMap && hightlightMap)
        {
            return true;
        }
        return false;
    }

    private void Start()
    {
        if (!ValidateMaps()) { Debug.LogWarning("Missing tilemap");  return; }
    }

    public bool inBound(Vector3Int position)
    {
        return position.x >= 0 && position.x < size.x && position.y >= 0 && position.y < size.y;
    }
    public bool inBound(Vector3 position)
    {
        return inBound(Vector3Int.FloorToInt(position));
    }

    public bool isSpawnable(Vector3Int position)
    {
        return true;
    }

    public void SpawnUnit(Vector3Int position,string name, ElementType type, int level, int teamID)
    {
        if (!isSpawnable(position)) { return; }

        Unit unit = new Unit(name, type, level, teamID);

        PutUnit(unit, position);
    }

    public bool PutUnit(Unit unit, Vector3Int position)
    {
        if (units[position] != null) {  return false; }

        
        units[position] = unit;
        return true;
    }

    public Unit TakeUnit(Vector3Int position)
    {
        if (units[position] == null) { return null; }

        Unit unit = units[position];

        units.Remove(position);
        unitMap.SetTile(position, null);

        return unit;
    }

    #region Gizmos

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(Vector3.zero, Vector3.right * size.x);
        Gizmos.DrawRay(Vector3.up * size.y, Vector3.right * size.x);
        Gizmos.DrawRay(Vector3.zero, Vector3.up * size.y);
        Gizmos.DrawRay(Vector3.right * size.x, Vector3.up * size.y);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Region : MonoBehaviour
{
    public static Region instance { get; private set; }

    [SerializeField] LayerMask UnitLayer;

    private Tilemap unitMap;

    Dictionary<Vector3Int, Unit> units;

    private void Awake()
    {
        instance = this;
        units = new Dictionary<Vector3Int, Unit>();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.layer == UnitLayer.value)
            {
                unitMap = transform.GetChild(i).GetComponent<Tilemap>();
                Debug.Log("Obtained unit map");
            }
        }
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
}

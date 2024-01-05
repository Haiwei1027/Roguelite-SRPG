using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{

    string name;
    ElementType type;
    int level;
    int teamID;

    public Unit(string name, ElementType type, int level, int teamID)
    {
        this.name = name;
        this.type = type;
        this.level = level;
        this.teamID = teamID;
    }

    
}

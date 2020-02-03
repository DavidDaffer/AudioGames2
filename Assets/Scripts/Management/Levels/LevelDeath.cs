using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDeath : Level
{
    public float dimFactor = 5f;
    public Light[] lightsOut;
    public override void LevelCleanUp()
    {
        foreach (Light l in lightsOut)
        {
            l.intensity *= dimFactor;
        }
    }

    public override void LevelInit()
    {
        foreach(Light l in lightsOut)
        {
            l.intensity /= 5;
        }
    }

    public override void LevelUpdate()
    {
        
    }
}

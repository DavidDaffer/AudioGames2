using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level : MonoBehaviour
{
    public int levelNumber = 0;
    public bool playerCanDie = false;
    public float timeForFinishingLevel = 120f;
    public bool lockDock = false;

    public abstract void LevelUpdate();
    public abstract void LevelInit();

    public abstract void LevelCleanUp();
}

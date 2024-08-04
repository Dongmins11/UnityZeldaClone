using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        Title,
        Loading,
        Test,
        Game,
        Dungeon_1,
        Dungeon_2,
        Castle,
        Boss,
    }

    public enum TimeType
    {
        Normal,
        Fixed,
        Unscale,
        End,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }


    public enum MouseEvent
    {
        Press,
        Click
    }
}

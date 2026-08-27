using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EOptionType
{
    LearnSkill,
    UpgradeStat
}

public enum EStatType
{
    None,
    MaxHP,
    Speed
}

[Serializable]
public class LevelUpOption
{
    public string optionName;

    [TextArea]
    public string description;
    public Sprite icon;
    public EOptionType type;
    public SkillID skillId;
    public EStatType statType;
    public float statAmount;
}

using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public Sprite skillIcon;
    public int level = 1;

    public void LevelUp()
    {
        level++;
    }
}

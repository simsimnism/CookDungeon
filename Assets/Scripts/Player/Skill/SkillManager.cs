using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public AppleJuiceSkill appleJuiceSkill;
    public PizzaSpinCutter pizzaSpinCutter;
    public EggCabbageSpirit eggCabbageSpirit;
    public CandyAppleRoll candyAppleRoll;

    public float appleJuiceCooldown = 5f;
    public float pizzaSpinCooldown = 8f;
    public float eggCabbageCooldown = 10f;
    public float candyAppleCooldown = 12f;

    private float appleJuiceTimer;
    private float pizzaSpinTimer;
    private float eggCabbageTimer;
    private float candyAppleTimer;

    void Start()
    {
        if (appleJuiceSkill != null) appleJuiceSkill.enabled = false;
        if (pizzaSpinCutter != null) pizzaSpinCutter.enabled = false;
        if (eggCabbageSpirit != null) eggCabbageSpirit.enabled = false;
        if (candyAppleRoll != null) candyAppleRoll.enabled = false;
    }

    void Update()
    {
        if (appleJuiceSkill != null && appleJuiceSkill.enabled) appleJuiceTimer += Time.deltaTime;
        if (pizzaSpinCutter != null && pizzaSpinCutter.enabled) pizzaSpinTimer += Time.deltaTime;
        if (eggCabbageSpirit != null && eggCabbageSpirit.enabled) eggCabbageTimer += Time.deltaTime;
        if (candyAppleRoll != null && candyAppleRoll.enabled) candyAppleTimer += Time.deltaTime;

        if (appleJuiceSkill != null && appleJuiceSkill.enabled && appleJuiceTimer >= appleJuiceCooldown)
        {
            appleJuiceSkill.ActivateSkill();
            appleJuiceTimer = 0f;
        }

        if (pizzaSpinCutter != null && pizzaSpinCutter.enabled && pizzaSpinTimer >= pizzaSpinCooldown)
        {
            pizzaSpinCutter.ActivateSkill();
            pizzaSpinTimer = 0f;
        }

        if (eggCabbageSpirit != null && eggCabbageSpirit.enabled && eggCabbageTimer >= eggCabbageCooldown)
        {
            eggCabbageSpirit.ActivateSkill();
            eggCabbageTimer = 0f;
        }

        if (candyAppleRoll != null && candyAppleRoll.enabled && candyAppleTimer >= candyAppleCooldown)
        {
            candyAppleRoll.ActivateSkill();
            candyAppleTimer = 0f;
        }
    }

    public void ActivateSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1:
                if (appleJuiceSkill != null) appleJuiceSkill.enabled = true;
                break;
            case 2:
                if (pizzaSpinCutter != null) pizzaSpinCutter.enabled = true;
                break;
            case 3:
                if (eggCabbageSpirit != null) eggCabbageSpirit.enabled = true;
                break;
            case 4:
                if (candyAppleRoll != null) candyAppleRoll.enabled = true;
                break;
        }
    }

    public int GetSkillLevel(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: return appleJuiceSkill != null ? appleJuiceSkill.Level : 0;
            case 2: return pizzaSpinCutter != null ? pizzaSpinCutter.Level : 0;
            case 3: return eggCabbageSpirit != null ? eggCabbageSpirit.Level : 0;
            case 4: return candyAppleRoll != null ? candyAppleRoll.Level : 0;
            default: return 0;
        }
    }

    public void LevelUpSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: if (appleJuiceSkill != null) appleJuiceSkill.LevelUp(); break;
            case 2: if (pizzaSpinCutter != null) pizzaSpinCutter.LevelUp(); break;
            case 3: if (eggCabbageSpirit != null) eggCabbageSpirit.LevelUp(); break;
            case 4: if (candyAppleRoll != null) candyAppleRoll.LevelUp(); break;
        }
    }

    public void ReduceCooldown(int skillIndex, float reductionAmount)
    {
        switch (skillIndex)
        {
            case 1: appleJuiceCooldown = Mathf.Max(1f, appleJuiceCooldown - reductionAmount); break;
            case 2: pizzaSpinCooldown = Mathf.Max(1f, pizzaSpinCooldown - reductionAmount); break;
            case 3: eggCabbageCooldown = Mathf.Max(1f, eggCabbageCooldown - reductionAmount); break;
            case 4: candyAppleCooldown = Mathf.Max(1f, candyAppleCooldown - reductionAmount); break;
        }
    }
}

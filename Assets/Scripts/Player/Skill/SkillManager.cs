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


    void Update()
    {
        appleJuiceTimer += Time.deltaTime;
        pizzaSpinTimer += Time.deltaTime;
        eggCabbageTimer += Time.deltaTime;
        candyAppleTimer += Time.deltaTime;

        if (appleJuiceTimer >= appleJuiceCooldown)
        {
            appleJuiceSkill.ActivateSkill();
            appleJuiceTimer = 0f;
        }

        if (pizzaSpinTimer >= pizzaSpinCooldown)
        {
            pizzaSpinCutter.ActivateSkill();
            pizzaSpinTimer = 0f;
        }

        if (eggCabbageTimer >= eggCabbageCooldown)
        {
            eggCabbageSpirit.ActivateSkill();
            eggCabbageTimer = 0f;
        }

        if (candyAppleTimer >= candyAppleCooldown)
        {
            candyAppleRoll.ActivateSkill();
            candyAppleTimer = 0f;
        }
    }

    public void LevelUpSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 1: appleJuiceSkill.LevelUp(); break;
            case 2: pizzaSpinCutter.LevelUp(); break;
            case 3: eggCabbageSpirit.LevelUp(); break;
            case 4: candyAppleRoll.LevelUp(); break;
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


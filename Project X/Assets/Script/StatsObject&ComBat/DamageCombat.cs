using UnityEngine;

public class PhysicalDamage : IDamageType
{
    public void ApplyDamage(BaseStats target, int amount)
    {
        target.ApplyPhysicalDamage(amount);
    }
}
public class MagicalDamage : IDamageType
{
    public void ApplyDamage(BaseStats target, int amount)
    {
        target.ApplyMagicalDamage(amount);
    }
}
public class MixedDamage : IDamageType
{
    private float physicalRate;
    public MixedDamage(float physicalRate)
    {
        this.physicalRate = Mathf.Clamp01(physicalRate);
    }
    public void ApplyDamage(BaseStats target, int amount)
    {
        this.physicalRate = Mathf.Clamp01(physicalRate);
        int physicalAmount = Mathf.RoundToInt(amount * physicalRate);
        int MagicalAmount = amount - physicalAmount;
        target.ApplyPhysicalDamage(physicalAmount);
        target.ApplyMagicalDamage(MagicalAmount);
    }
}
public class TrueDamage : IDamageType
{
    public void ApplyDamage(BaseStats target, int amount)
    {
        target.ApplyTrueDamage(amount);
    }
}

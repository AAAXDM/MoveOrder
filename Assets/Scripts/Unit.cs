using System;
using System.Reflection;

public class Unit
{
    private Enums.ArmyType _armyType;
    private int _numberInArmy;
    private int _initiative;
    private int _speed;

    public Enums.ArmyType ArmyType => _armyType;
    public int Initiative => _initiative;
    public int Speed => _speed;
    public int NumberInArmy => _numberInArmy;

    public Unit(int initiative, int speed)
    {
        _initiative = initiative;
        _speed = speed;
    }

    public void QueueUp(int numberInArmy, Enums.ArmyType armyType)
    {
        _numberInArmy = numberInArmy;
        _armyType = armyType;
    }

    public bool Compare(Unit unit)
    {
        Type thisType = this.GetType();
        Type unitType = unit.GetType();
        PropertyInfo[] properties =  unitType.GetProperties();
        PropertyInfo[] thisProperties = thisType.GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if(thisProperties[i] != properties[i])
            {
                return false;
            }
        }
        return true;
    }
}

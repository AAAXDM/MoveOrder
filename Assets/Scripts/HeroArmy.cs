using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HeroArmy : MonoBehaviour
{
    [SerializeField]private int _maxArmyValue;
    private List<Unit> _units = new List<Unit>();
    private Enums.ArmyType _armyType;

    public List<Unit> Units => _units;
    public Enums.ArmyType ArmyType => _armyType;

    public void HireAUnit(Unit unit)
    {
        if (_units.Count < _maxArmyValue)
        {
            unit.QueueUp(_units.Count, _armyType);
            _units.Add(unit);
        }
        else
        {
            Debug.Log("You have full army stack!");
        }
    }

    public void DeleteUnit(Unit unit)
    {
        Unit unitToDelete =  _units.Find(_unit => _unit.Compare(unit));
        _units.Remove(unitToDelete);
    }

    public void AddType(Enums.ArmyType armyType)
    {
        _armyType = armyType;
    }
}

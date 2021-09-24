using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnOfMovesSimulator : MonoBehaviour
{
    [SerializeField]private int _stepCount;
    [SerializeField] private ArmyInfo[] _redArmy;
    [SerializeField] private ArmyInfo[] _blueArmy;
    private HeroArmy _attackArmy;
    private HeroArmy _defenceArmy;
    private List<Unit> _allUnits = new List<Unit>();
    private List<Unit> _steps = new List<Unit>();
    private StepCountUI _stepCountUI;
    private UnitQueueUI _queueUI;
    private int _allRoundSteps;
    private int _startStep = 0;

    public int StepCount => _stepCount;
    public int StartStep => _startStep;
    public int AllRoundSteps => _allRoundSteps;
    public List<Unit> Steps => _steps;

    void Start()
    {
        _stepCountUI = FindObjectOfType<StepCountUI>();
        _queueUI = FindObjectOfType<UnitQueueUI>();
        HeroArmy[] armies = FindObjectsOfType<HeroArmy>();
        _attackArmy = armies[0];
        _defenceArmy = armies[1];
        _allRoundSteps = _redArmy.Length + _blueArmy.Length;
        AddArmy(_attackArmy, _redArmy, Enums.ArmyType.Attack);
        AddArmy(_defenceArmy, _blueArmy, Enums.ArmyType.Defence);
        AddUnitsToList();
        CalulateAllMoves();
        _stepCountUI.WriteText();
        _queueUI.WriteText();
    }

    void AddArmy(HeroArmy heroArmy, ArmyInfo[] armyInfo, Enums.ArmyType armyType)
    {
        heroArmy.AddType(armyType);
        for(int i = 0; i < armyInfo.Length; i++)
        {
            Unit unit = new Unit(armyInfo[i].initiative, armyInfo[i].speed);
            heroArmy.HireAUnit(unit);
        }
    }

    void AddUnitsToList()
    {
        _allUnits = _attackArmy.Units.Concat(_defenceArmy.Units).ToList();

    }

    void CalulateAllMoves()
    {
        int startStep = _startStep;
        _steps.Clear();
        while(_steps.Count < _stepCount)
        {
            
            List<Unit> step = CalculateMoves(startStep);
            _steps = _steps.Concat(step).ToList();
            startStep = 0; 
        }
    }
    List<Unit> CalculateMoves(int startStep)
    {
        List<Unit> thisRound;
        List<Unit> nextRound;
        int honestSkip;
        int oddSkip;
        int honesTake;
        int oddTake;
       
        honestSkip = startStep;
        oddSkip = 0;
        honesTake = _allRoundSteps - startStep;
        thisRound = CalculateHonestRoundMoves(honestSkip, honesTake);
        oddTake =_stepCount - thisRound.Count;
        nextRound = CalculateOddRoundMoves(oddSkip,oddTake);      
        return thisRound.Concat(nextRound).ToList();
    }

    List<Unit> CalculateHonestRoundMoves(int skip, int take)
    {
        return _allUnits.OrderByDescending(unit => unit.Initiative)
                        .ThenByDescending(unit => unit.Speed)
                        .ThenBy(unit => unit.ArmyType)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
    }

    List<Unit> CalculateOddRoundMoves(int skip, int take)
    {
        return _allUnits.OrderByDescending(unit => unit.Initiative)
                                   .ThenByDescending(unit => unit.Speed)
                                   .ThenByDescending(unit => unit.ArmyType)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToList();
    }
    public void SkipTurn()
    {
        _startStep++;
        if (_startStep == _allRoundSteps)
        {
            _startStep = 0;        
        }
        CalulateAllMoves();
        _stepCountUI.WriteText();
        _queueUI.WriteText();
    }

    public void KillUnit()
    {
        Enums.ArmyType type = _steps.First().ArmyType;
        Unit unitToDestroy = _steps.Where(unit => unit.ArmyType != type).First();
        _allUnits.Remove(unitToDestroy);
        _allRoundSteps--;
        if (type == Enums.ArmyType.Attack) _attackArmy.DeleteUnit(unitToDestroy);
        else _defenceArmy.DeleteUnit(unitToDestroy);
        SkipTurn();
    }
}

[System.Serializable]
public class ArmyInfo
{
    public int initiative;
    public int speed;
}


using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class UnitQueueUI : MonoBehaviour
{
    List<Image> _images = new List<Image>();
    TurnOfMovesSimulator _movesSimulator;
    void Start()
    {
        _images = GetComponentsInChildren<Image>().Where(image => image.gameObject != gameObject).ToList();
        _movesSimulator = FindObjectOfType<TurnOfMovesSimulator>();
    }
    public void WriteText()
    {
        List<Unit> units = _movesSimulator.Steps;
        int roundSteps = _movesSimulator.AllRoundSteps - _movesSimulator.StartStep;
        int factor = 0;
        for (int i = 0; i < _images.Count; i++)
        {
            Text imageText = _images[i].GetComponentInChildren<Text>();
            if (imageText == null)
            {
                Debug.LogError("No text in image" + i);
            }
            if (i < roundSteps)
            {
                StringBuilder stringBuilder = new StringBuilder("Unit ");
                stringBuilder.Append(units[i].NumberInArmy.ToString() + ":");
                stringBuilder.Append("Initiative - " + units[i].Initiative.ToString());
                stringBuilder.Append(" Speed - " + units[i].Speed.ToString());
                imageText.text = stringBuilder.ToString();
                _images[i].color = ChangeColor(units[i]);
            }
            else if (i == roundSteps + (_movesSimulator.AllRoundSteps + 1) * factor)
            {
                imageText.text = "Next Round";
                factor++;
                _images[i].color = Color.green;
            }
            else
            {
                int coefficient = i - factor;
                StringBuilder stringBuilder = new StringBuilder("Unit ");
                stringBuilder.Append(units[coefficient].NumberInArmy.ToString() + ":");
                stringBuilder.Append("Initiative - " + units[coefficient].Initiative.ToString());
                stringBuilder.Append(" Speed - " + units[coefficient].Speed.ToString());
                imageText.text = stringBuilder.ToString();
                _images[i].color = ChangeColor(units[coefficient]);
            }
        }
    }

    Color ChangeColor(Unit unit)
    {
        Color color;
        if (unit.ArmyType == Enums.ArmyType.Attack) color = Color.red;
        else color = Color.blue;
        return color;
    }
}

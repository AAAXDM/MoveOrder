using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class StepCountUI : MonoBehaviour
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
        int coefficient = 0;
        int roundSteps = _movesSimulator.AllRoundSteps - _movesSimulator.StartStep;
        int number = _movesSimulator.StartStep + 1;
        int step = 1;
        for (int i = 0; i < _images.Count; i++)
        {
            Text imageText = _images[i].GetComponentInChildren<Text>();
            if (imageText == null)
            {
                Debug.LogError("No text in image" + i);
            }
            if (i < roundSteps)
            {
                imageText.text = (number + i).ToString();
            }
            else if (i == roundSteps + (_movesSimulator.AllRoundSteps + 1)*coefficient)
            {
                imageText.text = "";
                coefficient++;
                step = 1;
            }
            else
            {
                imageText.text = (step).ToString();
                step++;
            }
        }
    }
}

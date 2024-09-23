using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControls : MonoBehaviour
{
    [SerializeField]
    private FoosballManager foosballManager;
    public Button[] buttons;

    void Start()
    {
        buttons[0].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(-1, -1),true));
        buttons[1].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(0, -1), true));
        buttons[2].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(1, -1), true));

        buttons[3].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(-1, 0), true));
        buttons[5].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(1, 0), true));

        buttons[6].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(-1, 1), true));
        buttons[7].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(0, 1), true));
        buttons[8].onClick.AddListener(() => foosballManager.PegPlacement(new Vector2(1, 1), true));
    }
}

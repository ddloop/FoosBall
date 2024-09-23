using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoosballManager : MonoBehaviour
{
    [SerializeField]
    private FieldRenderer fieldRenderer;

    bool playerTurn;
    Vector2 ballPosition;

    List<PegConnection> connections = new List<PegConnection>();

    public void RestartGame() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Start()
    {
        SetupField();

        //Initiate Game        
        ballPosition = new Vector2(4, 6);
        playerTurn = true;
    }

    void SetupField() 
    {
        for (int i = 0; i < 10; i++) 
        {
            AddConnection(new Vector2(0, 1+i), new Vector2(0, 2+i));
        }

        for (int i = 0; i < 10; i++)
        {
            AddConnection(new Vector2(8, 1+i), new Vector2(8, 2+i));
        }

        for (int i = 0; i < 3; i++)
        {
            AddConnection(new Vector2(0+i, 1), new Vector2(1+i, 1));
        }

        for (int i = 0; i < 3; i++)
        {
            AddConnection(new Vector2(5+i, 1), new Vector2(6+i, 0));
        }

        AddConnection(new Vector2(3, 1), new Vector2(3 , 0));
        AddConnection(new Vector2(5, 1), new Vector2(5 , 0));
        AddConnection(new Vector2(3, 10), new Vector2(3, 11));
        AddConnection(new Vector2(5, 10), new Vector2(5, 11));
    }

    public void PegPlacement(Vector2 _direction, bool player)
    {
        if (!CheckPossibleMove(_direction))
            return;

        Color pegColor;
        var newBallPosition = ballPosition + _direction;

        if (player)
            pegColor = Color.green;
        else
            pegColor = Color.red;

        fieldRenderer.PlacePeg(ballPosition, newBallPosition, pegColor);

        if (!CheckFinishedOnPeg(newBallPosition))
            playerTurn = !player;

        AddConnection(ballPosition, newBallPosition);

        ballPosition = newBallPosition;

        if (!RemisCheck())
        {
            Debug.Log("Remis");
            fieldRenderer.GameStatus("Remis");
            playerTurn = false;
            return;
        }

        if (BallInGoal())
        {
            
        }

        if (!playerTurn)
            AIMakeMove();
    }

    private void AIMakeMove()
    {
        bool TryMove(Vector2 test) 
        {
            if (CheckPossibleMove(test))
            {
                PegPlacement(test, false);
                return true;
            }
            return false;
        }

        //Try go down
        Vector2 testDirection = Vector2.up;
        if (TryMove(testDirection)) return;

        //If Not go either bottom left or right down
        testDirection = new Vector2(1, UnityEngine.Random.Range(0,1));
        if (TryMove(testDirection)) return;

        //Sides
        testDirection = new Vector2(0, UnityEngine.Random.Range(0, 2) * 2 - 1);
        if (TryMove(testDirection)) return;

        //Random up
        testDirection = new Vector2(-1, (int)UnityEngine.Random.Range(-1, 1));
        if (TryMove(testDirection)) return;
    }

    bool RemisCheck() 
    {
        var returnValue = false;

        if (CheckPossibleMove(-Vector2.up + -Vector2.right))
            returnValue = true;
        if (CheckPossibleMove(-Vector2.up))
            returnValue = true;
        if (CheckPossibleMove(-Vector2.up + Vector2.right))
            returnValue = true;
        if (CheckPossibleMove( -Vector2.right))
            returnValue = true;
        if (CheckPossibleMove( Vector2.right))
            returnValue = true;
        if (CheckPossibleMove( Vector2.up + -Vector2.right))
            returnValue = true;
        if (CheckPossibleMove( Vector2.up))
            returnValue = true;
        if (CheckPossibleMove( Vector2.up + Vector2.right))
            returnValue = true;

        return returnValue;
    }

    bool BallInGoal() 
    {
        if (ballPosition.y < 1)
        {
            fieldRenderer.GameStatus("Wygrana");
            PlayerPrefs.SetInt("PlayerWins", PlayerPrefs.GetInt("PlayerWins", 0) + 1);
            return true;
        }
        if (ballPosition.y > 11)
        {
            fieldRenderer.GameStatus("Przegrana");
            PlayerPrefs.SetInt("PlayerLosses", PlayerPrefs.GetInt("PlayerLosses", 0) + 1);
            return true;
        }

        return false;
    }

    bool CheckFinishedOnPeg(Vector2 newBallPosition)
    {
        foreach (var connection in connections)
        {
            if (connection.PegOnePosition == newBallPosition || connection.PegTwoPosition == newBallPosition)
                return true;
        }

        return false;
    }

    bool CheckPossibleMove(Vector2 _direction)
    {
        //Boundary Test
        var testVector = ballPosition + _direction;

        if (testVector.x < 0 || testVector.x > 8)
            return false;

        if ((testVector.y < 1 || testVector.y > 11) && (testVector.x <= 3 || testVector.x >= 5))
        {
            if(ballPosition == new Vector2(4, 1) || ballPosition == new Vector2(4, 11))
                return true;
            else
                return false;
        }

        //Made Connection Test
        if (CheckConnection(ballPosition, testVector))
        {
            //Debug.Log("Illegal Move");
            return false;
        }

        return true;
    }

    void AddConnection(Vector2 peg1, Vector2 peg2)
    {
        var newConnection = new PegConnection
        {
            PegOnePosition = peg1,
            PegTwoPosition = peg2
        };

        connections.Add(newConnection);
    }

    bool CheckConnection(Vector2 ball, Vector2 newPosition) 
    {
        foreach (var connection in connections)
        {
            if (connection.PegOnePosition == ball && connection.PegTwoPosition == newPosition)
                return true;

            if (connection.PegOnePosition == newPosition && connection.PegTwoPosition == ball)
                return true;
        }

        return false;
    }

    struct PegConnection 
    {
        public Vector2 PegOnePosition;
        public Vector2 PegTwoPosition;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldRenderer : MonoBehaviour
{
    [SerializeField]
    GameObject pegPrefab;
    [SerializeField]
    GameObject soccerball;
    [SerializeField]
    GameObject text;

    private void Start()
    {
        //PlacePeg(new Vector2(4, 6), new Vector2(5, 6), Color.red);
        //PlacePeg(new Vector2(5, 6), new Vector2(5, 7), Color.green);
        //PlacePeg(new Vector2(5, 7), new Vector2(4, 8), Color.red);
    }

    public void PlacePeg(Vector2 _oldPosition,Vector2 _newPosition,Color _color)
    {
        //Angle
        Vector2 direction = _oldPosition - _newPosition;
        float angle = Mathf.Atan2(-direction.y,direction.x) * Mathf.Rad2Deg;

        Vector3 translatedPosition = new Vector3(_newPosition.x, _newPosition.y * -1);

        var peg = Instantiate(pegPrefab);
        peg.transform.SetParent(transform);
        peg.transform.localScale = Vector3.one;
        peg.transform.position = transform.position + translatedPosition * 60.0f * transform.localScale.x;
        peg.transform.position += new Vector3(2,-2,0);
        peg.transform.eulerAngles = new Vector3( 0, 0, angle);

        if (angle % 90 != 0)
            peg.GetComponent<RectTransform>().sizeDelta = new Vector2(85f,4f);

        peg.GetComponent<Image>().color = _color;

        //Move ball indicator
        MoveBall(peg.transform.position);
    }

    private void MoveBall(Vector3 _position) 
    {
        soccerball.transform.position = _position;
    }

    internal void GameStatus(string status) 
    {
       text.SetActive(true);
       text.GetComponent<TextMeshProUGUI>().text = status;
    }

}

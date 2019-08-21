using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipManagger _shipManagger;

    [SerializeField]
    float _speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _shipManagger = FindObjectOfType<ShipManagger>();
    }

    // Update is called once per frame
    void Update()
    {
        var okDistance = _speed * 1.5f * (1f / 18f);
        var ship = _shipManagger.FindClosestShip(this, okDistance);
        if (ship != this)
        {
            var direction = (ship.Pos - Pos);

            transform.Translate(direction.normalized * _speed * Time.deltaTime);
        }
    }


    public Vector2 Pos
    {
        get
        {
            return transform.position;
        }
    }


}

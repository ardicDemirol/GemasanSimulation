using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class InfoController : MonoBehaviour
{
    [Range(1, 20)] [SerializeField] private float minute;

    private float vehicleEulerAngleY;

    private uint _second;
    private readonly byte _zero = 0;
    private readonly byte _one = 1;
    private readonly float _waitForDephtSeconds = 0.05f;
    private readonly float _waitForDirectionSeconds = 0.5f;

    readonly Dictionary<CompassDirection, Tuple<int, int>> directionAngles = new()
    {
        {CompassDirection.E, Tuple.Create(0, 45)},
        {CompassDirection.NE, Tuple.Create(45, 90)},
        {CompassDirection.N, Tuple.Create(90, 135)},
        {CompassDirection.NW, Tuple.Create(135, 180)},
        {CompassDirection.W, Tuple.Create(180, 225)},
        {CompassDirection.SW, Tuple.Create(225, 270)},
        {CompassDirection.S, Tuple.Create(270, 315)},
        {CompassDirection.SE, Tuple.Create(315, 360)}
    };

    private void Awake()
    {
        _second = (uint)minute * 60;
        StartCoroutine(SetTimer());
        StartCoroutine(SetVehicleDirection());
        StartCoroutine(SetDepht());
    }
    private IEnumerator SetVehicleDirection()
    {
        while (true)
        {
            vehicleEulerAngleY = ManuelController.Instance.transform.eulerAngles.y;
            foreach (var directionAngle in directionAngles)
            {
                if (vehicleEulerAngleY > directionAngle.Value.Item1 && vehicleEulerAngleY < directionAngle.Value.Item2)
                {
                    UISignals.Instance.OnCompassInfoChanged?.Invoke(directionAngle.Key);
                    break;
                }
            }

            yield return Extensions.GetWaitForSeconds(_waitForDirectionSeconds);
        }
    }

    private IEnumerator SetDepht()
    {
        while (true)
        {
            var vehiclePosition = ManuelController.Instance.transform.position.y;
            UISignals.Instance.OnDephtInfoChanged?.Invoke(Extensions.WriteNDigit(vehiclePosition,2));

            yield return Extensions.GetWaitForSeconds(_waitForDephtSeconds);
        }
      
    }

    private IEnumerator SetTimer()
    {
        while (_second > _zero)
        {
            yield return Extensions.GetWaitForSeconds(_one);
            UISignals.Instance.OnTimerInfoChanged?.Invoke(--_second);
        }
    }


}

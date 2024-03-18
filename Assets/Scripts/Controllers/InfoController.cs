using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class InfoController : MonoBehaviour
{
    private float vehicleEulerAngleY;
    private static readonly WaitForSeconds _waitForDirectionSeconds = new(0.5f);
    private static readonly WaitForSeconds _waitForDephtSeconds = new(0.05f);

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

            yield return _waitForDirectionSeconds;
        }
    }

    private IEnumerator SetDepht()
    {
        while (true)
        {
            var vehiclePosition = ManuelController.Instance.transform.position.y;
            UISignals.Instance.OnDephtInfoChanged?.Invoke(Extensions.WriteNDigit(vehiclePosition,2));

            yield return _waitForDephtSeconds;
        }
      
    }



}

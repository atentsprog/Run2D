using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAbility : MonoBehaviour
{
    Dictionary<Transform, float> items = new Dictionary<Transform, float>(); //<코인, 가속도>
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (items.ContainsKey(collision.transform))
            return;

        if (collision.GetComponent<CoinItem>() == null)
            return;

        items.Add(collision.transform, 0);
    }

    public float acceleration = 1;
    private void Update()
    {
        foreach (var item in items)
        {
            var coinTr = item.Key;
            var acc = item.Value;
            acc += acceleration * Time.deltaTime;
            Vector3 dir = (transform.position - coinTr.position).normalized;
            coinTr.position = coinTr.position + (dir * acc * Time.deltaTime);
        }
    }
}

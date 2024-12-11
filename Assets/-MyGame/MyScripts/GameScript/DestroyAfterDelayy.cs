using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelayy : MonoBehaviour
{
    [SerializeField] float _delay = 1f;
    private void OnEnable()
    {
        Destroy(gameObject, _delay);
    }
}

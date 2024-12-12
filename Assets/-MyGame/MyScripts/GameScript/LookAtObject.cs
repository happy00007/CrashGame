using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform _targetObj;
    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void FixedUpdate()
    {
        transform.LookAt(_targetObj, Vector3.left);
        Vector3 targetPosition = _targetObj.position;
        Vector3 direction = targetPosition - rectTransform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}

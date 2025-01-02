using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Anim2D : MonoBehaviour
{
    [SerializeField] Sprite[] _imgSprites;
    [SerializeField] float _frameDelayTime = 0.05f;
    Image _currentObj;
    void Start()
    {
        _currentObj = GetComponent<Image>();
        StartCoroutine(StartAnimate());
    }

    int _currentFrame = 0;
    IEnumerator StartAnimate()
    {
        while (true)
        {
            if (_currentFrame >= _imgSprites.Length)
                _currentFrame = 0;
            _currentObj.sprite = _imgSprites[_currentFrame];
            _currentFrame++;
            yield return new WaitForSeconds(_frameDelayTime);
        }
    }
}

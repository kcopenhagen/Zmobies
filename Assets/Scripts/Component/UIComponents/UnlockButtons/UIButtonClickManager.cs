using System;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonClickManager : MonoBehaviour
{
    public Action OnClick;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClick?.Invoke());
    }



}

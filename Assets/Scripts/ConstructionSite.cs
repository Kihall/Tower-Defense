using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConstructionSite : MonoBehaviour
{
    public event EventHandler OnButtonClicked;

    [SerializeField] private Button constructionSiteButton;


    private void Awake()
    {
        constructionSiteButton.onClick.AddListener(() => {
            OnButtonClicked?.Invoke(this, EventArgs.Empty);
        });
    }
}

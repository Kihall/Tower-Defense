using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerTypeSelectUI : MonoBehaviour
{
    [SerializeField] private ConstructionSite constructionSite;
    [SerializeField] private Transform towerTypeButtonPrefab;
    [SerializeField] private Transform towerTypeButtonContainerTransform;

    private TowerTypeListSO towerTypeList;


    private void Awake()
    {
        towerTypeList = Resources.Load<TowerTypeListSO>(typeof(TowerTypeListSO).Name);
    }

    private void Start()
    {
        constructionSite.OnButtonClicked += ConstructionSite_OnButtonClicked;

        CreateTowerButtons();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    private void ConstructionSite_OnButtonClicked(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void CreateTowerButtons()
    {
        foreach (Transform buttonTransform in towerTypeButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        foreach (TowerTypeSO towerType in towerTypeList.list)
        {
            Transform towerTypeButtonTransform = Instantiate(towerTypeButtonPrefab, towerTypeButtonContainerTransform);
            towerTypeButtonTransform.gameObject.SetActive(true);
            towerTypeButtonTransform.Find("Image").GetComponent<Image>().sprite = towerType.sprite;

            MouseEnterExitEvents mouseEnterExitEvents = towerTypeButtonTransform.GetComponent<MouseEnterExitEvents>();

            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => 
            {
                ToolTipUI.Instance.Show(towerType.nameString + "\n" + towerType.goldCost);
            };

            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => 
            {
                ToolTipUI.Instance.Hide();
            };

            towerTypeButtonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (CurrencyManager.Instance.CanAfford(towerType.goldCost))
               {
                  CurrencyManager.Instance.SpendGold(towerType.goldCost);

                  Transform towerPrefab = Instantiate(towerType.prefab, constructionSite.transform.position, Quaternion.identity);

                  towerPrefab.GetComponent<Tower>().SetProjectileType(towerType.projectileType);

                  ToolTipUI.Instance.Hide();

                  Destroy(constructionSite.gameObject);
               }
            });
        }
    }
}

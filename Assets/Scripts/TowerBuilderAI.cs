using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilderAI : MonoBehaviour
{
    [SerializeField] private List<ConstructionSite> constructionSitesList;
    [SerializeField] private List<Tower> towerPrefabs;
    [SerializeField] private int towerConstructionCost = 50;

    private void Update()
    {
        if (CurrencyManager.Instance.CanAfford(towerConstructionCost))
        {
            CurrencyManager.Instance.SpendGold(towerConstructionCost);

            ConstructionSite constructionSite = constructionSitesList[Random.Range(0, constructionSitesList.Count)];

            Instantiate(towerPrefabs[Random.Range(0, towerPrefabs.Count)].transform, constructionSite.transform.position, Quaternion.identity);

            constructionSitesList.Remove(constructionSite);
            Destroy(constructionSite.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;


    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryCounterManager.Instance.OnRecipeCompleted += DeliveryCounterManager_OnRecipeCompleted;
        DeliveryCounterManager.Instance.OnRecipeSpawned += DeliveryCounterManager_OnRecipeSpawned;

        UpdateVisual();
    }

    private void DeliveryCounterManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryCounterManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual(); 
    }

    private void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        

        foreach(RecipeSO recipeSO in DeliveryCounterManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}

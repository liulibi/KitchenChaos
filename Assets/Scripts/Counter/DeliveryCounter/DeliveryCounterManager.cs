using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounterManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSucess;

    public static DeliveryCounterManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        successfulRecipesAmount = 0;
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>(); 
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;


        if(spawnRecipeTimer < 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSO.waitTime = 0;
                waitingRecipeSO.waitTimeMax = 4f;

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }


        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
           
            if (waitingRecipeSOList[0] != null)
            {
                waitingRecipeSOList[0].waitTime += Time.deltaTime;
                print(waitingRecipeSOList[0].waitTime);
            }
            if (waitingRecipeSOList[i].waitTime < waitingRecipeSOList[i].waitTimeMax)
            {
                print("IS");
                waitingRecipeSOList.RemoveAt(i);
                OnRecipeFailed?.Invoke(this,EventArgs.Empty);
            }

        }

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipeSOList.Count; i++) 
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //has the same number of ingredients
                bool plateCountentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) 
                {
                    bool ingredientFound = false;
                    //cycling through all ingredients in the recipe
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //cycling through all ingrdient in the plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                            //ingredient matches!  
                        }
                    }
                    if (!ingredientFound)
                    {
                        //this recipe ingredient was not found on the plate
                        plateCountentsMatchesRecipe = false;
                    }
                }

                if (plateCountentsMatchesRecipe)
                {
                    //player delivery correct recipe
                    successfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSucess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        //no matches found
        //player idd not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }


    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}

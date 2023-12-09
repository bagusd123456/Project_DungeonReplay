using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public class CraftingLibrary : MonoBehaviour
    {
        public List<CraftingRecipe> craftingRecipeList = new List<CraftingRecipe>();

        public static CraftingLibrary Instance { get; private set; }
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting System/New Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public ItemData firstItem;
        public ItemData secondItem;
        public ItemData resultItem;
    }
}
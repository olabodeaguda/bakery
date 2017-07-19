using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class CommonDao
    {
        public RecipeDao recipeDao
        {
            get
            {
                return new RecipeDao();
            }
        }

        

        public bool checkIfProductNotExceedRecipe(ProductionProduct pp)
        {
            //get all recipe gram
            // get all gram for product
            // add current gram to already added gram of products
            //
            return false;
        }
    }
}

// Purpose: Contains the CategoryListViewModel class which is used to display the list of categories in the view.

using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; }
    }
}
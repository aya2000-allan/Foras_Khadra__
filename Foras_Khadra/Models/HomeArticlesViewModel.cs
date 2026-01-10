using System.Collections.Generic;

namespace Foras_Khadra.Models
{
    public class HomeArticlesViewModel
    {
        public List<Article> LatestArticles { get; set; }
        public List<Article> OtherArticles { get; set; }
    }
}

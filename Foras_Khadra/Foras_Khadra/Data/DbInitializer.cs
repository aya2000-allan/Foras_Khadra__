using Foras_Khadra.Models;

namespace Foras_Khadra.Data
{
    public static class DbInitializer
    {
        public static void SeedArticles(ApplicationDbContext context)
        {
            // إذا في بيانات مسبقاً، لا تعيدي الإضافة
            if (context.Articles.Any())
                return;

            var articles = new List<Article>
            {
                
            };

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }
    }
}

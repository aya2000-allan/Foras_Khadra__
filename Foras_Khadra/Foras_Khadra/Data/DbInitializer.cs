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
                new Article
                {
                    Title = "كيفية تعلم البرمجة من الصفر",
                    Author = "أحمد محمد",
                    Content = "هذا محتوى تجريبي للمقالة الأولى. يمكنك استبداله لاحقاً.",
                    PublishDate = DateTime.Now.AddDays(-5),
                    ImagePath = "~/images/logo.png "
                },
                new Article
                {
                    Title = "أهمية الذكاء الاصطناعي في الطب",
                    Author = "سارة خالد",
                    Content = "هذا محتوى تجريبي للمقالة الثانية. يمكنك استبداله لاحقاً.",
                    PublishDate = DateTime.Now.AddDays(-3),
                    ImagePath = "~/images/logo.png "
                },
                new Article
                {
                    Title = "مستقبل العمل الحر في الوطن العربي",
                    Author = "ياسر علي",
                    Content = "هذا محتوى تجريبي للمقالة الثالثة. يمكنك استبداله لاحقاً.",
                    PublishDate = DateTime.Now.AddDays(-1),
                    ImagePath = "~/images/logo.png "
                }
            };

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }
    }
}

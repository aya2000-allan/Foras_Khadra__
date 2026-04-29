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

        /// <summary>
        /// بيانات تجريبية لصفحة شبكة المنظمات (تُضاف مرة واحدة في Development).
        /// </summary>
        public static void SeedOrganizationsMapPreview(ApplicationDbContext context)
        {
            const string demoPrefix = "orgmap-demo-";
            // لا تستخدم StringComparison مع StartsWith — EF Core لا يترجمها إلى SQL
            if (context.Organizations.Any(o =>
                    o.ContactEmail != null &&
                    o.ContactEmail.StartsWith(demoPrefix)))
                return;

            var now = DateTime.UtcNow;
            const string pwd = "Aa1!aaaa"; // نفس نمط الاختبارات؛ الحسابات غير مفعّلة للدخول

            var demos = new Organization[]
            {
                new()
                {
                    Name = "جمعية نور للتنمية المستدامة",
                    Sector = "تعليم",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-1@local.test",
                    PhoneNumber = "0590000001",
                    Password = pwd,
                    Location = "رام الله",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "مؤسسة بسمة للطفولة",
                    Sector = "طفولة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-2@local.test",
                    PhoneNumber = "0590000002",
                    Password = pwd,
                    Location = "غزة",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Green Steps NGO",
                    Sector = "environment",
                    Country = "JO",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-3@local.test",
                    PhoneNumber = "0590000003",
                    Password = pwd,
                    Location = "Amman",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "تجمع إنماء المجتمع المدني",
                    Sector = "تنمية",
                    Country = "LB",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-4@local.test",
                    PhoneNumber = "0590000004",
                    Password = pwd,
                    Location = "بيروت",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Women Forward Initiative",
                    Sector = "women",
                    Country = "EG",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-5@local.test",
                    PhoneNumber = "0590000005",
                    Password = pwd,
                    Location = "Cairo",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "مركز الإغاثة الإنسانية",
                    Sector = "إغاثة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-6@local.test",
                    PhoneNumber = "0590000006",
                    Password = pwd,
                    Location = "الخليل",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "شبكة الإعلام البيئي",
                    Sector = "إعلام",
                    Country = "JO",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-7@local.test",
                    PhoneNumber = "0590000007",
                    Password = pwd,
                    Location = "إربد",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Palestine Tech Hub",
                    Sector = "technology",
                    Country = "PS",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-8@local.test",
                    PhoneNumber = "0590000008",
                    Password = pwd,
                    Location = "Nablus",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "جمعية الزراعة العائلية",
                    Sector = "زراعة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-9@local.test",
                    PhoneNumber = "0590000009",
                    Password = pwd,
                    Location = "جنين",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Youth Volunteers Circle",
                    Sector = "volunteering",
                    Country = "LB",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-10@local.test",
                    PhoneNumber = "0590000010",
                    Password = pwd,
                    Location = "Tripoli",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "مؤسسة الثقافة والفنون",
                    Sector = "ثقافة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-11@local.test",
                    PhoneNumber = "0590000011",
                    Password = pwd,
                    Location = "بيت لحم",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Human Rights Watchers MENA",
                    Sector = "human rights",
                    Country = "JO",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-12@local.test",
                    PhoneNumber = "0590000012",
                    Password = pwd,
                    Location = "Zarqa",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "اتحاد الجمعيات الصحية",
                    Sector = "صحة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-13@local.test",
                    PhoneNumber = "0590000013",
                    Password = pwd,
                    Location = "طولكرم",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Sports for All Association",
                    Sector = "sports",
                    Country = "EG",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-14@local.test",
                    PhoneNumber = "0590000014",
                    Password = pwd,
                    Location = "Alexandria",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "منظمة أعمال ريادية",
                    Sector = "أعمال",
                    Country = "LB",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-15@local.test",
                    PhoneNumber = "0590000015",
                    Password = pwd,
                    Location = "صيدا",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Relief & Hope Coalition",
                    Sector = "relief",
                    Country = "PS",
                    ContactName = "Preview",
                    ContactEmail = "orgmap-demo-16@local.test",
                    PhoneNumber = "0590000016",
                    Password = pwd,
                    Location = "Deir al-Balah",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "شبكة المنظمات البيئية الفلسطينية",
                    Sector = "بيئة",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-17@local.test",
                    PhoneNumber = "0590000017",
                    Password = pwd,
                    Location = "أريحا",
                    CreatedAt = now,
                },
                new()
                {
                    Name = "Filter Demo Org (PS / 222)",
                    Sector = "222",
                    Country = "PS",
                    ContactName = "معاينة",
                    ContactEmail = "orgmap-demo-18@local.test",
                    PhoneNumber = "0590000018",
                    Password = pwd,
                    Location = "رام الله",
                    CreatedAt = now,
                },
            };

            context.Organizations.AddRange(demos);
            context.SaveChanges();
        }
    }
}

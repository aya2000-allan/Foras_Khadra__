using Foras_Khadra.Models;
using System.Collections.Generic;
using System.Linq;

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
                new()
                {
                    TitleAr = "كيف تجد فرص التدريب المثالية",
                    TitleEn = "How to Find the Perfect Training Opportunities",
                    TitleFr = "Comment Trouver les Opportunités de Formation Idéales",
                    ContentAr = "تعرّف على أفضل النصائح لاختيار فرص التدريب التي تطوّر مهاراتك المهنية وتفتح لك أبواب المستقبل.",
                    ContentEn = "Learn the best tips for choosing training opportunities that enhance your skills and open future doors.",
                    ContentFr = "Découvrez les meilleures astuces pour choisir les opportunités de formation qui renforcent vos compétences et ouvrent des portes.",
                    ImagePath = "/images/articles/training.jpg",
                    PublishDate = DateTime.UtcNow.AddDays(-5),
                    Author = "فريق فرص خضرا"
                },
                new()
                {
                    TitleAr = "أحدث المناح الدراسية للشباب العربي",
                    TitleEn = "Latest Scholarships for Arab Youth",
                    TitleFr = "Dernières Bourses pour les Jeunes Arabes",
                    ContentAr = "ما تحتاج معرفته عن المنح الدراسية المفتوحة الآن وكيفية التحضير لطلبك بنجاح.",
                    ContentEn = "What you need to know about scholarships currently open and how to prepare a successful application.",
                    ContentFr = "Ce que vous devez savoir sur les bourses actuellement ouvertes et comment préparer une candidature réussie.",
                    ImagePath = "/images/articles/scholarship.jpg",
                    PublishDate = DateTime.UtcNow.AddDays(-10),
                    Author = "فريق فرص خضرا"
                },
                new()
                {
                    TitleAr = "دليل التطوع في المنظمات البيئية",
                    TitleEn = "A Guide to Volunteering with Environmental Organizations",
                    TitleFr = "Guide du Volontariat au Sein des Organisations Environnementales",
                    ContentAr = "اكتشف كيف تساهم في حماية البيئة من خلال فرص تطوعية مختارة في الوطن العربي.",
                    ContentEn = "Discover how to contribute to environmental protection through selected volunteer opportunities in the Arab world.",
                    ContentFr = "Découvrez comment contribuer à la protection de l'environnement grâce à des opportunités de bénévolat sélectionnées dans le monde arabe.",
                    ImagePath = "/images/articles/volunteering.jpg",
                    PublishDate = DateTime.UtcNow.AddDays(-20),
                    Author = "فريق فرص خضرا"
                }
            };

            context.Articles.AddRange(articles);
            context.SaveChanges();
        }

        public static void SeedOrganizations(ApplicationDbContext context)
        {
            const string pwd = "Aa1!aaaa";
            var seedOrganizations = new List<Organization>
            {
                new()
                {
                    Name = "مركز الابتكار الشبابي",
                    Sector = "تعليم",
                    Country = "PS",
                    ContactName = "سارة محمد",
                    ContactEmail = "seed-org-1@local.test",
                    PhoneNumber = "0591234567",
                    Password = pwd,
                    Location = "رام الله",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/youth-innovation.png"
                },
                new()
                {
                    Name = "مؤسسة دعم المرأة",
                    Sector = "نساء",
                    Country = "JO",
                    ContactName = "ليلى الحسين",
                    ContactEmail = "seed-org-2@local.test",
                    PhoneNumber = "0797654321",
                    Password = pwd,
                    Location = "عمان",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/women-support.png"
                },
                new()
                {
                    Name = "جمعية البيئة النظيفة",
                    Sector = "بيئة",
                    Country = "EG",
                    ContactName = "أحمد علي",
                    ContactEmail = "seed-org-3@local.test",
                    PhoneNumber = "01012345678",
                    Password = pwd,
                    Location = "القاهرة",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/clean-environment.png"
                },
                new()
                {
                    Name = "شبكة الموارد الخضراء",
                    Sector = "تنمية",
                    Country = "LB",
                    ContactName = "هالة رمضان",
                    ContactEmail = "seed-org-4@local.test",
                    PhoneNumber = "031234567",
                    Password = pwd,
                    Location = "بيروت",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/green-resources.png"
                },
                new()
                {
                    Name = "مركز ريادة الأعمال الشبابية",
                    Sector = "أعمال",
                    Country = "MA",
                    ContactName = "يوسف بن عمر",
                    ContactEmail = "seed-org-5@local.test",
                    PhoneNumber = "0661234567",
                    Password = pwd,
                    Location = "الرباط",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/entrepreneurship.png"
                },
                new()
                {
                    Name = "جمعية الصحة المجتمعية",
                    Sector = "صحة",
                    Country = "TN",
                    ContactName = "مها الشارف",
                    ContactEmail = "seed-org-6@local.test",
                    PhoneNumber = "98123456",
                    Password = pwd,
                    Location = "تونس",
                    CreatedAt = DateTime.UtcNow,
                    Website = "https://example.org",
                    LogoPath = "/images/orgs/community-health.png"
                }
            };

            var existingSeedEmails = context.Organizations
                .Where(o => o.ContactEmail != null && o.ContactEmail.StartsWith("seed-org-"))
                .Select(o => o.ContactEmail)
                .ToHashSet();

            var organizationsToAdd = seedOrganizations
                .Where(o => !existingSeedEmails.Contains(o.ContactEmail))
                .ToList();

            if (!organizationsToAdd.Any())
                return;

            context.Organizations.AddRange(organizationsToAdd);
            context.SaveChanges();
        }

        public static void SeedOpportunities(ApplicationDbContext context)
        {
            if (context.Opportunities.Any())
                return;

            var countries = context.Countries.ToList();
            var palestine = countries.FirstOrDefault(c => c.NameEn == "Palestine");
            var jordan = countries.FirstOrDefault(c => c.NameEn == "Jordan");
            var egypt = countries.FirstOrDefault(c => c.NameEn == "Egypt");

            if (palestine == null || jordan == null || egypt == null)
                return;

            var opportunities = new List<Opportunity>
            {
                new()
                {
                    TitleAr = "برنامج تدريب رقمي للمبتدئين",
                    TitleEn = "Digital Training Program for Beginners",
                    TitleFr = "Programme de Formation Numérique pour Débutants",
                    DescriptionAr = "فرصة تدريب عملي في المهارات الرقمية والتسويق عبر الإنترنت.",
                    DescriptionEn = "A practical training opportunity in digital skills and online marketing.",
                    DescriptionFr = "Une opportunité de formation pratique en compétences numériques et marketing en ligne.",
                    DetailsAr = "ستعمل على مشاريع حقيقية وتتلقى شهادة معتمدة بعد الانتهاء.",
                    DetailsEn = "You will work on real projects and receive a certified certificate after completion.",
                    DetailsFr = "Vous travaillerez sur des projets réels et recevrez un certificat reconnu après achèvement.",
                    EligibilityCriteriaAr = "مفتوح للطلبة والخريجين الجدد من جميع التخصصات.",
                    EligibilityCriteriaEn = "Open to students and recent graduates from all disciplines.",
                    EligibilityCriteriaFr = "Ouvert aux étudiants et récents diplômés de toutes disciplines.",
                    BenefitsAr = "شهادة تدريب، خبرة عملية، دعم توجيهي.",
                    BenefitsEn = "Training certificate, practical experience, mentorship support.",
                    BenefitsFr = "Certificat de formation, expérience pratique, soutien d'encadrement.",
                    ApplyLink = "https://example.org/apply/digital-training",
                    PublishedBy = "فرص خضرا",
                    Type = OpportunityType.Internships,
                    DeadlineType = DeadlineType.SpecificDate,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    AvailableCountries = new List<Country> { palestine, jordan }
                },
                new()
                {
                    TitleAr = "زمالة بحثية في الطاقة المتجددة",
                    TitleEn = "Research Fellowship in Renewable Energy",
                    TitleFr = "Bourse de Recherche en Énergies Renouvelables",
                    DescriptionAr = "زمالة لمدة 6 أشهر لدعم مشاريع الطاقة النظيفة.",
                    DescriptionEn = "A 6-month fellowship supporting clean energy projects.",
                    DescriptionFr = "Une bourse de 6 mois pour des projets d'énergie propre.",
                    DetailsAr = "تشمل الزمالة تدريباً مكثفاً، ورشة عمل، وفرص نشر.",
                    DetailsEn = "The fellowship includes intensive training, a workshop, and publication opportunities.",
                    DetailsFr = "La bourse inclut une formation intensive, un atelier et des possibilités de publication.",
                    EligibilityCriteriaAr = "خبرة سنة واحدة على الأقل في مجالات الهندسة أو العلوم.",
                    EligibilityCriteriaEn = "At least one year of experience in engineering or science fields.",
                    EligibilityCriteriaFr = "Au moins un an d'expérience en ingénierie ou sciences.",
                    BenefitsAr = "دعم مالي، إشراف أكاديمي، شبكة علاقات.",
                    BenefitsEn = "Financial support, academic supervision, networking.",
                    BenefitsFr = "Soutien financier, supervision académique, réseautage.",
                    ApplyLink = "https://example.org/apply/renewable-fellowship",
                    PublishedBy = "Foras Khadra",
                    Type = OpportunityType.Fellowships,
                    DeadlineType = DeadlineType.SpecificDate,
                    EndDate = DateTime.UtcNow.AddDays(45),
                    AvailableCountries = new List<Country> { jordan, egypt }
                },
                new()
                {
                    TitleAr = "فرصة تطوع لحماية السواحل",
                    TitleEn = "Coastal Protection Volunteer Opportunity",
                    TitleFr = "Opportunité de Bénévolat pour la Protection Côtière",
                    DescriptionAr = "شارك في حملة تنظيف الشواطئ وزراعة الوعي البيئي.",
                    DescriptionEn = "Join a campaign to clean beaches and raise environmental awareness.",
                    DescriptionFr = "Participez à une campagne de nettoyage des plages et de sensibilisation environnementale.",
                    DetailsAr = "الفعالية تستغرق أسبوعاً وتوفر تدريباً بيئياً مكثفاً.",
                    DetailsEn = "The event lasts one week and provides intensive environmental training.",
                    DetailsFr = "L'événement dure une semaine et offre une formation environnementale intensive.",
                    EligibilityCriteriaAr = "مفتوحة لجميع الأعمار مع اهتمام بالبيئة.",
                    EligibilityCriteriaEn = "Open to all ages with an interest in the environment.",
                    EligibilityCriteriaFr = "Ouvert à tous les âges ayant un intérêt pour l'environnement.",
                    BenefitsAr = "شهادة تطوع، دعم سفر جزئي، خبرة ميدانية.",
                    BenefitsEn = "Volunteer certificate, partial travel support, field experience.",
                    BenefitsFr = "Certificat de bénévolat, soutien partiel aux déplacements, expérience de terrain.",
                    ApplyLink = "https://example.org/apply/coastal-volunteer",
                    PublishedBy = "Foras Khadra",
                    Type = OpportunityType.Volunteering,
                    DeadlineType = DeadlineType.UntilFull,
                    AvailableCountries = new List<Country> { egypt }
                }
            };

            context.Opportunities.AddRange(opportunities);
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

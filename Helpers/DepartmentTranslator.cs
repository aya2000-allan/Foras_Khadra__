using Foras_Khadra.Models;

namespace Foras_Khadra.Helpers
{
    public static class DepartmentTranslator
    {
        public static string Translate(Department department, string culture)
        {
            return culture switch
            {
                // ===================== Arabic =====================
                "ar" => department switch
                {
                    Department.Technology =>
                    "التكنولوجيا|العقل التقني خلف المنصة. يطور الموقع والأدوات الرقمية، ويجعل الوصول للمحتوى والفرص أسهل وأسرع وأكثر فاعلية.",

                    Department.HR =>
                    "الموارد البشرية|المساحة التي ينمو فيها الفريق. يتابع الأعضاء، ينظم انضمامهم، ويعمل على خلق بيئة عمل تشجع المبادرة والاستمرارية.",

                    Department.SocialMedia =>
                    "التواصل الاجتماعي|الصوت الذي ينقل رسالة المنصة للناس. يصنع محتوى قريب ومباشر، ويجعل القضايا البيئية والفرص مفهومة وسهلة الوصول.",

                    Department.BoardOfDirectors =>
                    "الهيئة الإدارية|هي القلب الذي يقود المنصة. فريق يضع الرؤية، يتخذ القرارات، ويتابع التنفيذ اليومي لضمان أن تبقى «فرص خضراء» مساحة حقيقية تصنع الأثر.",

                    Department.HonoraryMembers =>
                    "أعضاء الشرف|أعضاء شغلوا مناصب إدارية سابقًا وكان لهم دور أساسي في بناء وتطوير «فرص خضراء». نذكرهم تقديرًا لجهودهم في مراحل مهمة من مسيرة المنصة.",

                    Department.Opportunities =>
                    "قسم الفرص|العين التي ترصد كل فرصة تستحق أن تصل للشباب في وطننا العربي. يبحث، يراجع، يكتب ويدقق قبل النشر لتحويل الفرص إلى مسارات واضحة قابلة للتقديم.",

                    Department.PublicRelations =>
                    "العلاقات العامة|الجسر الذي يربطنا بالعالم. يبني الشراكات، يفتح أبواب التعاون، ويحوّل التواصل إلى فرص حقيقية للشباب في المنطقة العربية.",

                    Department.LocalTeams =>
                    "الفرق المحلية|النبض على الأرض. فرق تعمل داخل مجتمعاتها، تنفذ مبادرات، وتنقل الواقع كما هو ليبقى عمل المنصة قريبًا من الناس.",

                    _ => department.ToString()
                },

                // ===================== English =====================
                "en" => department switch
                {
                    Department.Technology =>
                    "Technology|The technical mind behind the platform. Develops the website and digital tools, making access to content and opportunities easier, faster, and more effective.",

                    Department.HR =>
                    "Human Resources|The space where the team grows. Manages members, organizes onboarding, and creates an environment that encourages initiative and continuity.",

                    Department.SocialMedia =>
                    "Social Media|The voice that carries the platform’s message. Creates clear and engaging content, making environmental issues and opportunities accessible.",

                    Department.BoardOfDirectors =>
                    "Board of Directors|The heart that leads the platform. Sets the vision, makes strategic decisions, and ensures impactful implementation.",

                    Department.HonoraryMembers =>
                    "Honorary Members|Former administrative members whose contributions were fundamental in building and shaping Foras Khadra.",

                    Department.Opportunities =>
                    "Opportunities Department|Tracks and verifies valuable opportunities for Arab youth, transforming scattered links into clear and applicable pathways.",

                    Department.PublicRelations =>
                    "Public Relations|Builds partnerships, opens collaboration doors, and connects the platform with institutions and communities.",

                    Department.LocalTeams =>
                    "Local Teams|Teams working within their communities, implementing initiatives and ensuring the platform remains grounded and people-centered.",

                    _ => department.ToString()
                },

                // ===================== French =====================
                "fr" => department switch
                {
                    Department.Technology =>
                    "Technologie|L’esprit technique derrière la plateforme. Développe le site et les outils numériques, rendant l’accès au contenu plus simple et plus efficace.",

                    Department.HR =>
                    "Ressources Humaines|L’espace où l’équipe évolue. Gère les membres, organise leur intégration et favorise un environnement encourageant l’initiative.",

                    Department.SocialMedia =>
                    "Médias Sociaux|La voix de la plateforme. Crée un contenu clair et accessible pour rapprocher les enjeux environnementaux du public.",

                    Department.BoardOfDirectors =>
                    "Conseil d’Administration|Le cœur stratégique de la plateforme. Définit la vision et supervise la mise en œuvre quotidienne.",

                    Department.HonoraryMembers =>
                    "Membres Honoraires|Anciens membres administratifs dont la contribution a été essentielle au développement de Foras Khadra.",

                    Department.Opportunities =>
                    "Département des Opportunités|Identifie, vérifie et structure les opportunités pertinentes pour la jeunesse arabe.",

                    Department.PublicRelations =>
                    "Relations Publiques|Établit des partenariats et transforme la communication en opportunités concrètes.",

                    Department.LocalTeams =>
                    "Équipes Locales|Des équipes actives au sein de leurs communautés pour garder la plateforme proche des réalités du terrain.",

                    _ => department.ToString()
                },

                _ => department.ToString()
            };
        }

        public static string TranslateName(Department department, string culture)
        {
            var full = Translate(department, culture);
            return full.Contains("|") ? full.Split('|')[0] : full;
        }

        public static string TranslateDesc(Department department, string culture)
        {
            var full = Translate(department, culture);
            return full.Contains("|") ? full.Split('|')[1] : "";
        }
    }
}
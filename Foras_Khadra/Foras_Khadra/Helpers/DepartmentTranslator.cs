using Foras_Khadra.Models;

namespace Foras_Khadra.Helpers
{
    
        public static class DepartmentTranslator
        {
            public static string Translate(Department department, string culture)
            {
                return culture switch
                {
                    "ar" => department switch
                    {
                        Department.Technology => "التكنولوجيا|هذا القسم مسؤول عن تطوير وصيانة الأنظمة والتقنيات المستخدمة في المنصة.",
                        Department.HR => "الموارد البشرية|يهتم هذا القسم بإدارة شؤون الموظفين والتوظيف والتدريب.",
                        Department.SocialMedia => "التواصل الاجتماعي|يختص هذا القسم بإدارة المحتوى والتواصل عبر وسائل التواصل الاجتماعي.",
                        Department.BoardOfDirectors => "الهيئة الإدارية|تقوم بالقيادة والإشراف على عمل المنصة وتوجيه استراتيجيتها.",
                        Department.HonoraryMembers => "أعضاء الشرف|أعضاء متميزون يساهمون بخبراتهم وشبكاتهم لدعم المنصة.",
                        Department.Opportunities => "الفرص|يعمل على جمع وإدارة الفرص والمنح البيئية المتاحة للشباب.",
                        Department.PublicRelations => "العلاقات العامة|يهتم ببناء الشراكات والتواصل مع المؤسسات والجمهور.",
                        Department.LocalTeams => "الفرق المحلية|فرق محلية تنفذ الأنشطة والفعاليات على الأرض.",
                        _ => department.ToString()
                    },
                    "en" => department switch
                    {
                        Department.Technology => "Technology|Responsible for developing and maintaining platform technologies.",
                        Department.HR => "HR|Manages employee affairs, recruitment, and training.",
                        Department.SocialMedia => "Social Media|Handles content and communication via social media platforms.",
                        Department.BoardOfDirectors => "Board of Directors|Leads and supervises the platform’s strategy.",
                        Department.HonoraryMembers => "Honorary Members|Distinguished members contributing their expertise.",
                        Department.Opportunities => "Opportunities|Collects and manages environmental and climate opportunities.",
                        Department.PublicRelations => "Public Relations|Builds partnerships and communication with institutions and the public.",
                        Department.LocalTeams => "Local Teams|Implement activities and events on the ground.",
                        _ => department.ToString()
                    },
                    "fr" => department switch
                    {
                        Department.Technology => "Technologie|Responsable du développement et de la maintenance des technologies de la plateforme.",
                        Department.HR => "Ressources Humaines|Gère les affaires du personnel, le recrutement et la formation.",
                        Department.SocialMedia => "Médias Sociaux|Gère le contenu et la communication sur les réseaux sociaux.",
                        Department.BoardOfDirectors => "Conseil d’Administration|Dirige et supervise la stratégie de la plateforme.",
                        Department.HonoraryMembers => "Membres Honoraires|Membres distingués apportant leur expertise.",
                        Department.Opportunities => "Opportunités|Collecte et gère les opportunités environnementales et climatiques.",
                        Department.PublicRelations => "Relations Publiques|Établit des partenariats et communique avec les institutions et le public.",
                        Department.LocalTeams => "Équipes Locales|Mettent en œuvre des activités et événements sur le terrain.",
                        _ => department.ToString()
                    },
                    _ => department.ToString()
                };
            }

            public static string TranslateName(Department department, string culture)
            {
                var full = Translate(department, culture);
                return full.Split('|')[0]; // اسم القسم فقط
            }

            public static string TranslateDesc(Department department, string culture)
            {
                var full = Translate(department, culture);
                return full.Contains("|") ? full.Split('|')[1] : "";
            }
        }
    }


namespace Foras_Khadra.Models
{
    public class TeamMember
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; } // مؤسس، مدير قسم، عضو

        public TeamRole Role { get; set; } // الهيئة الإدارية / مدراء الأقسام

        public string Department { get; set; } // اسم القسم (إن وجد)

        public string Bio { get; set; } // الوصف الذي يظهر في النافذة

        public string ImagePath { get; set; }

        public bool IsFounder { get; set; } // مؤسس فرص خضراء
    }

    public enum TeamRole
    {
        AdministrativeBoard = 1,
        DepartmentManager = 2
    }
}

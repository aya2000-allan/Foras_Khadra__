using Foras_Khadra.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Foras_Khadra.Models
{
    public class AllOpportunitiesViewModel
    {
        public List<Opportunity> Opportunities { get; set; } = new();

        public string SelectedCountry { get; set; }
        public List<OpportunityType> SelectedTypes { get; set; } = new();
        // ⚡ إضافة هذه الخاصية للقائمة المنسدلة
        public List<string> Countries { get; set; } = new List<string>();

        // لتوليد قائمة الأنواع للـ dropdown
        public List<SelectListItem> TypeSelectList
        {
            get
            {
                var list = new List<SelectListItem>();
                foreach (var t in Enum.GetValues(typeof(OpportunityType)))
                {
                    var type = (OpportunityType)t;
                    list.Add(new SelectListItem
                    {
                        Text = type.GetDisplayName(),
                        Value = type.ToString(),
                        Selected = SelectedTypes.Contains(type) // <-- تعديلات هنا
                    });
                }
                return list;
            }
        }
    }
}

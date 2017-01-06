using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.Chart
{
    public class UserChartSearchModel
    {
        public UserChartSearchModel()
        {
            SchoolList = new List<SelectListItem>();
            MonthList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "1月", Value = "1"},
                new SelectListItem {Text = "2月", Value = "2"},
                new SelectListItem {Text = "3月", Value = "3"},
                new SelectListItem {Text = "4月", Value = "4"},
                new SelectListItem {Text = "5月", Value = "5"},
                new SelectListItem {Text = "6月", Value = "6"},
                new SelectListItem {Text = "7月", Value = "7"},
                new SelectListItem {Text = "8月", Value = "8"},
                new SelectListItem {Text = "9月", Value = "9"},
                new SelectListItem {Text = "10月", Value = "10"},
                new SelectListItem {Text = "11月", Value = "11"},
                new SelectListItem {Text = "12月", Value = "12"}
            };

            YearList = new List<SelectListItem>();
            var index = DateTime.Now.Year - 5;
            for (int i = 0; i < 10; i++)
            {
                YearList.Add(index == DateTime.Now.Year
                    ? new SelectListItem { Text = index + "年", Value = index.ToString(), Selected = true }
                    : new SelectListItem { Text = index + "年", Value = index.ToString() });
                index++;
            }
            Year = DateTime.Now.Year;
        }
        [Display(Name = "学校")]
        public int School_id { get; set; }
        [Display(Name = "月份")]
        public int Month { get; set; }
        [Display(Name = "年份")]
        public int Year { get; set; }

        public List<SelectListItem> SchoolList { get; set; }

        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> YearList { get; set; }
    }
}
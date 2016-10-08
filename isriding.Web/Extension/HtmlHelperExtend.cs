using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using isriding.Web.Models.Common;

namespace isriding.Web
{
    public static partial class HtmlHelperExtend
    {
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            DisplayDirection direction, int colOrRows, object htmlAttribute, object itemAttribute)
        {
            string resultStr = "";

            string checkPropertyName;
            string displayPropertyName;

            MemberExpression nameExpr = (MemberExpression)expression.Body;
            checkPropertyName = nameExpr.Member.Name;

            PropertyInfo property = typeof(TModel).GetProperty(checkPropertyName);
            KeyValueAttribute attribute = (KeyValueAttribute)property.GetCustomAttributes(typeof(KeyValueAttribute), false)[0];


            if (attribute == null)
            {
                throw new Exception();
            }
            displayPropertyName = attribute.DisplayProperty;

            string validAttribute = "";
            //RequiredAttribute requiredAttribute = (RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false)[0];
            //if (requiredAttribute != null)
            //{
            //    validAttribute += " data-val='true' ";
            //    if (String.IsNullOrEmpty(requiredAttribute.ErrorMessage))
            //    {
            //        string template = "The {0} field is required.";
            //        validAttribute += " data-val-required='" + string.Format(template, displayPropertyName) + "' ";
            //    }
            //    else
            //    {
            //        validAttribute += " data-val-required='" + requiredAttribute.ErrorMessage + "' ";
            //    }
            //}

            IEnumerable checkList = (IEnumerable)htmlHelper.ViewData.Eval(displayPropertyName);
            var isCheckedList = htmlHelper.ViewData.Eval(checkPropertyName) as IEnumerable;

            IDictionary<string, object> htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute);
            IDictionary<string, object> itemAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(itemAttribute);

            string htmlAttributeStr = "";
            foreach (var item in htmlAttributes)
            {
                htmlAttributeStr += " " + item.Key + "='" + item.Value + "' ";
            }
            string itemAttributeStr = "";
            foreach (var item in itemAttributes)
            {
                itemAttributeStr += " " + item.Key + "='" + item.Value + "' ";
            }


            string directionAppend = "";
            if (direction == DisplayDirection.Vertical)
                directionAppend = "<br/>";

            int index = 1;
            resultStr += "<div " + validAttribute + htmlAttributeStr + ">";
            foreach (KeyValueModel check in checkList)
            {
                resultStr += "<span " + itemAttributeStr + ">";
                string checkedStr = "";
                bool isCheck = false;
                foreach (var checkvalue in isCheckedList)
                {
                    isCheck = check.Value == checkvalue.ToString();
                    if (isCheck)
                    {
                        checkedStr = " checked='checked' ";
                        break;
                    }
                }
                KeyValueModel checkModel = (KeyValueModel)check;
                if (check.Disable == "disabled" && check.Disable != null)
                {
                    resultStr += "<label class='label-multi'><input class='" + check.Disable + "'disabled='" + check.Disable + "'name='" + checkPropertyName + "' type=\"checkbox\" value='" + check.Value + "'" + checkedStr + " ></input>" + " ";
                    resultStr += checkModel.Text + "</label>";
                }
                else
                {
                    resultStr += "<label class='label-multi'><input name='" + checkPropertyName + "' type=\"checkbox\" value='" + check.Value + "'" + checkedStr + " ></input>" + " ";
                    resultStr += checkModel.Text + "</label>";
                }

                resultStr += "</span>";
                if (index % colOrRows == 0)
                {
                    resultStr += directionAppend;
                }
                index = index + 1;
            }
            resultStr += "</div>";

            MvcHtmlString result = new MvcHtmlString(resultStr);
            return result;
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DisplayDirection direction, object htmlAttribute, object itemAttribute)
        {
            return CheckBoxListFor(htmlHelper, expression, direction, 2, htmlAttribute, itemAttribute);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DisplayDirection direction, object htmlAttribute)
        {
            return CheckBoxListFor(htmlHelper, expression, direction, 2, null, new { style = "display:inline-block;width:70px;" });
        }

    }
}
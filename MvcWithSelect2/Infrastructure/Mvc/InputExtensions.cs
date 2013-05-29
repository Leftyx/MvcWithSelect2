using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcWithSelect2.Infrastructure.Mvc
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Mvc.Properties;
    using System.Globalization;
    using System.Linq.Expressions;

    public static class Select2InputExtensions
    {
        private const string DATA_PLACEHOLDER = "data-placeholder";
        private const string DATA_OPTION = "data-option";

        public static IHtmlString Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.Select2For(expression, format: null);
        }

        public static IHtmlString Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
        {
            return htmlHelper.Select2For(expression, format, (IDictionary<string, object>)null);
        }

        public static IHtmlString Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.Select2For(expression, format: null, htmlAttributes: htmlAttributes);
        }

        public static IHtmlString Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        {
            return htmlHelper.Select2For(expression, format: format, htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString Select2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Select2For(expression, format: null, htmlAttributes: htmlAttributes);
        }

        public static IHtmlString Select2For<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            string value = string.Empty;
            string dataPlaceHolder = string.Empty;

            if (metadata.Model != null)
            {
                value = metadata.Model.ToString();
            }

            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("Cannot find fullName.");
            }

            System.Web.Mvc.TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
            tagBuilder.MergeAttribute("name", fullName, true);

            if (metadata.AdditionalValues != null)
            {
                if (metadata.AdditionalValues.ContainsKey(DATA_PLACEHOLDER))
                {
                    tagBuilder.MergeAttribute(DATA_PLACEHOLDER, metadata.AdditionalValues[DATA_PLACEHOLDER] as string);
                }
                if (metadata.AdditionalValues.ContainsKey(DATA_OPTION))
                {
                    string fieldName = metadata.AdditionalValues[DATA_OPTION] as string;
                    var parentType = metadata.ContainerType;
                    var parentMetaData = ModelMetadataProviders.Current
                        .GetMetadataForProperties(htmlHelper.ViewData.Model, parentType);

                    var dataOptionValue = (string)parentMetaData.FirstOrDefault(p => p.PropertyName == fieldName).Model;

                    tagBuilder.MergeAttribute(DATA_OPTION, dataOptionValue ?? "");
                }
            }

            string valueParameter = htmlHelper.FormatValue(value, format);
            // bool usedModelState = false;
            bool useViewData = false;
            bool isExplicitValue = true;

            string attemptedValue = string.Empty;

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Value != null)
                {
                    attemptedValue = (string)modelState.Value.ConvertTo(typeof(string), null /* culture */);
                }
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            var validationAttribs = htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata);
            tagBuilder.MergeAttributes(validationAttribs);
            // tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(string.Empty), metadata));

            tagBuilder.MergeAttribute("value", value ?? ((useViewData) ? attemptedValue : valueParameter), isExplicitValue);

            tagBuilder.GenerateId(fullName);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

    }
}

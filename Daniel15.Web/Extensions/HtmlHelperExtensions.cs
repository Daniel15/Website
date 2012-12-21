using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Daniel15.Web.ViewModels.Shared;
using StackExchange.Profiling;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Various HTML helpers for MVC views
	/// </summary>
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Renders a list of checkboxes for the specified property
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <typeparam name="TItem">The type of the items.</typeparam>
		/// <typeparam name="TId">The type of the ID.</typeparam>
		/// <param name="htmlHelper">The HTML helper.</param>
		/// <param name="expression">Expression for the property being modified</param>
		/// <param name="allItems">All the available items</param>
		/// <param name="idFunc">Function to get the ID of an item</param>
		/// <param name="labelFunc">Function to get the label of an item</param>
		/// <returns></returns>
		public static IHtmlString CheckboxListFor<TModel, TItem, TId>(
			this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TId>>> expression, 
			IEnumerable<TItem> allItems, Func<TItem, TId> idFunc, Func<TItem, string> labelFunc)
		{
			var name = ExpressionHelper.GetExpressionText(expression);
			using (MiniProfiler.Current.Step("Building CheckboxList for " + name))
			{
				var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

				// Convert selected items list to hashset for quick lookup
				var selectedItems = new HashSet<TId>(metadata.Model as IEnumerable<TId> ?? Enumerable.Empty<TId>());

				// Build list of checkbox items
				var checkboxItems = allItems.Select(item =>
				{
					var id = idFunc(item);
					return new CheckboxListModel.Item
					{
						Id = id,
						Label = labelFunc(item),
						Selected = selectedItems.Contains(id)
					};
				});

				// Actually render the list
				return htmlHelper.Partial(MVC.Shared.Views._CheckboxList, new CheckboxListModel
				{
					Name = name,
					Items = checkboxItems
				});
			}
		}
	}
}
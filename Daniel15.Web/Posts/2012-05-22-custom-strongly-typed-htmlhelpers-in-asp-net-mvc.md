---
id: 335
title: Custom strongly-typed HtmlHelpers in ASP.NET MVC
published: true
publishedDate: 2012-05-22 04:59:00Z
lastModifiedDate: 2012-05-22 04:59:00Z
categories:
- C#
- Web Development

---

The original release of ASP.NET MVC used HTML helpers with a syntax like the following:

```csharp
@Html.TextArea("Title")
```

These worked, but if you renamed the property in your model (for example, from “Title” to “Subject”) and forgot to update your view, you wouldn’t catch this error until you actually tried out the page and noticed your model isn’t populating properly. By this time, you might have users using the site and wondering why stuff isn’t working.

ASP.NET MVC 2 introduced the concept of strongly-typed HtmlHelper extensions, and ASP.NET MVC 3 extended this even further. An example of a strongly typed HtmlHelper is the following:

```csharp
@Html.TextAreaFor(post => post.Title)
```

These allow you to write more reliable code, as view compilation will fail if you change the field name in your model class but forget to change the field name in the view. If you use precompiled views, this error will be caught before deployment. 

## Creating your own
The built-in helpers are good, but quite often it’s nice to create your own helpers (for example, if you have your own custom controls like a star rating control or rich-text editor).<!--more-->
 These new helpers are very easy to create, since we can make use of two different classes that come with ASP.NET MVC:

* [ExpressionHelper](http://msdn.microsoft.com/en-us/library/system.web.mvc.expressionhelper.aspx) — Gets the model name from a lambda expression (for example, returns the string “Date” for the expression “post => post.Date”, and “Author.Email” for the expression “post => post.Author.Email”). This is what you’d use in the ID and name of the field
* [ModelMetadata](http://msdn.microsoft.com/en-us/library/system.web.mvc.modelmetadata.aspx) — Gets other information about the lambda expression, including its value

These two classes give us all the information we require to make our own HTML helpers (internally, these are what all the built-in strongly-typed HTML helpers use).

Here’s an example of a simple HTML helper that uses both of the above classes:

```csharp
public static MvcHtmlString NewTextBox(this HtmlHelper htmlHelper, string name, string value)
{
	var builder = new TagBuilder("input");
	builder.Attributes["type"] = "text";
	builder.Attributes["name"] = name;
	builder.Attributes["value"] = value;
	return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
}

public static MvcHtmlString NewTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
{
	var name = ExpressionHelper.GetExpressionText(expression);
	var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
	return NewTextBox(htmlHelper, name, metadata.Model as string);
}
```

Given a model like this:

```csharp
public class Post
{
	public string Title { get; set; }
	// ...
}
```

A view like this:

```csharp
@Html.NewTextBoxFor(model => model.Title)
```

Will produce HTML like this:

```html
<input name="Title" type="text" value="" />
```

For helpers with larger chunks of HTML, I’d suggest using partial views. These can be rendered using htmlHelper.Partial().

Hopefully this helps someone!

Until next time,  

— Daniel

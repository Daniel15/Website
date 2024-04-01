---
id: 350
title: "CSS: Two columns on desktop, one column on mobile"
published: true
publishedDate: 2024-03-31 22:52:00Z
lastModifiedDate: 2024-03-31 22:52:00Z
categories:
- Web Development

---

# This post is originally from Daniel15's Blog at https://d.sb/2024/03/css-conditional-two-column

---

In HTML and CSS, it's not uncommon to need a layout where you have two columns side-by-side on wide screens:

<div style="background-color: #333; color: #EEE; padding: 20px; width: 440px; width: max-content; text-align: center; margin-bottom: 12px">
	<div style="border: 2px solid lightblue; display: inline-block; padding: 8px; width: 200px; margin-right: 12px; box-sizing: border-box">Hello from column 1</div>
	<div style="border: 2px solid lightgreen; display: inline-block; padding: 8px; width: 200px; box-sizing: border-box">Hello from column 2</div>
</div>
<!-- ![Two-column layout](https://d.sb/blog-content/2024/two-col.png) -->

but both collapse into a single column on narrow screens such as mobile devices:

<div style="background-color: #333; color: #EEE; padding: 20px; width: 200px; text-align: center; margin-bottom: 12px">
	<div style="border: 2px solid lightblue; padding: 8px; margin-bottom: 12px; box-sizing: border-box">Hello from column 1</div>
	<div style="border: 2px solid lightgreen;  padding: 8px; box-sizing: border-box">Hello from column 2</div>
</div>
<!-- ![One-column layout](https://d.sb/blog-content/2024/one-col.png) -->


Let's start with some basic HTML:
```html
<div class="container">
  <div class="two-columns">
    <div class="one">
      Hello from column 1
    </div>
    <div class="two">
      Hello from column 2
    </div>
  </div>
</div>
```

There's several ways to accomplish this design, but one of the most elegant is using [CSS grid](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_grid_layout). Here's some example CSS that uses grid layout:

```css
.container {
  container-type: inline-size;
}

.two-columns {
  display: grid;
  gap: 8px;
  /* One column by default */
  grid-template-columns: 1fr;
}

@container (width > 500px) {
  .two-columns {
    /* Two columns on wider screens */
    grid-template-columns: 1fr 1fr;
  }
}
```

Here's a JSFiddle with the final HTML and CSS: https://jsfiddle.net/Daniel15/gefuncyp/1/

Pretty simple, right? This CSS snippet uses several newer CSS features:

## Container queries
Most web developers know about [media queries](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_media_queries), which let you apply particular CSS styles based on the size of the screen. They are very commonly used for responsive design, which is a design technique where a web site works well regardless of screen width.

[Container queries](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_containment/Container_queries) are similar to media queries, except they let you apply styles based on the size of the **container** element. In this case, we're saying that we want to show two columns only when the width of the container is greater than 500px. Defining break points based on the element rather than the screen makes the whole thing more reusable. This is useful if the element is embedded inside an existing app that might have its own sidebar, padding, etc. 

## `fr` units

`fr` units are [*fractional* units](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_grid_layout/Basic_concepts_of_grid_layout#the_fr_unit), which are new with CSS grid. They represent a fraction of the available space. For example, a grid with two `1fr` columns (like we're using for the desktop layout) will have two columns of equal width, effectively 50% minus the gap between them.

One way they're useful is if you want both fixed-width and flexible-width columns in the same grid. For example, `grid-template-columns: 80px 2fr 1fr 100px` creates four columns: The leftmost and rightmost columns have a fixed width of 80px and 100px respectively, while the inner columns consume 66.67% (2/3) and 33.33% (1/3) of the left over width respectively.

# Conclusion

Newer CSS functionality such as CSS grid make things like this way easier than it used to be in the past, when we had to mess with floats to get multi-column layouts working correctly. I use flexbox more than CSS grid, but both are extremely useful tools to have. CSS grid has [widespread support](https://caniuse.com/css-grid) so now's the time to start learning about it if you haven't already!

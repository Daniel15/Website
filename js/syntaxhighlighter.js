/**
 * dan.cx JavaScript (revision 2) - By Daniel15, 2011-2012
 * Syntax highlighting
 * Feel free to use any of this, but please link back to my site
 */
 
if (SyntaxHighlighter && SyntaxHighlighter.all)
{
	SyntaxHighlighter.defaults['toolbar'] = false;
	// Run in another "thread"
	SyntaxHighlighter.highlight.bind(SyntaxHighlighter).delay(0);
}
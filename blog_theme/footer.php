<?php
// TODO: Clean this up!
if (Daniel15Blog::is_compression_enabled())
{
	$siteData = Daniel15Blog::get_compression_info();
}
?>
				<?php if(function_exists('wp_pagenavi')) { wp_pagenavi(); } ?>
				<p></p>
			</div></div>

<?php get_sidebar(); ?>

		</div></div>
		<div id="footer">
			&copy;2008&ndash;2010 <a href="http://<?php echo $_SERVER['HTTP_HOST']; ?>">Daniel15 (Daniel Lo Nigro)</a>.
			Valid <a href="http://validator.w3.org/check?uri=referer">XHTML</a> and <a href="http://jigsaw.w3.org/css-validator/">CSS</a>.<br />
			Powered by <a href="http://wordpress.org/">WordPress</a>. <a href="<?php bloginfo('rss2_url'); ?>">Entries (RSS)</a> and <a href="<?php bloginfo('comments_rss2_url'); ?>">Comments (RSS)</a>.
			<!--<?php echo get_num_queries(); ?> queries. <?php timer_stop(1); ?> seconds.-->

		</div>
	</div>
	<!-- Now for the JS -->	
	<script src="http://ajax.googleapis.com/ajax/libs/mootools/1.3.0/mootools-yui-compressed.js" type="text/javascript"></script>
<?php if (Daniel15Blog::is_compression_enabled()) : ?>
	<script src="/res/<?php echo $siteData->latestJS ?>" type="text/javascript"></script>
<?php else: ?>
	<script src="/res/mootools-more-1.3.0.1.js" type="text/javascript"></script>
	<script src="/res/scripts_r1.js" type="text/javascript"></script>
<?php endif; ?>
<?php wp_footer(); ?>
</body>
</html>

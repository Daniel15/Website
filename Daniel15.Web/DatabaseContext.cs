using Daniel15.Web.Models.Blog;
using Daniel15.Web.Models.Projects;
using Daniel15.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Daniel15.Web
{
	/// <summary>
	/// Entity Framework database context
	/// </summary>
	public class DatabaseContext : DbContext
	{
		/// <summary>
		/// Creates a new instance of <see cref="DatabaseContext"/>.
		/// </summary>
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

		/// <summary>
		/// Projects in the database.
		/// </summary>
		public virtual DbSet<ProjectModel> Projects { get; set; }
		/// <summary>
		/// Technologies used to build projects.
		/// </summary>
		public virtual DbSet<ProjectTechnologyModel> Technologies { get; set; }
		/// <summary>
		/// Blog posts.
		/// </summary>
		public virtual DbSet<PostModel> Posts { get; set; }
		/// <summary>
		/// Blog categories.
		/// </summary>
		public virtual DbSet<CategoryModel> Categories { get; set; }
		/// <summary>
		/// Blog tags.
		/// </summary>
		public virtual DbSet<TagModel> Tags { get; set; }
		/// <summary>
		/// Comments synchronised from Disqus.
		/// </summary>
		public virtual DbSet<DisqusCommentModel> DisqusComments { get; set; }

		/// <summary>
		/// Initialises the Entity Framework model
		/// </summary>
		/// <param name="modelBuilder">EF model builder</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ConfigureConventions();
			ConfigureManyToMany(modelBuilder);

			// Special cases
			modelBuilder.Entity<ProjectTechnologyModel>().ToTable("project_techs");
			modelBuilder.Entity<PostModel>().ToTable("blog_posts");
			modelBuilder.Entity<PostModel>().Property(x => x.MainCategoryId).HasColumnName("maincategory_id");
			modelBuilder.Entity<CategoryModel>().ToTable("blog_categories");
			modelBuilder.Entity<CategoryModel>().Property(x => x.ParentId).HasColumnName("parent_category_id");
			modelBuilder.Entity<TagModel>().ToTable("blog_tags");
			modelBuilder.Entity<TagModel>().Property(x => x.ParentId).HasColumnName("parent_tag_id");
			modelBuilder.Entity<DisqusCommentModel>()
				.ToTable("disqus_comments")
				.Ignore(x => x.Children);

			modelBuilder.Entity<PostModel>()
				.Property(x => x.RawContent)
				.HasColumnName("content");

			// Backwards compatibility with old DB - Dates as UNIX times
			modelBuilder.Entity<PostModel>()
				.Property(x => x.Date)
				.HasConversion(
					x => x.ToUnix(),
					x => DateExtensions.FromUnix(x)
				);

			modelBuilder.Entity<PostModel>()
				.Property(x => x.ShareCounts)
				.HasColumnName("share_counts")
				.HasConversion(
					x => JsonConvert.SerializeObject(x),
					x => string.IsNullOrEmpty(x) ? null : JsonConvert.DeserializeObject<IDictionary<string, int>>(x)
				);

			modelBuilder.Entity<ProjectModel>()
				.Property(x => x.ProjectType)
				.HasColumnName("type")
				.HasConversion(
					x => x.ToString(),
					x => x.FirstUpper().ParseEnum<ProjectType>()
				);

			modelBuilder.Entity<ProjectModel>()
				.Property(x => x.Technologies)
				.HasConversion(
					x => string.Join(",", x),
					x => x.Split(',',StringSplitOptions.None)
				);
		}

		/// <summary>
		/// Configures many to many relationships
		/// </summary>
		/// <param name="modelBuilder">EF model builder</param>
		private void ConfigureManyToMany(ModelBuilder modelBuilder)
		{
			// Posts to categories many to many
			modelBuilder.Entity<PostCategoryModel>()
				.ToTable("blog_post_categories")
				.HasKey(x => new { x.PostId, x.CategoryId });
			modelBuilder.Entity<PostModel>()
				.HasMany(x => x.PostCategories)
				.WithOne(x => x.Post);
			modelBuilder.Entity<CategoryModel>()
				.HasMany(x => x.PostCategories)
				.WithOne(x => x.Category);

			// Posts to tags many to many
			modelBuilder.Entity<PostTagModel>()
				.ToTable("blog_post_tags")
				.HasKey(x => new { x.PostId, x.TagId });
			modelBuilder.Entity<PostModel>()
				.HasMany(x => x.PostTags)
				.WithOne(x => x.Post);
			modelBuilder.Entity<TagModel>()
				.HasMany(x => x.PostTags)
				.WithOne(x => x.Tag);
		}
	}
}

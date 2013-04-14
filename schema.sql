-- phpMyAdmin SQL Dump
-- version 3.3.9
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: May 07, 2011 at 08:40 PM
-- Server version: 5.5.8
-- PHP Version: 5.3.5

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `daniel15_new`
--

-- --------------------------------------------------------

--
-- Table structure for table `blog_categories`
--

CREATE TABLE IF NOT EXISTS `blog_categories` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `slug` varchar(255) NOT NULL,
  `parent_category_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `slug` (`slug`),
  KEY `parent_category_id` (`parent_category_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blog_comments`
--
-- No longer required since Disqus is now used
/*CREATE TABLE IF NOT EXISTS `blog_comments` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `post_id` int(10) unsigned NOT NULL,
  `author` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `url` varchar(255) DEFAULT NULL,
  `ip` varchar(255) NOT NULL,
  `ip2` varchar(255) DEFAULT NULL,
  `date` int(10) unsigned NOT NULL,
  `content` text NOT NULL,
  `parent_comment_id` int(10) unsigned DEFAULT NULL,
  `status` enum('pending','visible','hidden','spam') NOT NULL,
  `user_agent` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `parent_comment_id` (`parent_comment_id`),
  KEY `post_id` (`post_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;*/

-- --------------------------------------------------------

--
-- Table structure for table `blog_posts`
--

CREATE TABLE IF NOT EXISTS `blog_posts` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `date` int(10) unsigned NOT NULL,
  `content` text NOT NULL,
  `modified` int(10) unsigned DEFAULT NULL,
  `comment_count` int(10) unsigned NOT NULL,
  `maincategory_id` int(10) unsigned NOT NULL,
  `slug` varchar(255) NOT NULL,
  `published` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `slug` (`slug`),
  KEY `maincategory_id` (`maincategory_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blog_post_categories`
--

CREATE TABLE IF NOT EXISTS `blog_post_categories` (
  `post_id` int(10) unsigned NOT NULL,
  `category_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`post_id`,`category_id`),
  KEY `post_id` (`post_id`),
  KEY `category_id` (`category_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blog_post_tags`
--

CREATE TABLE IF NOT EXISTS `blog_post_tags` (
  `post_id` int(10) unsigned NOT NULL,
  `tag_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`post_id`,`tag_id`),
  KEY `tag_id` (`tag_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blog_tags`
--

CREATE TABLE IF NOT EXISTS `blog_tags` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `slug` varchar(255) NOT NULL,
  `parent_tag_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `parent_tag_id` (`parent_tag_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `blog_subscriptions`
--
-- No longer required since Disqus is now used
/*CREATE TABLE IF NOT EXISTS `blog_subscriptions` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `post_id` int(10) unsigned NOT NULL,
  `email` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `post_id` (`post_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;*/

-- --------------------------------------------------------
--
-- Table structure for table `disqus_comments`
--
CREATE TABLE IF NOT EXISTS `disqus_comments` (
  `id` varchar(32) NOT NULL,
  `thread_id` varchar(32) NOT NULL,
  `thread_link` varchar(200) NOT NULL,
  `thread_identifier` varchar(255) NOT NULL,
  `parent_comment_id` varchar(32) DEFAULT NULL,
  `content` text NOT NULL,
  `author_name` varchar(200) NOT NULL,
  `author_url` varchar(200) DEFAULT NULL,
  `author_profile_url` varchar(255) NOT NULL,
  `author_image` varchar(255) DEFAULT NULL,
  `date` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `thread_identifier` (`thread_identifier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


--
-- Constraints for dumped tables
--

--
-- Constraints for table `blog_categories`
--
ALTER TABLE `blog_categories`
  ADD CONSTRAINT `blog_categories_ibfk_1` FOREIGN KEY (`parent_category_id`) REFERENCES `blog_categories` (`id`);

--
-- Constraints for table `blog_comments`
--
/*ALTER TABLE `blog_comments`
  ADD CONSTRAINT `blog_comments_ibfk_2` FOREIGN KEY (`parent_comment_id`) REFERENCES `blog_comments` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `blog_comments_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `blog_posts` (`id`) ON DELETE CASCADE;*/

--
-- Constraints for table `blog_posts`
--
ALTER TABLE `blog_posts`
  ADD CONSTRAINT `blog_posts_ibfk_1` FOREIGN KEY (`maincategory_id`) REFERENCES `blog_categories` (`id`);

--
-- Constraints for table `blog_post_categories`
--
ALTER TABLE `blog_post_categories`
  ADD CONSTRAINT `blog_post_categories_ibfk_2` FOREIGN KEY (`post_id`) REFERENCES `blog_posts` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `blog_post_categories_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `blog_categories` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `blog_post_tags`
--
ALTER TABLE `blog_post_tags`
  ADD CONSTRAINT `blog_post_tags_ibfk_2` FOREIGN KEY (`tag_id`) REFERENCES `blog_tags` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `blog_post_tags_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `blog_posts` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `blog_tags`
--
ALTER TABLE `blog_tags`
  ADD CONSTRAINT `blog_tags_ibfk_1` FOREIGN KEY (`parent_tag_id`) REFERENCES `blog_tags` (`id`);


--
-- Views
--
CREATE VIEW v_blog_categories AS
SELECT cat.id, cat.title, cat.slug, cat.parent_category_id,
	parent.slug AS parent_slug
FROM blog_categories cat
LEFT OUTER JOIN blog_categories parent ON parent.id = cat.parent_category_id
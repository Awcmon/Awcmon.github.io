﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanDuongCom
{
	class Blog : Page 
	{
		SortedSet<BlogPost> blogPosts;

		public Blog(string templatePath, string id, List<NavItem> navItems) : base(templatePath, id, navItems)
		{
			blogPosts = new SortedSet<BlogPost>();
			ProcessBlogPosts(@"BlogPosts\", @"ProcessedBlogPosts\");
		}

		public void LoadBlogPosts(string dir)
		{
			if(Directory.Exists(dir))
			{
				string[] postPaths = Directory.GetFiles(dir);

				foreach(string p in postPaths)
				{
					blogPosts.Add(new BlogPost(p));
				}
			}
			else
			{

			}
		}

		public void ProcessBlogPosts(string srcDir, string outputDir)
		{
			if (!Directory.Exists(srcDir))
			{
				Console.WriteLine("Src dir does not exist.");
				return;
			}

			if(!Directory.Exists(outputDir))
			{
				Directory.CreateDirectory(outputDir);
			}

			string[] srcBlogPosts = Directory.GetFiles(srcDir);
			foreach(string p in srcBlogPosts)
			{
				ProcessBlogPost(p, outputDir);
			}
		}

		public bool ProcessBlogPost(string path, string outputDir) //process a raw blog post
		{
			//the first line is the title
			//the second line are tags
			//every line after should be the content.
			string[] lines = File.ReadAllLines(path);

			if(lines.Count() < 2 || !File.Exists(path))
			{
				Console.WriteLine(path + " is not correctly formatted.");
				return false;
			}

			string fileName = path.Split(new char[] { '/', '\\' }).Last();
			string processedPathName = String.Format("{0}/{1}", outputDir, fileName);

			if (!File.Exists(processedPathName))
			{
				var processed = File.CreateText(processedPathName);
				processed.WriteLine(DateTime.Now.ToFileTimeUtc());
				foreach (String l in lines)
				{
					processed.WriteLine(l);
				}
				processed.Close();
			}

			File.Delete(path); //delete the original file now that we have processed it.

			return true;
		}

	}
}

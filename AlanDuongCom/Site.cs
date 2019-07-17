﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlanDuongCom
{
	class Site
	{
		private List<Page> pages;
		private List<NavItem> navItems;

		private string sitename;
		private string templatePath;

		//turn a string into only lowercase letters, numbers, and hyphens.
		private string SanitizeURL(string url)
		{
			Regex rgx = new Regex("[^a-zA-Z0-9-]");
			return rgx.Replace(url.ToLower(), "-");
		}

		//navCategory: "" to show on the navBar without a category, null to not show at all
		public void AddPage(string title, string contentPath, string navCategory)
		{
			pages.Add(new Page(title, contentPath, sitename, templatePath, navItems));
			
			//add an entry for this page to the list of NavItems.
			if(navCategory == null) { return; }
			if (navCategory == "")
			{
				navItems.Add(new NavItem(title, SanitizeURL(title) + ".html"));
			}
			else
			{
				NavItem category = null;
				foreach(NavItem i in navItems)
				{
					if(i.title == navCategory) //if there already exists an item for that category
					{
						category = i;
						break;
					}
				}
				if(category == null) //if the category does not exist yet, create it
				{
					category = new NavItem(navCategory, "#");
					navItems.Add(category);
				}
				category.children.Add(new NavItem(title, SanitizeURL(title) + ".html"));
			}
		}

		public Site(string sitename, string templatePath)
		{
			this.sitename = sitename;
			this.templatePath = templatePath;
			navItems = new List<NavItem>();
			pages = new List<Page>();
		}

		//bake and write all the pages to the final html output
		public void Generate()
		{
			System.IO.Directory.CreateDirectory("Out");
			foreach (Page p in pages)
			{
				p.GenerateNav();
				System.IO.File.WriteAllText(@"Out\" + SanitizeURL(p.title) + ".html", p.Bake());
			}
		}
	}
}

---
id: 345
title: 'Fixing "error MSB3552: Resource file "**/*.resx" cannot be found"'
published: true
publishedDate: 2017-05-10 21:50:21Z
lastModifiedDate: 2017-05-10 21:50:21Z
categories:
- C#
- Programming

---

<p>Recently I was upgrading one of my projects from Visual Studio 2015 to Visual Studio 2017 (including converting from project.json and .xproj to .csproj), when I hit an error like this:</p>

<pre>Microsoft.Common.CurrentVersion.targets(2867,5): error MSB3552: Resource file "**/*.resx" cannot be found.</pre>

<p>It turns out this is caused by a long-standing MSBuild bug: <a href="https://github.com/Microsoft/msbuild/issues/406" rel="nofollow" target="_blank">Wildcard expansion is silently disabled when a wildcard includes a file over MAX_PATH</a>. The <code>Microsoft.NET.Sdk.DefaultItems.props</code> file bundled with .NET Core includes a section that looks like this:</p>
<pre>&lt;EmbeddedResource 
  Include="**/*.resx" 
  Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)"
  Condition=" '$(EnableDefaultEmbeddedResourceItems)' == 'true' "
/></pre>

<p>When MSBuild tries to expand the <code>**/*.resx</code> wildcard, it hits this bug, resulting in the wildcard not being expanded properly. Some other MSBuild task interprets the <code>**/*.resx</code> as a literal file name, and crashes and burns as a result.</p>

<p>In my case, my build server was running an old version of npm, which is known to create extremely long file paths. The way to "fix" this is by reducing the nesting of your folders. If you're using npm,
 upgrading to a newer version (or switching to <a href="https://yarnpkg.com/" target="_blank">Yarn</a>) should fix the issue. Otherwise, you may need to move your project to a different directory, such as a directory in the root of <code>C:\</code>.</p>

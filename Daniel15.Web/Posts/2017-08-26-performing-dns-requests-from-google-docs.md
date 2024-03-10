---
id: 346
title: Performing DNS requests from Google Docs
published: true
publishedDate: 2017-08-26 15:09:29Z
lastModifiedDate: 2017-08-26 15:09:29Z
categories:
- Web Development

---

<p>I use a Google Docs spreadsheet to manage all my domains. It contains a list of all the domain names I own, along with their expiry dates, the name of the registrar the domain is registered with, and some other details. I also wanted to also add a column showing the nameservers, so I could tell which domains were parked vs which domains I'm actively using.</p>
<p>Google Apps Script provides a <a href="https://developers.google.com/apps-script/reference/url-fetch/url-fetch-app" rel="nofollow">URLFetchApp.fetch</a> function to perform network requests. We can combine this with Google's <a href="https://developers.google.com/speed/public-dns/docs/dns-over-https" rel="nofollow">DNS-over-HTTPS API</a> to load DNS records for a given domain:</p>
<pre class="brush: javascript">
function GetDNSEntries(domain, type) {
  var response = UrlFetchApp.fetch('https://dns.google.com/resolve?name=' + domain + '&type=' + type);
  var data = JSON.parse(response);
  
  var results = data.Answer.map(function(answer) {
    // Remove trailing dot from answer
    return answer.data.replace(/\.$/, '');
  });
  return results.sort().join(', ');
}
</pre>

<p>We can then use this function in a spreadsheet:</p>
<pre class="brush: javascript">
=GetDNSEntries(A1, "NS")
</pre>

<p>This results in a column listing the DNS servers for each domain, with data that's always kept up-to-date by Google Docs:</p>
<img src="https://dan.cx/blog-content/2017/dns-google-docs.png" />

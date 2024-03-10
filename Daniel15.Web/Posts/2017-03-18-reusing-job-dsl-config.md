---
id: 344
title: Reusing Job-DSL configuration sections
published: true
publishedDate: 2017-03-18 20:58:03Z
lastModifiedDate: 2017-03-18 20:58:03Z
summary: Job DSL is an excellent plugin for Jenkins. In this post, I describe how to reuse job configuration across multiple jobs.
categories:
- Programming

---

<p><a href="https://github.com/jenkinsci/job-dsl-plugin/wiki">Job DSL</a> is an excellent plugin for Jenkins, allowing you to configure your Jenkins jobs through code rather than through the Jenkins UI. This allows you to more easily track changes to your Jenkins jobs, and revert to old versions in case of any issues. As an example, for the <a href="https://yarnpkg.com/">Yarn project</a>, we have a Jenkins job to publish a <a href="https://chocolatey.org/">Chocolatey</a> package whenever a new stable Yarn version is out. The configuration for a Jenkins job to do that might look something like this:</p>

<pre class="brush: plain">
job('yarn-chocolatey') {
  displayName 'Yarn Chocolatey'
  description 'Publishes a Chocolatey package whenever Yarn is updated'
  label 'windows'
  scm {
    github 'yarnpkg/yarn', 'master'
  }
  triggers {
    urlTrigger {
      cron 'H/15 * * * *'
      url('https://yarnpkg.com/latest-version') {
        inspection 'change'
      }
    }
  }
  steps {
    powerShell '.\\scripts\\build-chocolatey.ps1 -Publish'
  }
  publishers {
    gitHubIssueNotifier {}
  }
}
</pre>

<p>This works well, but what if we want to use the exact same trigger for another project? Sure, we could copy and paste it, but that becomes unmaintainable pretty quickly. Instead, we can take advantage of the fact that Job DSL configuration files are <a href="http://groovy-lang.org/">Groovy</a> scripts, and simply pull the shared configuration out into its own separate function:</p>

<pre class="brush: plain">
def yarnStableVersionChange = {
  triggerContext -> triggerContext.with {
    urlTrigger {
      cron 'H/15 * * * *'
      url('https://yarnpkg.com/latest-version') {
        inspection 'change'
      }
    }
  }
}
</pre>

<p>Now we can call that function within the job definition, passing the <a href="http://groovy-lang.org/closures.html#_delegate_of_a_closure">delegate of the closure</a>:</p>
<pre class="brush: plain">
job('yarn-chocolatey') {
  ...
  triggers {
    yarnStableVersionChange delegate
  }
  ...
}
</pre>

<p>Now whenever we want to create a new job using the same trigger, we can simply reuse the <code>yarnStableVersionChange </code> function!</p>

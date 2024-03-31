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

# This post is originally from Daniel15's Blog at https://d.sb/2017/03/reusing-job-dsl-config

---

[Job DSL](https://github.com/jenkinsci/job-dsl-plugin/wiki) is an excellent plugin for Jenkins, allowing you to configure your Jenkins jobs through code rather than through the Jenkins UI. This allows you to more easily track changes to your Jenkins jobs, and revert to old versions in case of any issues. As an example, for the [Yarn project](https://yarnpkg.com/), we have a Jenkins job to publish a [Chocolatey](https://chocolatey.org/) package whenever a new stable Yarn version is out. The configuration for a Jenkins job to do that might look something like this:

```plain
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
```

This works well, but what if we want to use the exact same trigger for another project? Sure, we could copy and paste it, but that becomes unmaintainable pretty quickly. Instead, we can take advantage of the fact that Job DSL configuration files are [Groovy](http://groovy-lang.org/) scripts, and simply pull the shared configuration out into its own separate function:

```plain
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
```

Now we can call that function within the job definition, passing the [delegate of the closure](http://groovy-lang.org/closures.html#_delegate_of_a_closure):

```plain
job('yarn-chocolatey') {
  ...
  triggers {
    yarnStableVersionChange delegate
  }
  ...
}
```

Now whenever we want to create a new job using the same trigger, we can simply reuse the `yarnStableVersionChange ` function!

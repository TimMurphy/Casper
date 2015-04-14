using System;
using System.IO;
using System.Threading.Tasks;
using Anotar.LibLog;
using Casper.Domain.Features.Authors;
using LibGit2Sharp;
using NullGuard;

namespace Casper.Data.Git.Git
{
    public class GitRepository : IGitRepository
    {
        private readonly IGitRepositoryOptions _options;

        public GitRepository(IGitRepositoryOptions options)
        {
            _options = options;
        }

        public Task CommitAsync(GitBranches branch, string relativePath, string comment, IAuthor author)
        {
            return Task.Run(() => Commit(branch, relativePath, comment, author));
        }

        private void Commit(GitBranches branch, string relativePath, string comment, IAuthor author)
        {
            LogTo.Trace("Commit(branch: {0}, relativePath: {1}, comment: {2}, author: {3})", branch, relativePath, comment, author);

            using (var repo = new Repository(WorkingDirectory.FullName))
            {
                repo.Checkout(branch.Name());
                repo.Stage(relativePath);
                repo.Commit(comment, author.ToSignature());
            }
        }

        public Task PushAsync(GitBranches branch)
        {
            LogTo.Trace("PushAsync(branch: {0})", branch);

            return Task.Run(() => Push(branch.Name()));
        }

        private void Push(string branchName)
        {
            using (var repo = new Repository(WorkingDirectory.FullName))
            {
                var branch = repo.Checkout(branchName);
                var branchRefs = string.Format("{0}:{1}", branch.CanonicalName, branch.UpstreamBranchCanonicalName);
                var signature = new Signature(_options.UserName, _options.Password, DateTime.UtcNow);

                LogTo.Debug("Pushing...  {{ remote: {0}, branchRefers: {1}, userName: {2}, password: {3}, when: {4} }}", branch.Remote.Url, branchRefs, _options.UserName, string.IsNullOrWhiteSpace(_options.Password) ? "is null or whitespace" : "*****", signature.When);

                var options = new PushOptions()
                {
                    CredentialsProvider = GitCredentials
                };

                repo.Network.Push(branch.Remote, branchRefs, pushOptions: options);

                LogTo.Debug("Pushed...  {{ remote: {0}, branchRefers: {1}, userName: {2}, password: {3}, when: {4} }}", branch.Remote, branchRefs, _options.UserName, string.IsNullOrWhiteSpace(_options.Password) ? "is null or whitespace" : "*****", signature.When);
            }
        }

        public Credentials GitCredentials(string url, [AllowNull] string usernameFromUrl, SupportedCredentialTypes types)
        {
            LogTo.Trace("GitCredentials(url: {0}, usernameFromUrl: {1}, types: {2})", url, usernameFromUrl, types);

            if (types != SupportedCredentialTypes.UsernamePassword)
            {
                throw new ArgumentOutOfRangeException("types", types, "Value must be SupportedCredentialTypes.UsernamePassword.");
            }

            return new UsernamePasswordCredentials
            {
                Username = _options.UserName,
                Password = _options.Password
            };
        }

        public DirectoryInfo WorkingDirectory { get { return _options.WorkingDirectory; } }
    }
}
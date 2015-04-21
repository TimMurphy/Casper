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
        private readonly IGitRepositorySettings _settings;

        public GitRepository(IGitRepositorySettings settings)
        {
            _settings = settings;
        }

        public Task CommitAsync(GitBranches branch, string relativePath, string comment, Author author)
        {
            return Task.Run(() => Commit(branch, relativePath, comment, author));
        }

        private void Commit(GitBranches branch, string relativePath, string comment, Author author)
        {
            LogTo.Trace("Commit(branch: {0}, relativePath: {1}, comment: {2}, author: {3})", branch, relativePath, comment, author);

            using (var repo = new Repository(WorkingDirectory.FullName))
            {
                repo.Checkout(branch.Name());
                repo.Stage(relativePath);
                repo.Commit(comment, author.ToGitSignature(), Committer());
            }
        }

        private Signature Committer()
        {
            return new Signature(_settings.UserName, _settings.Password, DateTime.UtcNow);
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
                var signature = new Signature(_settings.UserName, _settings.Password, DateTime.UtcNow);

                LogTo.Debug("Pushing...  {{ remote: {0}, branchRefers: {1}, userName: {2}, password: {3}, when: {4} }}", branch.Remote.Url, branchRefs, _settings.UserName, string.IsNullOrWhiteSpace(_settings.Password) ? "is null or whitespace" : "*****", signature.When);

                var options = new PushOptions()
                {
                    CredentialsProvider = GitCredentials
                };

                repo.Network.Push(branch.Remote, branchRefs, pushOptions: options);

                LogTo.Debug("Pushed...  {{ remote: {0}, branchRefers: {1}, userName: {2}, password: {3}, when: {4} }}", branch.Remote, branchRefs, _settings.UserName, string.IsNullOrWhiteSpace(_settings.Password) ? "is null or whitespace" : "*****", signature.When);
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
                Username = _settings.UserName,
                Password = _settings.Password
            };
        }

        public DirectoryInfo WorkingDirectory { get { return _settings.WorkingDirectory; } }
    }
}
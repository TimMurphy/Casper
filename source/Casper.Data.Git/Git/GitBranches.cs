namespace Casper.Data.Git.Git
{
    public enum GitBranches
    {
        Master
    }

    public static class GitBranchesExtensions
    {
        public static string Name(this GitBranches branch)
        {
            return branch.ToString().ToLower();
        }
    }
}
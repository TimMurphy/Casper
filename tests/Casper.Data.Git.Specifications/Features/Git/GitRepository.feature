Feature: GitRepository
	As a developer
	I want to interact with a Git repository

Scenario: CommitAsync(branch, relativePath, comment) with valid repository and parameters
	Given branch is valid
		And relativePath is valid
		And comment is valid
		And author is valid
	When I call CommitAsync(branch, relativePath, comment)
	Then the git repository should be updated with relativePath and comment


Scenario: PushAsync(GitBranches master)
	Given a git repository has repositories to push to its remote origin
	When I call PushAsync(branch)
	Then the git repository should be pushed to its remote origin

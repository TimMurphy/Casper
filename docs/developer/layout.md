To follow is the layout for this project. [Original source](https://gist.github.com/TimMurphy/e4237a9c3a135d18cd57).

```
$/
  artifacts/
  build/
  docs/
  docs/consumer
  docs/developer
  packages/
  samples/
  scripts/
  source/
  tests/
  .gitignore
  .gitattributes
  Casper.sln
  build.cmd
  LICENSE.txt
  NuGet.config
  README.md
```

- `artifacts` - Build outputs go here. Doing a build.cmd generates artifacts here (nupkgs, dlls, pdbs, etc.)
- `build` - Build customizations (custom msbuild files/psake/fake/albacore/etc) scripts
- `docs` - Documentation stuff, markdown files, help files etc.
    - `docs/consumer` - Documentation for consumers (users) of a Casper based website.
    - `docs/developers` - Documentation for Casper developers.
- `packages` - NuGet packages
- `samples` - Sample projects.
- `scripts` - Scripts for the solution that are not directly related to the build. e.g. DeleteFilesNotUnderSourceControl.bat.
- `source` - Main projects
- `tests` - Test projects
- `build.cmd` - Bootstrap the build for windows

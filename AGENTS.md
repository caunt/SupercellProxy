# Repository conventions

- Keep each source file under 1,000 lines and split larger implementations by coherent responsibility.
- Use a partial class only when the combined class implementation exceeds 1,000 lines. Otherwise, keep the class in one file.
- Never pass `--no-restore` or `--no-build` to `dotnet` commands; rely on the SDK's incremental restore and build behavior.

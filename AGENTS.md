# Repository conventions

- Keep each source file under 1,000 lines and split larger implementations by coherent responsibility.
- Use a partial class only when the combined class implementation exceeds 1,000 lines. Otherwise, keep the class in one file.
- Never pass `--no-restore` or `--no-build` to `dotnet` commands; rely on the SDK's incremental restore and build behavior.
- Do not give a type the same name as its containing namespace or directory segment; avoid shapes such as `Fields/Fields.cs` or `Fields.Fields`.
- Never use `while (true)`; express loop termination in the condition or with a meaningfully named state variable.
- Define shared constants once in their authoritative owner. Packet ID mappings belong only in `MessageRegistry`; static game-asset paths belong only in `GameAssetFiles`; consumers must reference those definitions instead of repeating literals or aliases.

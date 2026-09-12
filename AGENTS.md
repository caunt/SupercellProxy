# Repository conventions

- Never create unit test projects.
- Never pass `--no-restore` or `--no-build` to `dotnet` commands; rely on the SDK's incremental restore and build behavior.
- Never add diagnostic suppressions (`#pragma`, `SuppressMessage`, `NoWarn`); fix the code.
- Use `[LoggerMessage]` for logging.
- Use concrete types for all JSON serialization and deserialization; no untyped JSON access.
- Define shared constants once in their authoritative owner. Packet ID mappings belong only in `MessageRegistry`; static game-asset paths belong only in `GameAssetFiles`; consumers must reference those definitions instead of repeating literals or aliases.

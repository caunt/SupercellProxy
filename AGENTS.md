# Solution projects

```text
+-----------------------------------+---------------------------------------------------------------------------------------------------------------+
| Project                           | Purpose                                                                                                       |
+-----------------------------------+---------------------------------------------------------------------------------------------------------------+
| SupercellProxy.Networking         | Public protocol, transport, cryptography, session, client, and proxy library.                                 |
| SupercellProxy.Capture            | Git-ignored live capture host for protocol traffic plus native RNG, checksum, and turn recording.             |
| SupercellProxy.Simulation         | Git-ignored managed port of game state, actions, turns, RNG, and checksums; native behavior is authoritative. |
| SupercellProxy.Buyer              | Git-ignored Hay Day roadside-shop buyer built on Simulation.                                                  |
| SupercellProxy.Assistant          | Git-ignored Hay Day farm assistant: a queued task loop with recurring background publishers.                  |
| SupercellProxy.Replay             | Git-ignored replay, inspection, and verification of retained captures.                                        |
| SupercellProxy.Keys               | Public tool that searches and downloads IPAs, extracts server public keys, and updates KEYS.md.               |
+-----------------------------------+---------------------------------------------------------------------------------------------------------------+
```

# Repository conventions

- Never create unit test projects.
- Never pass `--no-restore` or `--no-build` to `dotnet` commands; rely on the SDK's incremental restore and build behavior.
- Never add diagnostic suppressions (`#pragma`, `SuppressMessage`, `NoWarn`); fix the code.
- Use `[LoggerMessage]` for logging.
- Use concrete types for all JSON serialization and deserialization; no untyped JSON access.
- Define shared constants once in their authoritative owner. Packet ID mappings belong only in `MessageRegistry`; static game-asset paths belong only in `GameAssetFiles`; consumers must reference those definitions instead of repeating literals or aliases.

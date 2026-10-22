# Agent guidance

This fork carries Vizor-specific desktop changes. Read [VIZOR.md](VIZOR.md) before changing the fork integration, native lifetime, configuration hook, or local packages. [Readme.md](Readme.md) describes the upstream components and Nuke packaging pipeline.

## Scope and verification

- Confirm the consumer-pinned revision and intended integration branch before editing. The latest owner integration currently lives on `codex/46-native-lifecycle`, descending from `codex/vizor-nativeaot`; upstream `main` is a separate line. Create new work branches with `bugfix/` unless the user specifies another name. Never advance a consumer gitlink implicitly.
- Preserve upstream history, MIT notices, locked dependency closure, and the native handle ownership and event-origin contracts in VIZOR.md. Do not infer cross-platform, release, leak, or performance acceptance from the macOS prototype.
- For builds, initialize the pinned `build-common` submodule with `git submodule update --init --recursive` and use the SDK policy in [global.json](global.json). The existing Nuke targets in [build/Build.cs](build/Build.cs) define the upstream build/test/package pipeline; fork NativeAOT acceptance additionally requires the consumer fresh-checkout proof described in VIZOR.md.
- Verify changes with the relevant tests and consumer proof. Documentation-only changes need link, factual consistency, and diff checks; do not claim runtime validation from those checks. Report unavailable verification explicitly. Local packing does not authorize feed publication.

## Independent architecture and review

Start each delegated architect, advisor, and reviewer with empty conversation context. With `spawn_agent`, use `fork_turns="none"`; otherwise use the tool's equivalent fresh-context mode. Provide only the concrete question, repository path and relevant revisions, scope, constraints, and evidence locations. Do not pass the author's reasoning, preferred verdict, or earlier agents' conclusions.

Each agent must inspect the relevant implementation and contracts itself, gather evidence, and state its own conclusions and unresolved uncertainty. Keep the assignment focused on the actual topic. The final reviewer must be separate from the implementer and design advisor, inspect the final diff at the named revision, and report actionable findings or a qualified pass. If separate review is unavailable, disclose that limitation instead of labeling self-review independent.

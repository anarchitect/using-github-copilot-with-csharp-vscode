# Validation report

**Reviewed on:** 18 November 2026 (17 November UTC)
**Source page:** [microsoft/Mastering-GitHub-Copilot-for-Paired-Programming › Using-GitHub-Copilot-with-CSharp](https://github.com/microsoft/Mastering-GitHub-Copilot-for-Paired-Programming/tree/main/Using-GitHub-Copilot-with-CSharp)
**Target:** local VS Code (1.146) with the **Copilot harness**. Codespaces and the Local harness are out of scope.

**Status key:** ✅ Current · ⚠️ Changed (still possible, but the steps, names, or defaults are different) · ❌ Outdated or removed · ➖ Out of scope for this repo

> **Method.** Product claims were checked against official VS Code docs, GitHub Docs and Changelog, and Microsoft Learn / .NET sources; terminology was refreshed on 18 November 2026. Runtime checks are recorded separately below: source verification is not proof of editor interaction. Copilot output is not deterministic, so exercise checkpoints describe outcomes, not exact generated code.

## Beginner-company review

**Verdict:** the starter runs locally, representative agent-generated changes work, and the added C# method-stub exercise was completed with genuine ghost text in an isolated VS Code 1.146 editor. Earlier CLI checks used an environment whose normal editor was 1.145.2; the later 1.146 observations are scoped separately below. The entire workshop has **not** been replayed end to end in 1.146. CLI checks and illustrative screenshots do not prove unobserved editor interactions or preview availability.

| Finding | Correction |
| --- | --- |
| Initial restore selected `Microsoft.OpenApi` 2.0.0 and reported NU1903 | Updated ASP.NET Core package references from 10.0.0 to 10.0.15, resolving `Microsoft.OpenApi` 2.13.0. The advisory affects parsing untrusted OpenAPI documents; this is not a claim that the sample exposes that exploit path. [Advisory](https://github.com/advisories/GHSA-v5pm-xwqc-g5wc). |
| Scalar used a floating `2.*` version; the SDK allowed future major/prerelease versions | Pinned Scalar 2.20.0. `global.json` requires a stable .NET 10 SDK (10.0.100 or later, `latestFeature` roll-forward), so beginners can use any installed 10.0 SDK. BackEnd and test package locks are committed and restored in locked mode in CI. FrontEnd has no lock file because Blazor implicitly requests SDK-versioned assets that differ across 10.0 feature bands. [SDK matching rules](https://learn.microsoft.com/dotnet/core/tools/global-json). |
| Fahrenheit conversion used an approximate divisor and truncation | Applied the exact conversion and rounding to the nearest whole degree, with six reference-point tests. |
| Running processes were left active before edits/builds | Added explicit stop, build, test, and restart steps to avoid Windows output locks and stale endpoints. |
| NES had no guaranteed use of the renamed property | Added a dependent reference and a manual/rename-symbol fallback; a missing suggestion does not block learning. |
| Demo reset discarded all uncommitted work | Replaced blanket discard/clean commands with inspection and a fresh disposable clone. |
| Manual permissions promised approval for every action | Explained existing approvals/read-only actions; corrected **Allow all** versus **Autopilot** mode terminology. [Approval reference](https://code.visualstudio.com/docs/agents/run/approvals). |
| Personal instructions were directed to the agents directory | Separated personal agents, modular instructions, and general instructions. [Migration destinations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations#_move-user-agents-and-instructions). |
| Local execution could be mistaken for offline inference | Distinguished local file/terminal operations from hosted-model context transmission. [Information flow](https://code.visualstudio.com/docs/agents/concepts/trust-and-safety#_where-information-goes). |
| Corporate onboarding, storage, and licensing boundaries were incomplete | Added approved-seat/policy checks, synthetic-only data, in-memory reset behaviour, optional preview gates, and C# Dev Kit eligibility including classroom/open-source exceptions. [Dev Kit licence](https://marketplace.visualstudio.com/items/ms-dotnettools.csdevkit/license). |

The .NET 10 LTS baseline remains intentional, not a claim that it is the newest major version. The [support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core), updated 10 November 2026, lists .NET 11 as current STS and .NET 10.0.15 as the current .NET 10 patch; .NET 10 is supported until 14 November 2028.

## Headline: the Local harness is retiring

| Item | Status | Evidence |
| --- | --- | --- |
| The Local harness (VS Code's original built-in agent harness) | ❌ Deprecated in VS Code 1.146; **removed in February 2027** | [Changelog, 16 Nov 2026](https://github.blog/changelog/2026-11-16-vs-code-retires-the-local-harness-in-february-2027/) · [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) |
| The Copilot harness (built on the Copilot SDK, shared with Copilot CLI and the GitHub Copilot app; default for new sessions since 1.143; runs locally) | ✅ Used throughout this repo | [VS Code 1.146 release notes](https://code.visualstudio.com/updates/v1_146) · [Move to the Copilot harness](https://code.visualstudio.com/docs/agents/run/move-to-copilot) |

> Note: the changelog says the Local harness is removed in **February 2027**. It doesn't name a specific day.

## Source module, item by item

| # | Source claim or step | Status | What's current (Nov 2026) | Evidence |
| --- | --- | --- | --- | --- |
| 1 | Create a GitHub Codespace from the `github/dotnet-codespaces` template | ➖ | Replaced with a local clone + local `dotnet` CLI and a self-contained sample in this repo. Codespaces itself still exists but isn't part of a local-VS-Code training. | [Set up Copilot in VS Code](https://code.visualstudio.com/docs/setup/copilot) |
| 2 | Sample targets .NET 8 | ❌ | .NET 8 (and 9) reached end of support on **10 Nov 2026**. Use **.NET 10 (LTS, to Nov 2028)**. The upstream `dotnet-codespaces` sample also moved to .NET 10. | [.NET blog: .NET 8 and 9 end of support](https://devblogs.microsoft.com/dotnet/dotnet-8-9-end-of-support/) · [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) |
| 3 | Install/enable the Copilot extensions and sign in | ⚠️ | Copilot is built into VS Code. Sign in from the Status Bar Copilot menu or the Accounts menu. Without a plan, sign-in enrolls eligible accounts in **Copilot Free**. | [Set up Copilot in VS Code](https://code.visualstudio.com/docs/setup/copilot) |
| 4 | Comment-driven code completion (ghost text, `Tab` to accept) — e.g. a `Customer` class with an email validator | ✅ | **Ghost text suggestions** are one type of **inline suggestions**; the other is NES. The guide now also completes a C# method stub and checks three outputs. | [Ghost text suggestions](https://code.visualstudio.com/docs/editing/ai-powered-suggestions#_ghost-text-suggestions) · [Getting your first suggestions](https://code.visualstudio.com/docs/editing/ai-powered-suggestions#_getting-your-first-suggestions) |
| 5 | Next Edit Suggestions (gutter arrow; enable if missing) | ✅ | **Next edit suggestions (NES)** propose another edit/location. The arrow shows its direction relative to the cursor; hover or navigate before accepting. A diagnostic underline does not identify the proposed change. `github.copilot.nextEditSuggestions.enabled` controls availability. | [Inline suggestions](https://code.visualstudio.com/docs/editing/ai-powered-suggestions) · [AI settings reference](https://code.visualstudio.com/docs/agents/reference/ai-settings) |
| 6 | Inline chat with `Ctrl+I` | ⚠️ | **Editor inline chat**: select code, `Ctrl+I`, enter a prompt, submit with `Enter`, then review with **Keep** / **Undo**. In active editing sessions **Ask in Chat** may replace it; `inlineChat.askInChat: false` selects regular inline chat. | [Inline chat](https://code.visualstudio.com/docs/chat/inline-chat) |
| 7 | "`Ctrl+Shift+I` opens chat" | ❌ | **Open the Chat view: `Ctrl+Alt+I`**. On Windows, `Ctrl+Shift+I` now switches to agents in the Chat view. | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) |
| 8 | Chat slash commands `/explain`, `/doc`, `/fix`, `/generate`, `/help` | ⚠️ / ❌ | Copilot harness slash commands are `/plan`, `/compact`, `/fork`, and `/<skill-name>` (type `/` to list them). `/generate` is gone. Inline chat still recognizes `/doc`, `/explain`, `/fix`, `/tests` if typed, but they no longer appear in autocomplete. **This repo uses plain-English prompts instead.** | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) · [microsoft/vscode#330866](https://github.com/microsoft/vscode/issues/330866) |
| 9 | Ask / Edit / Agent **modes** | ⚠️ | Distinguish **Session Target**, **Agent**, **Language model**, **mode**, and **Permissions**. Ask, Agent, and Plan are built-in agents; Copilot also has Plan/Interactive/Autopilot modes. Set Session Target before the first prompt; an existing chat uses a harness prefix. **Edit mode is deprecated** — use Agent. | [Chat view](https://code.visualstudio.com/docs/agents/run/chat-view) · [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) · [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) |
| 10 | "Agent mode is selected by default; you can choose your model" | ⚠️ | The **Copilot harness** is the default target. The model picker now offers **Copilot Fusion** (routes/orchestrates models; you pick an effort level) and Auto. | [Copilot Fusion changelog](https://github.blog/changelog/2026-10-28-route-and-orchestrate-models-with-github-copilot-fusion/) · [Choose a model](https://code.visualstudio.com/docs/agents/guides/choose-a-model) |
| 11 | Add context (files, folders), open editor tab used as context | ✅ | **Add Context** and `#`-mentions still work. | [AI features cheat sheet](https://code.visualstudio.com/docs/agents/reference/ai-features-cheat-sheet) |
| 12 | Agent multi-task prompt (Customer endpoint, CORS, DI, controllers, docs, xUnit tests) | ⚠️ | Simplified to a minimal-API route group; CORS isn't needed for this server-side Blazor client. `/plan` precedes implementation. **Submit Feedback** revises; **Implement Plan** starts execution; **Approve Plan Only** does not. Tracked edits are saved before **Keep**, which marks review complete. | [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) · [Chat view](https://code.visualstudio.com/docs/agents/run/chat-view) |
| 13 | Approving agent actions with *Continue* | ⚠️ | Copilot-harness permission levels are **Manual permissions**, **Assisted permissions** (LLM judge), and **Allow all**. **Autopilot is a mode**, not a permission level. Manual uses existing tool/URL/terminal rules and does not guarantee a prompt for every action. Use one-time approvals where offered. | [Manage approvals and permissions](https://code.visualstudio.com/docs/agents/run/approvals) |
| 14 | Repository instructions in `.github/copilot-instructions.md` | ✅ | Supported by the Copilot harness. `AGENTS.md` is also supported. | [Custom instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions) · [GitHub Docs](https://docs.github.com/copilot/how-tos/configure-custom-instructions-in-your-ide/add-repository-instructions-in-your-ide) |
| 15 | Path-specific `.github/instructions/*.instructions.md` with `applyTo` | ✅ | Supported. In Copilot sessions, `applyTo` targets the files the agent works with. | [Custom instructions reference](https://code.visualstudio.com/docs/agents/reference/custom-instructions-reference) |
| 16 | Reusable prompt files (`*.prompt.md`) | ❌ (Copilot harness) | Not loaded by the Copilot harness. Convert to **agent skills**. | [Migrate customizations](https://code.visualstudio.com/docs/agent-customization/migrate-customizations) |
| 17 | "Agent mode supports MCP" | ⚠️ | Still true, but the Copilot harness uses the portable **`mcpServers`** format in workspace-root `.mcp.json` or `~/.copilot/mcp-config.json`. `.vscode/mcp.json` (`servers` format) is the editor/Local format; **Chat: Open Customizations > Migrations** converts it. **Copilot connectors** (GitHub-managed MCP, e.g. Microsoft Learn) are new and in **preview**. | [MCP configuration reference](https://code.visualstudio.com/docs/agents/reference/mcp-configuration) · [Copilot connectors (preview)](https://code.visualstudio.com/docs/agent-customization/copilot-connectors) · [Microsoft Learn MCP Server](https://learn.microsoft.com/training/support/mcp) |

## New since the source module (included where relevant)

| Feature | Status | Used in | Evidence |
| --- | --- | --- | --- |
| Copilot harness + Session Target picker | GA, default | Part 2 | [Choose an agent harness](https://code.visualstudio.com/docs/agents/run/agent-harnesses) |
| Plan agent (`/plan`) with plan review card | GA | Exercise 5 | [Plan work with agents](https://code.visualstudio.com/docs/agents/run/planning) |
| Copilot Fusion | GA, all plans; effort tiers and model access vary by plan/policy | Part 2 | [Changelog](https://github.blog/changelog/2026-10-28-route-and-orchestrate-models-with-github-copilot-fusion/) |
| Assisted permissions | Copilot harness | Exercise 6 tip | [Approvals](https://code.visualstudio.com/docs/agents/run/approvals#_permission-levels) |
| Copilot connectors | **Preview** | Exercise 9 (if MCP is allowed) | [Copilot connectors](https://code.visualstudio.com/docs/agent-customization/copilot-connectors) |
| Canvases | **Preview** | Learn more only | [Canvases](https://code.visualstudio.com/docs/agents/run/canvases) |

## Demo walkthrough (DEMO1 / DEMO2)

Source: [Azure-Samples/octocat-supply › demo-steps.md](https://github.com/Azure-Samples/octocat-supply/blob/main/demo/walkthroughs/general/demo-steps.md) (DEMO1 and DEMO2 only). Translated to this repo's C# sample in [demo/walkthrough.md](demo/walkthrough.md). No OctoCAT Supply code was copied.

| # | Original demo step | Status | Translation in this repo |
| --- | --- | --- | --- |
| D1 | `[Ask Mode]` project overview prompt | ⚠️ | Same prompt, using the **Ask** agent in a Copilot harness session. ("Ask Mode" → Ask agent.) |
| D2 | `make install` / `npm install`, `make dev` | ➖ | `dotnet build` (restores NuGet automatically) and `dotnet run --project src/BackEnd` / `src/FrontEnd`. |
| D3 | Open `order.ts`, `ordersRepo.ts`, `routes/order.ts`; add `// Count orders by branch` between async methods | ✅ (concept) | Open `WeatherForecast.cs`, `WeatherRepository.cs`, `Program.cs`; add `// Count forecasts by city` between `GetByCityAsync` and `GetCitiesAsync`; accept the inline suggestion. |
| D4 | `[Agent Mode]` "Add a retrieval endpoint … update the API documentation … Do not deploy or test yet" | ⚠️ | Same intent with the **Agent** agent; documentation = OpenAPI `WithName`/`WithSummary`/`WithDescription`. The count route and unknown-city result are explicit. Build/test before starting the changed app. |
| D5 | Verify at `http://localhost:3000/api-docs` (Swagger) | ⚠️ | Verify at `http://localhost:5100/scalar` (ASP.NET Core OpenAPI + Scalar). Sample added `Microsoft.AspNetCore.OpenApi` and `Scalar.AspNetCore` for this. |
| D6 | `#codebase` and `#fetch <url>` in the deep-analysis prompt | ❌ (as written) | These are **Local harness** tool reference names; the docs say not to assume they map to Copilot harness tools. Rewritten as plain language ("analysis of this codebase", "use <URL> as a reference"); the agent searches the workspace and fetches the URL with approval. [Tools and context reference](https://code.visualstudio.com/docs/agents/reference/tools-reference) |
| D7 | Fetch W3Schools *TypeScript* best practices | ⚠️ | This sample is C#, so it uses Microsoft Learn's [.NET coding conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions). |
| D8 | Update `.github/copilot-instructions.md` | ✅ | Still the Copilot harness's repository instructions file. Also shown: **Chat: Open Customizations > Instructions > +**. [Custom instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions) |
| D9 | Optional persona added to the instructions file | ✅ | Kept as **Optional, for fun** (Neil Armstrong / James Bond examples, new chat, remove afterwards). A separate optional plain-language, no-unexecuted-success-claims line is offered as a team-friendly alternative. |
| D10 | `[Agent Mode]` code review against the instructions file | ✅ | Same prompt, plus "list findings first … run dotnet build and dotnet test" so beginners see verification. |

## Walkthrough screenshots

The README uses five cropped VS Code 1.146 screenshots: chat controls, ghost text inside a method stub, the NES gutter indicator, the inline-chat prompt, and plan review. Only the new ghost-text image was added for the terminology revision, so learners can distinguish dimmed completion at the cursor from the NES arrow. Captures are from the 18 November 2026 rehearsal; the NES image uses the screenshot supplied during that walkthrough, with numbered callouts. Unrelated panels, account details, and local paths are excluded.

These are visual signposts, not proof that the complete workshop passed. The inline-chat image shows an unsubmitted prompt; the NES image identifies an available suggestion, not its acceptance or correctness. Generated code and plan wording can differ. The starter still excludes `Customer.cs`, which learners create in Exercise 1.

## Method-stub editor rehearsal

The README's `demo/SumNumbersPractice.cs` starting code was opened in a fresh disposable copy, outside all three project directories. Only the empty stub and three calls were supplied; its implementation was not written manually or generated by a chat agent.

| Step | Observed result |
| --- | --- |
| Enter inside the empty method body | VS Code 1.146 displayed genuine ghost text (`int sum = 0;`) at the cursor. |
| Review and accept, then continue on a new line | `Tab` accepted the first suggestion. A second ghost-text suggestion supplied a `foreach` loop and `return sum;`; it was accepted with `Tab` and saved. The screenshot shows this second suggestion before acceptance. |
| Execute the saved file | `dotnet run --file .\demo\SumNumbersPractice.cs` on SDK 10.0.404 returned `6`, `0`, `3`, one per line, with exit code 0. No extra packages or project file were needed. |
| Recovery boundary | The guide documents **Trigger Inline Suggestion** from the official instructions. Automatic suggestions worked in this run; unavailable-feature recovery is not counted as feature success. |

This verifies the new C# exercise, not deterministic future output. The generated practice implementation remains outside the published starter. [.NET file-based apps](https://learn.microsoft.com/dotnet/core/sdk/file-based-apps) documents `--file` and recommends keeping such files outside project directories; this is why the exercise uses `demo`, not `src/BackEnd`.

The starter's 15 tests still passed in that disposable copy. The revised README rendered all five images without image overflow at 1100px and 390px viewport widths. An independent Watson-style review checked product claims and the beginner path separately; it found missing save steps before builds and instruction discovery. Explicit saves were added to the learner and presenter paths. That document review and the new exercise's runtime evidence are not a full-workshop runtime pass.

## Sample app verification

**Observed environment:** Windows ARM64; .NET SDK 10.0.404; ASP.NET Core runtime 10.0.15. The implementation and target both use C#/.NET 10, ASP.NET Core minimal APIs, Blazor InteractiveServer, and xUnit. No framework translation or mocked replacement packages were used.

| Check | Result |
| --- | --- |
| Untouched starter at `ff43a9e` | Built successfully; all 5 original tests passed. Restore emitted the dependency warning noted above. |
| Corrected starter build | `dotnet build WeatherApp.slnx --no-restore --warnaserror`: passed with zero warnings/errors. |
| Corrected starter tests | `dotnet test WeatherApp.slnx --no-build`: 15 passed, covering data shape, case-insensitive lookup, unknown city, city list, OpenAPI metadata, Scalar, Development-only docs, and temperature conversion. |
| Dependency reproducibility | Locked restore of BackEnd and BackEnd.Tests (`--locked-mode`) runs in CI; FrontEnd restores without a lock file. `dotnet list WeatherApp.slnx package --vulnerable --include-transitive` reported no known vulnerabilities in this environment's configured feed. This is not a security certification. |
| Two real running apps | `pwsh -File .\demo\Test-Demo.ps1 -NoBuild -BackEndPort 15100 -FrontEndPort 15200`: passed API, Scalar script, CSS, and 15 server-rendered rows. Owned processes were stopped afterwards. |
| Occupied-port failure | Smoke check refused an already-used port and left the pre-existing API running and responsive. It does not kill other listeners. |
| Real browser, untouched starter | Navigated Home → Weather: 15 rows, five each for Brisbane/Sydney/Melbourne. Stopping the API and reloading showed the intended API-unavailable message. A missing favicon produced a cosmetic 404; no functionality depended on it. |
| Presenter Demo 1 rehearsal | Actual Copilot CLI 1.0.112-0 generated a count method/endpoint in an isolated checkout. Independent build and 5 existing tests passed; live HTTP returned Sydney → `5`, Atlantis → `0`; Scalar displayed metadata and sent Sydney → `200`, body `5`. The inline-completion result was simulated by agent edits, not observed in the editor. |
| Customer exercises rehearsal | Actual Copilot CLI generated Customer CRUD, 400/404 tests, and an InteractiveServer list/add page in another isolated checkout. Independent test execution: 23 passed. Live HTTP verified create `201`, read/update, delete `204`, invalid email `400`, and missing-after-delete `404`. In a real browser, added `Alex Example` / `alex@example.com`, reloaded and saw the saved row; invalid email was blocked and did not add another row. Generated solutions are deliberately not included in the starter. |
| Instruction-guided search exercise | Copilot CLI discovery listed the repository instructions and C# scoped instructions with `applyTo: "**/*.cs"`. A continued session read both files and added FullName search with synchronous in-memory work, XML docs, and tests. Independent execution: 32 total tests passed. Weather implementation was preserved. This demonstrates CLI discovery/use, not VS Code discovery or guaranteed rule enforcement. |
| Continuous integration | [Workflow](.github/workflows/build.yml) now specifies Ubuntu and Windows, locked restore, warnings-as-errors build, xUnit, live-app smoke checks, and retained `TestResults` artifacts. Those new hosted runs are pending publication; local Windows results are not evidence of Linux execution. |

### Remaining manual acceptance

- Replay the full learner sequence in VS Code 1.146 or later using the company's approved account, especially NES acceptance, submitted inline-chat edits, plan implementation, and instruction discovery. The isolated 1.146 method-stub rehearsal and the four other visual signposts do not establish that full sequence.
- Check the company's actual Copilot policy, entitlement/allowance, approved feeds/proxy, and C# Dev Kit eligibility. Public documentation cannot establish a particular company's access.
- Optional connectors/MCP need separate administrator approval and live tool-call verification. Their published configuration was checked; neither preview availability nor organization authorization was demonstrated here.
- Do not infer production readiness from a local training app. There is no authentication, durable customer storage, production hosting, or deployment claim in this module.

For maintenance, review SDK/package patch updates regularly. `global.json` accepts any stable .NET 10 SDK from 10.0.100 (`latestFeature`). The BackEnd and test locks don't depend on the SDK feature band; refresh them with `dotnet restore <project> --use-lock-file --force-evaluate`, inspect the diff, and rerun build/tests/smoke. FrontEnd intentionally has no lock file because Blazor's implicit `Microsoft.AspNetCore.App.Internal.Assets` package follows the installed SDK. CI restores BackEnd and tests with `--locked-mode` to reject unreviewed resolution changes, and restores FrontEnd normally. Avoid locked mode during learner exercises until their new package references have been intentionally resolved.

# Vizor NativeAOT source build

The initial branch `codex/vizor-nativeaot` starts at official upstream b45e042d21d96371bb6d07822a55c85ee5f74d2f (WebView 12.1.0). The parent repository pins a commit, never a moving branch. This fork preserves upstream history and the MIT license.

The current owner integration branch is `codex/46-native-lifecycle`, which descends from `codex/vizor-nativeaot` and includes the desktop code-completion and native lifetime corrections below. Upstream `main` is a separate line; select the intended consumer revision before integrating changes.

Changes for the Vizor macOS ARM64 prototype:
- A synchronous public `ConfigureWKWebViewConfiguration` callback over the borrowed native configuration handle, invoked before WKWebView construction. The callback must neither retain nor release the handle. A throwing callback removes/releases configuration and script-handler resources and rethrows.
- The two desktop source projects target net10.0 only and enable locked package restore. The control project publishes the MIT notice under licenses/Avalonia.Controls.WebView.txt. Both original signed projects are referenced separately (the upstream package uses ILRepack).
- build-common stays an upstream submodule at 8de7dcc6a0dd2d158af87882ebecd631e128e535. Its URL is explicit because the upstream relative URL resolves against the fork owner.

Bootstrap with `git submodule update --init --recursive`. Native proof, app fixture and acceptance limits live in [EyeAuras.Vizor](https://github.com/iXab3r/EyeAuras.Vizor/issues/13). No other-platform, release-package or leak/performance claim follows from this prototype.

To update: select an immutable upstream revision, merge/rebase only unpublished work or merge into the published fork branch without rewriting history, review the hook/build diff and locked dependency closure, run the Vizor fresh-checkout NativeAOT proof, and update the Vizor gitlink in a reviewed commit. If upstream supplies the equivalent hook, delete the local hook change in the same consumer migration.

Desktop code-completion extension (local package version12.0.0-vizor.2):
- WK navigation/message events carry native main-frame identity and the copied source URI, before asynchronous body conversion. These let hosting isolate iframe events and admit the configured application origin.
- Split Core/WebView packages declare their exact dependency; the Core package has its own import entry. The WebView package includes its MIT notice and copies it transitively into a published consuming application. No package feed publication follows from local packing.

- Native lifetime correction proved by repeated real reload/close: NSObject allocation adopts
  its alloc reference; borrowed owned handles still retain. The WKWebView stays adapter-owned
  instead of retaining/disowning an unreleased handle. Dispose releases that reference and
  the allocated Objective-C super-call storage once. Native resource-owner deallocation is
  measured by Vizor's test-only counters, not inferred from AdapterDestroyed alone.

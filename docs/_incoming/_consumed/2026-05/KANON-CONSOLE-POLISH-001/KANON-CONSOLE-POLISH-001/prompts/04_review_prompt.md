# Cursor self-review prompt

Review the implementation as a strict operator-console reviewer.

Check:

1. Are any warnings hidden instead of clarified?
2. Can fixture/demo/generated state be mistaken for authoritative live state?
3. Does any default UI sample contain secret-like values?
4. Does every partial/unavailable state explain why?
5. Are domain-pack lifecycle states truthful to backend data?
6. Are evidence links contextual and accessible?
7. Did any backend scope creep occur?

Return a punch list and fix remaining issues.

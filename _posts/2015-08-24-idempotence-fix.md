---
title: Idempotence Fix
layout: post
---

# Idempotence fix

Release 3.6.5 of [Shuttle.Esb](https://github.com/Shuttle/Shuttle.Esb/releases/tag/v3.6.5) and release 3.4.5 of [Shuttle.Esb.SqlServer](https://github.com/Shuttle/Shuttle.Esb.SqlServer/releases/tag/v3.4.5) contain a refactor of the idempotence handling.

Idempotence is quite tricky to get correct and these releases are aimed at fixing some issues found whilst creating the idempotence guide.

If you have been using idempotence you will need to drop the following tables from your idempotence store and recreate them by running the `IdempotenceServiceCreate.sql` script:

- Idempotence
- IdempotenceDeferredMessage
- IdempotenceHistory

If you have any questions you are welcome to log an issue on the Shuttle.Esb.SqlServer [issue page](https://github.com/Shuttle/Shuttle.Esb.SqlServer/issues).
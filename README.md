# Konzept
Das Spiel ist ein 2D Jump'n Run. Das Ziel ist es, am Ende möglichst viele Münzen gesammelt zu haben und lebend die Level zu beenden.
Ein Spiel Durchlauf besteht darin, alle Level zu beenden. Wenn man stirbt, ist der Durchlauf fehlgeschlagen und man muss neu beginnen.
Besonderheit ist, dass man anstatt den eigenen Charackter hochzuleveln, man PowerDowns einsammeln kann welche den Spieler immer schwächer machen.

# Dependencies
- .NetCoreApp 3.1
- OpenTKNetStandard v1.0.5
- .NET Core 3.1 Runtime for desktop apps

# Starten
Um die Software zu starten wird die [.NET Core 3.1 Runtime for dektop apps](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) benötigt.

# Build und Tests
Das Buildverfahren ist klassisch mit einer beliebigen .Net IDE oder auf der Azure DevOps via CI durchführbar. Man benötigt dafür noch die .Net Core 3.1 SDK.
Alle Tests sind in einem separaten Projekt Namens UnitTests gespeichert und können in der IDE ausgeführt werden.
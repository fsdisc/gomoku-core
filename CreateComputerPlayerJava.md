# Introduction #

To create new computer player (AI) in Java, you start with /Client/java/Player/application/awt project or /Client/java/Player/application/swing project.

# Details #

  * Create new class that inherit from com.bhivef.gomoku.core.Player class.
  * Override 'public void think(final Move move)' method
  * In think method, start with checking whether player is ready or not:
```
if (!getReady()) return;
```
  * When next move is found, call 'makeMove' method to finish:
```
makeMove(new Move(row, column, getPiece()));
```
  * In com.bhivef.gomoku.player.application.awt.Game class or com.bhivef.gomoku.player.application.swing.Game class, add code that create new player to 'createUnknownPlayer' method.
  * In com.bhivef.gomoku.player.application.awt.Runner class or com.bhivef.gomoku.player.application.swing.Runner class, add code for new player to 'getPlayerNames', 'getPlayerIndex', 'getPlayerType' methods
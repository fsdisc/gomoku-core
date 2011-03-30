<?php
session_start();
require('session.php');

$session = '';
if (isset($_SESSION['session'])) {
  $session = $_SESSION['session'];
}

$online = false;
$userid = '';
$username = '';
$data = query(sprintf('select * from %s where id = %s', stable('session'), quote($session)));
if (count($data) > 0) {
  $online = true;
  $userid = $data[0]['user_id'];
}
if ($online) {
  $data = query(sprintf('select * from %s where id = %s', stable('users'), quote($userid)));
  if (count($data) == 0) {
    $online = false;
  } else {
    $username = $data[0]['username'];
  }
}

$ui = 'awt';

?>

<html>
<head>
  <title>Gomoku</title>
  <style>
body { margin: 0px; }
.menubar { border-bottom: solid 1px red; padding: 5px; padding-left: 10px; padding-right: 10px; }
.menubar a { text-transform: none; text-decoration: none; }
.frame { border: solid 1px silver; border-left: solid 5px red; width: 220px; height: 100px; margin: 50px auto 0 auto; padding: 10px; }
  </style>
</head>
<body>
<?php if ($online) { ?>

<div class='menubar'>
<span>Welcome <?php print($username); ?></span>&nbsp;|&nbsp;<span><a href='logout.php'>Logout</a></span>
</div>

<div class='frame'>
<?php if ($ui == 'awt') { ?>

<applet archive='gomoku-core.jar, gomoku-ai.jar, gomoku-player-awt.jar' code='com.bhivef.gomoku.player.applet.awt.Loader' width=220 height=100>
  <param name='session' value='<?php print($session); ?>'>
</applet>

<?php } else if ($ui == 'swing') { ?>

<applet archive='gomoku-core.jar, gomoku-ai.jar, gomoku-player-swing.jar' code='com.bhivef.gomoku.player.applet.swing.Loader' width=220 height=100>
  <param name='session' value='<?php print($session); ?>'>
</applet>

<?php } ?>
</div>

<?php } else { ?>

<div class='menubar'>
<span><a href='register.php'>Register</a></span>&nbsp;|&nbsp;<span><a href='login.php'>Login</a></span>
</div>

<?php } ?>

</body>
</html>

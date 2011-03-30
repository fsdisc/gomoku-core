<?php
session_start();
require('session.php');

$session = '';
if (isset($_SESSION['session'])) {
  $session = $_SESSION['session'];
}
$online = false;
$data = query(sprintf('select * from %s where id = %s', stable('session'), quote($session)));
if (count($data) > 0) {
  $online = true;
}
if ($online) {
  execute(sprintf('delete from %s where id = %s', stable('session'), quote($session)));
}

$_SESSION['session'] = '';
header('Location: index.php');

?>